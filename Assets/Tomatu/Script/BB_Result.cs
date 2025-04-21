using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class BB_Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button titleButton;

    public void Setup(bool isWin)
    {
        resultText.text = isWin ? "You Win!" : "You Lose...";
        gameObject.SetActive(true);

        retryButton.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        titleButton.onClick.AddListener(() => {
            SceneManager.LoadScene("TitleScene");
        });
    }
}
