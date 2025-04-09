using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSceneChanger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "TitleScene"; // 遷移先のシーン名

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("シーン遷移");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
