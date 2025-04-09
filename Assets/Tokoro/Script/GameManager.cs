using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject[] enemies;
    public string nextSceneName = "ResultScene";

    public Text countdownText;
    public float countdownTime = 3f;

    private bool gameStarted = false;
    private PlayerController playerController;
    private BombThrower bombThrower;

    void Start()
    {
        if (player1 != null)
        {
            playerController = player1.GetComponent<PlayerController>();
            bombThrower = player1.GetComponent<BombThrower>();
            if (playerController != null) playerController.enabled = false;
            if (bombThrower != null) bombThrower.enabled = false;
        }

        StartCoroutine(GameStartCountdown());
    }

    IEnumerator GameStartCountdown()
    {
        float currentTime = countdownTime;

        while (currentTime > 0)
        {
            countdownText.text = Mathf.Ceil(currentTime).ToString();
            countdownText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        countdownText.text = "START!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        if (playerController != null) playerController.enabled = true;
        if (bombThrower != null) bombThrower.enabled = true;
        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted) return;

        // プレイヤーが死亡している
        if (player1 == null)
        {
            Debug.Log("プレイヤーが死亡。リザルトへ");
            Invoke(nameof(GoToNextScene), 3f);
            gameStarted = false;
            return;
        }

        // 敵がすべて死亡している
        bool allEnemiesDead = true;
        foreach (GameObject e in enemies)
        {
            if (e != null)
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (allEnemiesDead)
        {
            Debug.Log("敵全滅。リザルトへ");
            Invoke(nameof(GoToNextScene), 3f);
            gameStarted = false;
        }
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}