using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaserGameManager : MonoBehaviour
{
   [Header("�v���C���[���f��"), SerializeField]
   private GameObject m_player;
   [Header("�^�C�}�[�\���pUI�e�L�X�g"), SerializeField]
   private TextMeshProUGUI m_timerText;
   [Header("���x���A�b�v�ʒm�摜"), SerializeField]
   private Image m_levelUpImage; // ���x���A�b�v���ɕ\������Image

   [Header("���U���g�V�[��"), SerializeField]
   private string m_resultScene;

   public float m_timer { get; private set; } = 0.0f;
   private bool isChangingScene = false;

   private bool hasDisplayed10s = false;
   private bool hasDisplayed30s = false;
   private bool hasDisplayed60s = false;

   private float displayDuration = 2.0f; // Image ��\������b��

   void Start()
   {
      m_levelUpImage.gameObject.SetActive(false); // �ŏ��͔�\��
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
         Debug.Log("�����������Ƃ����m");
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
      m_levelUpImage.gameObject.SetActive(true); // Image ��\��
      yield return new WaitForSeconds(displayDuration); // �w�莞�ԑҋ@
      m_levelUpImage.gameObject.SetActive(false); // Image ���\��
   }
}
