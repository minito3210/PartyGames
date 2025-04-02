using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
                                                                                                                                                                                                                           

public class ResulutManager : MonoBehaviour
{
   [Header("タイトルシーン"), SerializeField]
   private string m_titleScene;
   [Header("プレイシーン"), SerializeField]
   private string m_playScene;

   //シーンをタイトルシーンに切り替える
   public void ChangeTitleScene()
   {
      SceneManager.LoadScene(m_titleScene);
   }

   //シーンをプレイしシーンに切り替える
   public void ChangePlayScene()
   {
      SceneManager.LoadScene(m_playScene);
   }
}
 