using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Transform respawnPoint;
    public float invincibilityDuration = 3f;
    public float fallThresholdY = -10f; // Y座標がこの値以下になったらリスポーン
    public bool isRespawn = true;
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

    void Update()
    {
        if (!isInvincible && transform.position.y < fallThresholdY)
        {
            Debug.Log("プレイヤーが落下しました！");
            HandleDeath();
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
            if (isRespawn)
            {
                HandleDeath(); // プレイヤー：リスポーンへ
            }
            else
            {
                Debug.Log($"{gameObject.name} は死亡し、リスポーンしません。");
                if (playerController != null) playerController.enabled = false;
                if (bombThrower != null) bombThrower.enabled = false;
                SetRenderersEnabled(false);
                rb.isKinematic = true;

                // エネミーを削除（シーン遷移に反映される）
                Destroy(gameObject);
            }
        }
    }


    void HandleDeath()
    {
        if (isRespawn && RespawnManager.instance != null)
        {
            RespawnManager.instance.RespawnWithDelay(3f);
        }
        Destroy(gameObject);
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