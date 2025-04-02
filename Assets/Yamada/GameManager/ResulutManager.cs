using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
                                                                                                                                                                                                                           

public class ResulutManager : MonoBehaviour
{
   [Header("�^�C�g���V�[��"), SerializeField]
   private string m_titleScene;
   [Header("�v���C�V�[��"), SerializeField]
   private string m_playScene;

   //�V�[�����^�C�g���V�[���ɐ؂�ւ���
   public void ChangeTitleScene()
   {
      SceneManager.LoadScene(m_titleScene);
   }

   //�V�[�����v���C���V�[���ɐ؂�ւ���
   public void ChangePlayScene()
   {
      SceneManager.LoadScene(m_playScene);
   }
}
 