using UnityEngine;

public class SelectCamera : MonoBehaviour
{
    private Transform playerTransform;
    public Vector3 offset = new Vector3(0, 5, -10); // カメラの位置をプレイヤーから少し後ろ＆上にする
    public float smoothSpeed = 0.125f;              // スムーズに追従させるための補間速度

    [Header("カメラ角度設定")]
    public float rotationSpeed = 5.0f;  // マウスでカメラの向きを変えるスピード
    //上下にカメラ角度を変えたいなら以下2つのパラメータを変更する
    public float minVerticalAngle = 0.0f;  // カメラの上下方向の最小角度
    public float maxVerticalAngle = 0.0f;   // カメラの上下方向の最大角度

    [Header("カメラの初期角度設定")]
    public float initialHorizontalAngle = 0f;  // 初期の水平回転角度
    public float initialVerticalAngle = 20f;   // 初期の垂直回転角度

    private float currentVerticalAngle = 0f; // 現在のカメラの垂直角度
    private float currentHorizontalAngle = 0f; // 現在のカメラの水平方向の角度

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Playerタグのオブジェクトが見つかりません！");
        }

        // 初期角度を設定
        currentHorizontalAngle = initialHorizontalAngle;
        currentVerticalAngle = initialVerticalAngle;
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // マウス入力によるカメラの回転
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
            float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed;

            // 水平回転
            currentHorizontalAngle += horizontalInput;

            // 垂直回転（上限・下限を設定）
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle + verticalInput, minVerticalAngle, maxVerticalAngle);

            // カメラの回転を設定
            Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
            Vector3 rotatedOffset = rotation * offset;

            // カメラの位置を補間して滑らかに
            Vector3 desiredPosition = playerTransform.position + rotatedOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // カメラがプレイヤーを向く（カメラの回転をそのまま使用）
            transform.LookAt(playerTransform);
        }
    }
}
