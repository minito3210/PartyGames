using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    void Update()
    {
        if (player1 == null || player2 == null)
        {
            Debug.Log("ÉQÅ[ÉÄèIóπÅI");
            Invoke(nameof(RestartGame), 3f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
