using UnityEngine;

/// <summary>
/// マウスでキャラを引っ張って動かすコンポーネント
/// ターン制バトルの各キャラクターにアタッチ
/// </summary>
public class BB_CharacterPull : MonoBehaviour
{
    // メインカメラ関連
    private Camera m_MainCamera;
    private Transform m_MainCameraTransform;

    // 物理演算用Rigidbody
    private Rigidbody m_Physics;

    // 力の計算用
    private Vector3 m_CurrentForce = Vector3.zero;
    private Vector3 m_DragStart = Vector3.zero;
    const float MaxMagnitude = 1f; // 力の最大値（マウスの引っ張り）

    public float FixForce = 10f;   // 固定倍率の力
    private bool isSelected = false;

    // キャラ情報
    public int playerID;          // プレイヤーID（チーム区別用）
    public bool canAct = true;    // 行動可能か
    public bool isAlive = true;   // 生存フラグ

    public int maxHP = 100;
    public int currentHP;
    public float speed = 5.0f;      // 力の倍率に使う
    public int attackPower = 10;

    // 初期位置（リスポーン用）
    private Vector3 initialPosition;

    // ターン・発射・停止判定
    private bool isLaunched = false;
    public bool pendingRespawn = false;
private float stopTimer = 0f;
private const float stopThreshold = 0.05f;
private const float requiredStopDuration = 0.5f; // 0.5秒停止し続けたら終了

    void Start()
    {
        // カメラ・物理・初期位置初期化
        m_Physics = GetComponent<Rigidbody>();
        m_MainCamera = Camera.main;
        m_MainCameraTransform = m_MainCamera.transform;

        initialPosition = transform.position;
        currentHP = maxHP;

        // キャラ登録（TurnManagerへ）
        BB_TurnManager.Instance.RegisterCharacter(this);
    }

    void Update()
    {
    if (isLaunched)
    {
        if (m_Physics.linearVelocity.magnitude < stopThreshold)
        {
            stopTimer += Time.deltaTime;

            if (stopTimer >= requiredStopDuration)
            {
                isLaunched = false;
                stopTimer = 0f;
                BB_TurnManager.Instance.EndTurn(this);

                if (pendingRespawn)
                {
                    Respawn();
                    pendingRespawn = false;
                }
            }
        }
        else
        {
            stopTimer = 0f; // 動いたらリセット
        }
    }
    }

    /// <summary>
    /// マウスのワールド座標（XZ平面）を取得
    /// </summary>
    Vector3 GetMousePosition()
    {
        var pos = Input.mousePosition;
        pos.z = m_MainCameraTransform.position.y; // Y軸で合わせる
        pos = m_MainCamera.ScreenToWorldPoint(pos);
        pos.y = 0; // 地面に固定
        return pos;
    }

    void OnMouseDown()
    {
        // 自分のターンなら操作開始
        if (BB_TurnManager.Instance.CanControl(this))
        {
            m_DragStart = GetMousePosition();
            isSelected = true;
            BB_TurnManager.Instance.SetCurrentCharacter(this);
        }
    }

    void OnMouseDrag()
    {
        // マウスドラッグ中に力を計算
        if (isSelected)
        {
            var pos = GetMousePosition();
            m_CurrentForce = pos - m_DragStart;

            // 最大距離制限
            if (m_CurrentForce.sqrMagnitude > MaxMagnitude * MaxMagnitude)
            {
                m_CurrentForce *= MaxMagnitude / m_CurrentForce.magnitude;
            }
        }
    }

    void OnMouseUp()
    {
        // マウスを離したら発射
        if (isSelected)
        {
            Flip(-m_CurrentForce * FixForce * speed);
            isSelected = false;
            isLaunched = true; // 発射状態ON
        }
    }

    /// <summary>
    /// 指定された力でキャラを飛ばす
    /// </summary>
    void Flip(Vector3 force)
    {
        m_Physics.AddForce(force, ForceMode.Impulse);
    }

void OnCollisionEnter(Collision collision)
{
    Debug.Log("OnCollisionEnter 発火");

    var other = collision.gameObject.GetComponent<BB_CharacterPull>();
    if (other == null)
    {
        Debug.Log("相手が CharacterPull を持ってない");
        return;
    }

    Debug.Log($"CurrentCharacter: {BB_TurnManager.Instance.CurrentCharacter.name}, this: {this.name}");

    if (BB_TurnManager.Instance.CurrentCharacter == this && other.playerID != this.playerID)
    {
        Debug.Log("敵にぶつかった！");
        other.TakeDamage(this.attackPower);
        
        ContactPoint contact = collision.contacts[0];
        Vector3 incomingVelocity = m_Physics.linearVelocity;
        Vector3 normal = contact.normal;
        Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);
        m_Physics.linearVelocity = reflectedVelocity * 0.5f;
    }
    else
    {
        Debug.Log("条件不一致：攻撃処理に入らなかった");
    }
}

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public void TakeDamage(int damage)
    {
    if (!isAlive) return;

    currentHP -= damage;
    Debug.Log($"{gameObject.name} took {damage} damage, currentHP: {currentHP}");

    if (currentHP <= 0)
    {
        isAlive = false;
        currentHP = maxHP; // 復活時のHP初期化
        pendingRespawn = true;

        // Rigidbody停止
m_Physics.linearVelocity = Vector3.zero;
m_Physics.angularVelocity = Vector3.zero;


        gameObject.SetActive(false); // 死亡状態：非表示
    }
    }

    /// <summary>
    /// リスポーン処理（初期位置へ戻す）
    /// </summary>
    public  void Respawn()
    {
    transform.position = initialPosition;
    isAlive = true;
    canAct = true;
    gameObject.SetActive(true);
    }
}
