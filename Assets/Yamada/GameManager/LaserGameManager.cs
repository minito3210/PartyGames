using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaserGameManager : MonoBehaviour
{
   [Header("プレイヤーモデル"), SerializeField]
   private GameObject m_player;
   [Header("タイマー表示用UIテキスト"), SerializeField]
   private TextMeshProUGUI m_timerText;
   [Header("レベルアップ通知画像"), SerializeField]
   private Image m_levelUpImage; // レベルアップ時に表示するImage

   [Header("リザルトシーン"), SerializeField]
   private string m_resultScene;

   public float m_timer { get; private set; } = 0.0f;
   private bool isChangingScene = false;

   private bool hasDisplayed10s = false;
   private bool hasDisplayed30s = false;
   private bool hasDisplayed60s = false;

   private float displayDuration = 2.0f; // Image を表示する秒数

   void Start()
   {
      m_levelUpImage.gameObject.SetActive(false); // 最初は非表示
   }

   void Update()
   {
      if (!isChangingScene)
      {
         m_timer += Time.deltaTime;
         m_timerText.text = m_timer.ToString("F0") + " S";
         GameLevelUp();
      }

      if (m_player.GetComponent<Player>().m_isGameOver && !isChangingScene)
      {
         Debug.Log("当たったことを検知");
         isChangingScene = true;
         StartCoroutine(ChangeSceneAfterDelay(1.0f));
      }
   }

   private IEnumerator ChangeSceneAfterDelay(float delay)
   {
      yield return new WaitForSeconds(delay);
      SceneManager.LoadScene(m_resultScene);
   }

   private void GameLevelUp()
   {
      if (m_timer >= 10.0f && !hasDisplayed10s)
      {
         hasDisplayed10s = true;
         StartCoroutine(ShowLevelUpImage());
      }
      else if (m_timer >= 30.0f && !hasDisplayed30s)
      {
         hasDisplayed30s = true;
         StartCoroutine(ShowLevelUpImage());
      }
      else if (m_timer >= 60.0f && !hasDisplayed60s)
      {
         hasDisplayed60s = true;
         StartCoroutine(ShowLevelUpImage());
      }
   }

   private IEnumerator ShowLevelUpImage()
   {
      m_levelUpImage.gameObject.SetActive(true); // Image を表示
      yield return new WaitForSeconds(displayDuration); // 指定時間待機
      m_levelUpImage.gameObject.SetActive(false); // Image を非表示
   }
}
