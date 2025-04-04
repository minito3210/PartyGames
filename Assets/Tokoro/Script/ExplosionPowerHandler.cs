using System.Collections;
using UnityEngine;

public class ExplosionPowerHandler : MonoBehaviour
{
    [Header("Power Increments")]
    public float radiusIncrement = 0.1f;
    public float knockbackIncrement = 0.1f;

    private BombThrower bombThrower;

    void Awake()
    {
        bombThrower = GetComponent<BombThrower>();
    }

    public void IncreaseExplosionPower()
    {
        if (bombThrower != null)
        {
            bombThrower.explosionRadiusMultiplier += radiusIncrement;
            bombThrower.knockbackForceMultiplier += knockbackIncrement;
        }
    }
}

