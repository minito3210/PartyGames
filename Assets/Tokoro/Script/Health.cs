using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Transform respawnPoint;
    public float respawnDelay = 3f;
    public float invincibilityDuration = 3f;
    public float blinkInterval = 0.2f;

    private bool isInvincible = false;
    private Renderer[] renderers; // すべての Renderer を取得

    void Start()
    {
        currentHealth = maxHealth;

        // 自分と子オブジェクトからすべての Renderer を取得
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("Renderer が見つかりません！オブジェクトに MeshRenderer があるか確認してください。");
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
        Debug.Log($"{gameObject.name} が {respawnDelay} 秒後にリスポーンします");

        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogWarning("リスポーン地点が設定されていません！");
        }

        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        Debug.Log("無敵状態開始！");

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            ToggleRenderer(false); // 透明化
            yield return new WaitForSeconds(blinkInterval);
            ToggleRenderer(true); // 表示
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval * 2;
        }

        ToggleRenderer(true);
        isInvincible = false;
        Debug.Log("無敵状態終了");
    }

    void ToggleRenderer(bool isVisible)
    {
        foreach (Renderer rend in renderers)
        {
            rend.enabled = isVisible;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
