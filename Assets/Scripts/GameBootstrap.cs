using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoStart()
    {
        if (FindFirstObjectByType<GameBootstrap>() != null)
            return;
        var go = new GameObject("GameBootstrap");
        go.AddComponent<GameBootstrap>();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ArenaBuilder.Build();
        GameManager.Ensure();

        Vector3 spawn = new Vector3(0f, 1.2f, -42f);
        TankFactory.SpawnPlayer(spawn, Quaternion.identity);

        int enemies = 6;
        for (int i = 0; i < enemies; i++)
        {
            float angle = (360f / enemies) * i + 20f;
            float radius = Random.Range(28f, 48f);
            Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 1.2f, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
            if (Vector3.Distance(pos, spawn) < 16f) continue;
            TankFactory.SpawnEnemy(pos, Quaternion.LookRotation(Vector3.zero - pos));
        }

        HudUI.Ensure();
    }
}
