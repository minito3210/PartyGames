using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Transform respawnPoint; // リスポーン地点
    public float invincibilityDuration = 3f; // 無敵時間
    private bool isInvincible = false; // 無敵フラグ

    void Start()
    {
        currentHealth = maxHealth;
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
            Respawn();
        }
    }

    void Respawn()
    {
        Debug.Log($"{gameObject.name} がリスポーンします");

        // 体力を回復
        currentHealth = maxHealth;

        // リスポーン地点に移動
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogWarning("リスポーン地点が設定されていません！");
        }

        // 無敵状態にする
        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        Debug.Log("無敵状態開始！");

        // 一定時間待つ
        yield return new WaitForSeconds(invincibilityDuration);

        // 無敵解除
        isInvincible = false;
        Debug.Log("無敵状態終了");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
