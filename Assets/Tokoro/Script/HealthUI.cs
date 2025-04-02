using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health playerHealth; // プレイヤーのHealthスクリプト
    public Text hpText; // HP表示用のText

    void Update()
    {
        if (playerHealth != null && hpText != null)
        {
            hpText.text = "HP: " + playerHealth.GetCurrentHealth();
        }
    }
}
