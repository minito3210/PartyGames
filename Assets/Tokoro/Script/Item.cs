using System.Collections;
using UnityEngine;

public enum ItemType
{
    BombCapacity,
    SpeedBoost,
    ExplosionPower
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public float effectDuration = 5.0f;
    public float lifetime = 10.0f;

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
            BombThrower bombThrower = other.GetComponent<BombThrower>();
            SpeedHandler speedHandler = other.GetComponent<SpeedHandler>();

            ApplyEffect(bombThrower, speedHandler);
            Destroy(gameObject);
        }
    }

    void ApplyEffect(BombThrower bombThrower, SpeedHandler speedHandler)
    {
        switch (itemType)
        {
            case ItemType.BombCapacity:
                bombThrower?.IncreaseBombCapacity();
                break;
            case ItemType.SpeedBoost:
                speedHandler?.ActivateSpeedBoost(effectDuration);
                break;
            case ItemType.ExplosionPower:
                var powerHandler = bombThrower?.GetComponent<ExplosionPowerHandler>();
                powerHandler?.IncreaseExplosionPower();
                break;
        }
    }


}
