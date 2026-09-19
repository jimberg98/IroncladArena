using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Ensure()
    {
        var existing = FindFirstObjectByType<GameManager>();
        if (existing != null) return existing;
        return new GameObject("GameManager").AddComponent<GameManager>();
    }

    public int Score { get; private set; }
    public int EnemiesRemaining { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool PlayerWon { get; private set; }

    public void RegisterEnemy() { EnemiesRemaining++; }

    public void NotifyDestroyed(Health victim, GameObject source)
    {
        if (victim.IsPlayer)
        {
            IsGameOver = true;
            PlayerWon = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        EnemiesRemaining = Mathf.Max(0, EnemiesRemaining - 1);
        if (source != null && source.CompareTag("Player")) Score += 100;
        if (EnemiesRemaining <= 0)
        {
            IsGameOver = true;
            PlayerWon = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
