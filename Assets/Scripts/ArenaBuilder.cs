using UnityEngine;

public static class ArenaBuilder
{
    public static void Build()
    {
        CreateLighting();
        CreateGround();
        CreatePerimeter();
        CreateCover();
        CreateSkyTint();
    }

    private static void CreateLighting()
    {
        var sunGo = new GameObject("Sun");
        var light = sunGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(1f, 0.93f, 0.78f);
        light.intensity = 1.15f;
        light.shadows = LightShadows.Soft;
        sunGo.transform.rotation = Quaternion.Euler(48f, 140f, 0f);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.55f, 0.62f, 0.72f);
        RenderSettings.ambientEquatorColor = new Color(0.42f, 0.38f, 0.32f);
        RenderSettings.ambientGroundColor = new Color(0.18f, 0.16f, 0.12f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.55f, 0.58f, 0.52f);
        RenderSettings.fogDensity = 0.0085f;
    }

    private static void CreateSkyTint()
    {
        if (Camera.main != null)
            Camera.main.backgroundColor = new Color(0.48f, 0.62f, 0.78f);
    }

    private static void CreateGround()
    {
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(16f, 1f, 16f);
        ApplyColor(ground, new Color(0.33f, 0.38f, 0.22f));
        ground.isStatic = true;
    }

    private static void CreatePerimeter()
    {
        const float half = 72f;
        const float height = 6f;
        const float thick = 3f;
        PlaceWall("WallN", new Vector3(0f, height * 0.5f, half), new Vector3(half * 2f + thick, height, thick));
        PlaceWall("WallS", new Vector3(0f, height * 0.5f, -half), new Vector3(half * 2f + thick, height, thick));
        PlaceWall("WallE", new Vector3(half, height * 0.5f, 0f), new Vector3(thick, height, half * 2f));
        PlaceWall("WallW", new Vector3(-half, height * 0.5f, 0f), new Vector3(thick, height, half * 2f));
    }

    private static void PlaceWall(string name, Vector3 pos, Vector3 scale)
    {
        var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.tag = "Cover";
        wall.transform.position = pos;
        wall.transform.localScale = scale;
        ApplyColor(wall, new Color(0.28f, 0.26f, 0.22f));
        wall.isStatic = true;
    }

    private static void CreateCover()
    {
        Vector3[] blocks =
        {
            new Vector3(18f, 0f, 12f), new Vector3(-22f, 0f, 8f), new Vector3(8f, 0f, -24f),
            new Vector3(-14f, 0f, -18f), new Vector3(32f, 0f, -8f), new Vector3(-36f, 0f, 22f),
            new Vector3(0f, 0f, 28f), new Vector3(24f, 0f, 36f), new Vector3(-28f, 0f, -34f)
        };
        foreach (var p in blocks) PlaceBunker(p);
        for (int i = 0; i < 18; i++)
        {
            Vector3 p = new Vector3(Random.Range(-55f, 55f), 0.75f, Random.Range(-55f, 55f));
            if (p.magnitude < 10f) continue;
            var crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crate.name = "Crate";
            crate.tag = "Cover";
            crate.transform.position = p;
            crate.transform.localScale = new Vector3(Random.Range(1.4f, 2.6f), Random.Range(1.2f, 2.2f), Random.Range(1.4f, 2.6f));
            crate.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            ApplyColor(crate, new Color(0.45f, 0.32f, 0.18f));
        }
    }

    private static void PlaceBunker(Vector3 origin)
    {
        var bunker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bunker.name = "Bunker";
        bunker.tag = "Cover";
        bunker.transform.position = origin + Vector3.up * 2.2f;
        bunker.transform.localScale = new Vector3(8f, 4.4f, 6f);
        bunker.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        ApplyColor(bunker, new Color(0.38f, 0.36f, 0.30f));
    }

    public static void ApplyColor(GameObject go, Color color)
    {
        var renderer = go.GetComponent<Renderer>();
        if (renderer == null) return;
        var mat = new Material(Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Sprites/Default"));
        if (mat.HasProperty("_Color")) mat.color = color;
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        renderer.material = mat;
    }
}
