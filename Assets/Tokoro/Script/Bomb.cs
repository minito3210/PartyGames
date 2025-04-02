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

    private GameObject rangeIndicator;

    public event System.Action OnExplode;  // 爆発イベント

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
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        if (rangeIndicator != null)
        {
            Destroy(rangeIndicator);
        }

        OnExplode?.Invoke();  // 爆発イベント発火
        Destroy(gameObject);
    }
}
