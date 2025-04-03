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
        // 爆発エフェクト
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);

        // 爆風範囲内のオブジェクトを取得
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // ダメージ処理
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // プレイヤーが爆風に当たった場合、ノックバック
            if (hit.CompareTag("Player"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 knockbackDirection = (hit.transform.position - transform.position).normalized;
                    knockbackDirection.y = 0.5f; // 斜め上にノックバック
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                }
            }
        }

        // 範囲表示を削除
        if (rangeIndicator != null)
        {
            Destroy(rangeIndicator);
        }

        OnExplode?.Invoke();
        Destroy(gameObject);
    }
}