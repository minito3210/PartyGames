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
    private Renderer[] renderers; // ���ׂĂ� Renderer ���擾

    void Start()
    {
        currentHealth = maxHealth;

        // �����Ǝq�I�u�W�F�N�g���炷�ׂĂ� Renderer ���擾
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("Renderer ��������܂���I�I�u�W�F�N�g�� MeshRenderer �����邩�m�F���Ă��������B");
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
        {
            Debug.Log($"{gameObject.name} �͖��G��ԂŃ_���[�W�����I");
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
        Debug.Log($"{gameObject.name} �� {respawnDelay} �b��Ƀ��X�|�[�����܂�");

        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogWarning("���X�|�[���n�_���ݒ肳��Ă��܂���I");
        }

        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        Debug.Log("���G��ԊJ�n�I");

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            ToggleRenderer(false); // ������
            yield return new WaitForSeconds(blinkInterval);
            ToggleRenderer(true); // �\��
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval * 2;
        }

        ToggleRenderer(true);
        isInvincible = false;
        Debug.Log("���G��ԏI��");
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
