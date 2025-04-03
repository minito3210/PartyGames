using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Transform respawnPoint;
    public float invincibilityDuration = 3f;
    private bool isInvincible = false;
    private Renderer[] renderers;
    private Collider[] colliders;
    private Rigidbody rb;
    private PlayerController playerController;
    private BombThrower bombThrower; // 爆弾投げスクリプト
    private int defaultLayer;
    private int ghostLayer;

    void Start()
    {
        currentHealth = maxHealth;
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        bombThrower = GetComponent<BombThrower>(); // 追加

        defaultLayer = gameObject.layer;
        ghostLayer = LayerMask.NameToLayer("Ghost");

        if (ghostLayer == -1)
        {
            Debug.LogWarning("Ghost レイヤーが存在しません！レイヤー設定を確認してください。");
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
        {
            Debug.Log($"{gameObject.name} は無敵状態でダメージ無効！");
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        Debug.Log($"{gameObject.name} が消滅...");

        // **操作を無効化**
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (bombThrower != null)
        {
            bombThrower.enabled = false; // 爆弾を投げるスクリプトも無効化
        }

        // プレイヤーを非表示
        SetRenderersEnabled(false);
        rb.isKinematic = true;

        // **Ghost レイヤーが存在する場合のみ適用**
        if (ghostLayer != -1)
        {
            gameObject.layer = ghostLayer;
        }

        yield return new WaitForSeconds(3f); // 3秒後に復活

        Debug.Log($"{gameObject.name} がリスポーン！");

        // 体力回復
        currentHealth = maxHealth;

        // **リスポーン位置を補正**
        Vector3 newPosition = respawnPoint.position;
        if (Physics.Raycast(respawnPoint.position, Vector3.down, out RaycastHit hit, 2f))
        {
            newPosition = hit.point + Vector3.up * 0.5f;
        }
        transform.position = newPosition;

        // プレイヤーを再表示
        SetRenderersEnabled(true);
        rb.isKinematic = false;

        // **元のレイヤーに戻す**
        gameObject.layer = defaultLayer;

        // **操作を復活**
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (bombThrower != null)
        {
            bombThrower.enabled = true; // 爆弾を投げるスクリプトも再有効化
        }

        // 無敵時間の開始（点滅あり）
        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        Debug.Log("無敵状態開始！");

        float blinkInterval = 0.2f;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            ToggleRenderers();
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        isInvincible = false;
        SetRenderersEnabled(true);
        Debug.Log("無敵状態終了");
    }

    void SetRenderersEnabled(bool isEnabled)
    {
        foreach (Renderer rend in renderers)
        {
            rend.enabled = isEnabled;
        }
    }

    void ToggleRenderers()
    {
        foreach (Renderer rend in renderers)
        {
            rend.enabled = !rend.enabled;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
