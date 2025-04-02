using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // 生成するアイテムのリスト
    public float spawnInterval = 5f; // アイテムのスポーン間隔
    public float spawnRangeX = 10f;  // X方向の範囲
    public float spawnRangeZ = 10f;  // Z方向の範囲
    public float spawnHeight = 10f;  // アイテムが落下する高さ

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        if (itemPrefabs.Length == 0) return; // アイテムが登録されていない場合は処理しない

        // ランダムなアイテムを選択
        GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        // ランダムなスポーン位置を設定
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            spawnHeight,
            Random.Range(-spawnRangeZ, spawnRangeZ)
        );

        // アイテムを生成
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }
}
