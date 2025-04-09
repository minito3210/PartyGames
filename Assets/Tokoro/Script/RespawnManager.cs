using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public GameObject playerPrefab;

    public Transform respawnPoint;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void RespawnWithDelay(float delay)
    {
        StartCoroutine(RespawnCoroutine(delay));
    }

    IEnumerator RespawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerPrefab == null)
        {
            Debug.LogError("RespawnManager: playerPrefab Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅI");
            yield break;
        }

        Vector3 spawnPos = respawnPoint.position;
        if (Physics.Raycast(respawnPoint.position, Vector3.down, out RaycastHit hit, 2f))
        {
            spawnPos = hit.point + Vector3.up * 0.5f;
        }

        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }
}
