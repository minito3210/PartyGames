using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionDelay = 3f;
    public GameObject explosionEffect;
    public GameObject explosionRangePrefab;
    public float explosionRadius = 5f;
    public int damage = 20;
    public float rangeHeightOffset = 0.2f;
    public float rangeDelay = 0.5f;
    public float knockbackForce = 10f; // �m�b�N�o�b�N�̋���

    private GameObject rangeIndicator;
    public event System.Action OnExplode;

    void Start()
    {
        StartCoroutine(ShowRangeWithDelay());
        Invoke(nameof(Explode), explosionDelay);
    }

    private IEnumerator ShowRangeWithDelay()
    {
        yield return new WaitForSeconds(rangeDelay);
        rangeIndicator = Instantiate(explosionRangePrefab, transform.position, Quaternion.identity);
        rangeIndicator.transform.SetParent(transform);
        rangeIndicator.transform.localScale = new Vector3(explosionRadius * 2, 0.1f, explosionRadius * 2);
        AdjustRangeHeight();
    }

    private void AdjustRangeHeight()
    {
        Vector3 newPos = rangeIndicator.transform.localPosition;
        newPos.y = rangeHeightOffset;
        rangeIndicator.transform.localPosition = newPos;
    }

    void Explode()
    {
        // �����G�t�F�N�g
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);

        // �����͈͓��̃I�u�W�F�N�g���擾
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // �_���[�W����
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // �v���C���[�������ɓ��������ꍇ�A�m�b�N�o�b�N
            if (hit.CompareTag("Player"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 knockbackDirection = (hit.transform.position - transform.position).normalized;
                    knockbackDirection.y = 0.5f; // �΂ߏ�Ƀm�b�N�o�b�N
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                }
            }
        }

        // �͈͕\�����폜
        if (rangeIndicator != null)
        {
            Destroy(rangeIndicator);
        }

        OnExplode?.Invoke();
        Destroy(gameObject);
    }
}