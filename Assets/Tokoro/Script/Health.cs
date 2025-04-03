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
    private BombThrower bombThrower; // ���e�����X�N���v�g
    private int defaultLayer;
    private int ghostLayer;

    void Start()
    {
        currentHealth = maxHealth;
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        bombThrower = GetComponent<BombThrower>(); // �ǉ�

        defaultLayer = gameObject.layer;
        ghostLayer = LayerMask.NameToLayer("Ghost");

        if (ghostLayer == -1)
        {
            Debug.LogWarning("Ghost ���C���[�����݂��܂���I���C���[�ݒ���m�F���Ă��������B");
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
        Debug.Log($"{gameObject.name} ������...");

        // **����𖳌���**
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (bombThrower != null)
        {
            bombThrower.enabled = false; // ���e�𓊂���X�N���v�g��������
        }

        // �v���C���[���\��
        SetRenderersEnabled(false);
        rb.isKinematic = true;

        // **Ghost ���C���[�����݂���ꍇ�̂ݓK�p**
        if (ghostLayer != -1)
        {
            gameObject.layer = ghostLayer;
        }

        yield return new WaitForSeconds(3f); // 3�b��ɕ���

        Debug.Log($"{gameObject.name} �����X�|�[���I");

        // �̗͉�
        currentHealth = maxHealth;

        // **���X�|�[���ʒu��␳**
        Vector3 newPosition = respawnPoint.position;
        if (Physics.Raycast(respawnPoint.position, Vector3.down, out RaycastHit hit, 2f))
        {
            newPosition = hit.point + Vector3.up * 0.5f;
        }
        transform.position = newPosition;

        // �v���C���[���ĕ\��
        SetRenderersEnabled(true);
        rb.isKinematic = false;

        // **���̃��C���[�ɖ߂�**
        gameObject.layer = defaultLayer;

        // **����𕜊�**
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (bombThrower != null)
        {
            bombThrower.enabled = true; // ���e�𓊂���X�N���v�g���ėL����
        }

        // ���G���Ԃ̊J�n�i�_�ł���j
        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        Debug.Log("���G��ԊJ�n�I");

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
        Debug.Log("���G��ԏI��");
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
