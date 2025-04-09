using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    private int currentPlayerTurn = 1; // 1: Player1, 2: Player2
    private int turnCount = 0;

    private Dictionary<int, List<CharacterPull>> playerCharacters = new Dictionary<int, List<CharacterPull>>();
    private Dictionary<int, int> killCount = new Dictionary<int, int>() { { 1, 0 }, { 2, 0 } };

    private HashSet<CharacterPull> usedCharactersThisCycle = new HashSet<CharacterPull>();
    private CharacterPull currentCharacter = null;

    public int maxKillsToWin = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void RegisterCharacter(CharacterPull character)
    {
        if (!playerCharacters.ContainsKey(character.playerID))
        {
            playerCharacters[character.playerID] = new List<CharacterPull>();
        }
        playerCharacters[character.playerID].Add(character);
    }

    public bool CanControl(CharacterPull character)
    {
        return character.playerID == currentPlayerTurn
            && character.canAct
            && character.isAlive
            && !usedCharactersThisCycle.Contains(character)
            && currentCharacter == null;
    }

    public void EndTurn(CharacterPull character)
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

        // デバッグ用のログ
        Debug.Log($"EndTurn called. Turn {turnCount}, Player {currentPlayerTurn}");
    }

    public void ReportKill(int killerPlayerID)
    {
        int opponent = (killerPlayerID == 1) ? 2 : 1;
        killCount[killerPlayerID]++;

        if (killCount[killerPlayerID] >= maxKillsToWin)
        {
            Debug.Log($"Player {killerPlayerID} wins!");
            Invoke(nameof(RestartScene), 2f); // 2秒後にリスタート
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CheckWinCondition()
    {
        // 勝利判定は ReportKill に任せる
    }

    public void SetCurrentCharacter(CharacterPull character)
    {
        currentCharacter = character;
    }

    public CharacterPull CurrentCharacter => currentCharacter;

    // OnGUI() メソッドでターンとプレイヤー情報を画面に表示
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"Turn: {turnCount + 1}", new GUIStyle() { fontSize = 20, normal = new GUIStyleState() { textColor = Color.white } });
        GUI.Label(new Rect(10, 40, 200, 30), $"Current Player: Player {currentPlayerTurn}", new GUIStyle() { fontSize = 20, normal = new GUIStyleState() { textColor = Color.white } });
    }
}
