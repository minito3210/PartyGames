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
    public float knockbackForce = 10f; // ノックバックの強さ
    private float radiusMultiplier = 1f;
    private float knockbackMultiplier = 1f;

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

        float finalRadius = explosionRadius * radiusMultiplier;
        rangeIndicator.transform.localScale = new Vector3(finalRadius * 2, 0.1f, finalRadius * 2);

        AdjustRangeHeight();
    }


    private void AdjustRangeHeight()
    {
        Vector3 newPos = rangeIndicator.transform.localPosition;
        newPos.y = rangeHeightOffset;
        rangeIndicator.transform.localPosition = newPos;
    }

    public void SetPowerMultiplier(float radiusMult, float knockbackMult)
    {
        radiusMultiplier = radiusMult;
        knockbackMultiplier = knockbackMult;
    }

    void Explode()
    {
        float finalRadius = explosionRadius * radiusMultiplier;
        float finalKnockback = knockbackForce * knockbackMultiplier;

        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, finalRadius);
        foreach (Collider hit in colliders)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            if (hit.CompareTag("Player"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 knockbackDirection = (hit.transform.position - transform.position).normalized;
                    knockbackDirection.y = 0.5f;
                    rb.AddForce(knockbackDirection * finalKnockback, ForceMode.Impulse);
                }
            }
        }

        if (rangeIndicator != null) Destroy(rangeIndicator);
        OnExplode?.Invoke();
        Destroy(gameObject);
    }
}