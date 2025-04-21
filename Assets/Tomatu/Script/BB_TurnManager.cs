using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BB_TurnManager : MonoBehaviour
{
    public static BB_TurnManager Instance;

    private int currentPlayerTurn = 1; // 1: Player1, 2: Player2
    private int turnCount = 0;

    private Dictionary<int, List<BB_CharacterPull>> playerCharacters = new Dictionary<int, List<BB_CharacterPull>>();
    private Dictionary<int, int> killCount = new Dictionary<int, int>() { { 1, 0 }, { 2, 0 } };

    private HashSet<BB_CharacterPull> usedCharactersThisCycle = new HashSet<BB_CharacterPull>();
    private BB_CharacterPull currentCharacter = null;

    public int maxKillsToWin = 5;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // シングルトンとして
    }

    public void RegisterCharacter(BB_CharacterPull character)
    {
        if (!playerCharacters.ContainsKey(character.playerID))
        {
            playerCharacters[character.playerID] = new List<BB_CharacterPull>();
        }
        playerCharacters[character.playerID].Add(character);
    }

    public bool CanControl(BB_CharacterPull character)
    {
        return character.playerID == currentPlayerTurn
            && character.canAct
            && character.isAlive
            && !usedCharactersThisCycle.Contains(character)
            && currentCharacter == null;
    }

    public void EndTurn(BB_CharacterPull character)
    {
        usedCharactersThisCycle.Add(character);
        currentCharacter = null;

        CheckWinCondition();
        turnCount++;

        // 周期終了後に使用キャラリセット
        if (usedCharactersThisCycle.Count >= 6)
        {
            usedCharactersThisCycle.Clear();
        }

        // プレイヤー交代
        currentPlayerTurn = (currentPlayerTurn == 1) ? 2 : 1;

        // ★ 死亡キャラを復活
        RespawnPendingCharacters();

        Debug.Log($"EndTurn called. Turn {turnCount}, Player {currentPlayerTurn}");
    }

    private void RespawnPendingCharacters()
    {
        foreach (var kvp in playerCharacters)
        {
            foreach (var character in kvp.Value)
            {
                if (character != null && character.pendingRespawn && !character.gameObject.activeSelf)
                {
                    character.Respawn();
                    character.pendingRespawn = false;
                }
            }
        }
    }

    public void ReportKill(int killerPlayerID)
    {
        int opponent = (killerPlayerID == 1) ? 2 : 1;
        killCount[killerPlayerID]++;

        if (killCount[killerPlayerID] >= maxKillsToWin)
        {
            Debug.Log($"Player {killerPlayerID} wins!");
            Invoke(nameof(LoadTitleScene), 3f); // 3秒後にTitleSceneへ
        }
    }

    private void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void CheckWinCondition()
    {
        // 勝利判定は ReportKill に任せる
    }

    public void SetCurrentCharacter(BB_CharacterPull character)
    {
        currentCharacter = character;
    }

    public BB_CharacterPull CurrentCharacter => currentCharacter;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"Turn: {turnCount + 1}", new GUIStyle() { fontSize = 20, normal = new GUIStyleState() { textColor = Color.white } });
        GUI.Label(new Rect(10, 40, 200, 30), $"Current Player: Player {currentPlayerTurn}", new GUIStyle() { fontSize = 20, normal = new GUIStyleState() { textColor = Color.white } });
    }
}
