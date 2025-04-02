using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    BombCapacity,
    SpeedBoost
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public float effectDuration = 5.0f; // 効果時間（スピードアップ用）
    public float lifetime = 10.0f; // 10秒後に消える

    void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                ApplyEffect(player);
                Destroy(gameObject); // アイテムを消す
            }
        }
    }

    void ApplyEffect(PlayerController player)
    {
        switch (itemType)
        {
            case ItemType.BombCapacity:
                player.IncreaseBombCapacity();
                break;
            case ItemType.SpeedBoost:
                player.StartCoroutine(player.SpeedBoost(effectDuration));
                break;
        }
    }
}
