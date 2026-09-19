using UnityEngine;

public class Explosion : MonoBehaviour
{
    public static void Burst(Vector3 position, float size)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "Explosion";
        go.transform.position = position;
        go.transform.localScale = Vector3.one * 0.2f;
        Object.Destroy(go.GetComponent<Collider>());
        ArenaBuilder.ApplyColor(go, new Color(1f, 0.55f, 0.12f));
        var fx = go.AddComponent<Explosion>();
        fx._size = size;
        Object.Destroy(go, 0.35f);
    }

    private float _size;
    private float _t;

    private void Update()
    {
        _t += Time.deltaTime / 0.28f;
        transform.localScale = Vector3.one * Mathf.Lerp(0.2f, _size, Mathf.Clamp01(_t));
        var r = GetComponent<Renderer>();
        if (r != null && r.material.HasProperty("_Color"))
            r.material.color = Color.Lerp(new Color(1f, 0.7f, 0.15f), new Color(0.15f, 0.12f, 0.1f, 0f), _t);
    }
}
