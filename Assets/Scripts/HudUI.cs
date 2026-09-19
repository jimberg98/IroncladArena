using UnityEngine;

public class HudUI : MonoBehaviour
{
    public static void Ensure()
    {
        if (FindFirstObjectByType<HudUI>() != null) return;
        new GameObject("HUD").AddComponent<HudUI>();
    }

    private Health _player;
    private GUIStyle _center;
    private GUIStyle _left;
    private Texture2D _pixel;

    private void Start()
    {
        _pixel = Texture2D.whiteTexture;
        _center = new GUIStyle { alignment = TextAnchor.MiddleCenter, fontSize = 28, fontStyle = FontStyle.Bold };
        _center.normal.textColor = Color.white;
        _left = new GUIStyle { alignment = TextAnchor.UpperLeft, fontSize = 18 };
        _left.normal.textColor = new Color(0.92f, 0.93f, 0.86f);
    }

    private void OnGUI()
    {
        if (_player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) _player = p.GetComponent<Health>();
        }
        DrawCrosshair();
        DrawBars();
        DrawHelp();
        DrawEndCard();
    }

    private void DrawCrosshair()
    {
        float cx = Screen.width * 0.5f;
        float cy = Screen.height * 0.5f;
        Color prev = GUI.color;
        GUI.color = new Color(1f, 0.92f, 0.55f, 0.85f);
        GUI.DrawTexture(new Rect(cx - 10f, cy - 1.5f, 20f, 3f), _pixel);
        GUI.DrawTexture(new Rect(cx - 1.5f, cy - 10f, 3f, 20f), _pixel);
        GUI.color = prev;
    }

    private void DrawBars()
    {
        var gm = GameManager.Ensure();
        float hp = _player != null ? _player.Normalized : 0f;
        GUI.Label(new Rect(24f, 18f, 480f, 28f), $"ARMOR  {Mathf.RoundToInt(hp * 100f)}%", _left);
        DrawMeter(24f, 46f, 280f, 16f, hp, new Color(0.25f, 0.75f, 0.32f));
        GUI.Label(new Rect(24f, 72f, 480f, 24f), $"SCORE  {gm.Score}     HOSTILES  {gm.EnemiesRemaining}", _left);
    }

    private void DrawMeter(float x, float y, float w, float h, float t, Color fill)
    {
        Color prev = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.55f);
        GUI.DrawTexture(new Rect(x, y, w, h), _pixel);
        GUI.color = fill;
        GUI.DrawTexture(new Rect(x + 2f, y + 2f, (w - 4f) * Mathf.Clamp01(t), h - 4f), _pixel);
        GUI.color = prev;
    }

    private void DrawHelp()
    {
        var style = new GUIStyle(_left) { fontSize = 14, alignment = TextAnchor.LowerLeft };
        style.normal.textColor = new Color(1f, 1f, 1f, 0.7f);
        GUI.Label(new Rect(24f, Screen.height - 52f, 800f, 36f), "WASD drive   Mouse aim turret   LMB / Space fire   Esc unlock cursor", style);
    }

    private void DrawEndCard()
    {
        var gm = GameManager.Ensure();
        if (!gm.IsGameOver) return;
        Color prev = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.55f);
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _pixel);
        GUI.color = prev;
        string title = gm.PlayerWon ? "ARENA CLEARED" : "HULL BREACHED";
        string sub = gm.PlayerWon ? $"All hostiles destroyed.  Score {gm.Score}" : "Your tank was knocked out.  Press R to refit.";
        GUI.Label(new Rect(0f, Screen.height * 0.38f, Screen.width, 40f), title, _center);
        var subStyle = new GUIStyle(_center) { fontSize = 18, fontStyle = FontStyle.Normal };
        subStyle.normal.textColor = new Color(0.9f, 0.9f, 0.85f);
        GUI.Label(new Rect(0f, Screen.height * 0.38f + 48f, Screen.width, 32f), sub, subStyle);
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R)
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
