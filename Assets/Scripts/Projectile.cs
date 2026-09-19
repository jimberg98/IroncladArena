using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage = 25f;
    public GameObject Owner;
    public float Life = 4f;

    public static void Spawn(Vector3 position, Vector3 direction, float speed, float damage, GameObject owner)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "Shell";
        go.tag = "Projectile";
        go.transform.position = position;
        go.transform.localScale = Vector3.one * 0.28f;
        Object.Destroy(go.GetComponent<SphereCollider>());
        var col = go.AddComponent<SphereCollider>();
        col.radius = 0.5f;
        col.isTrigger = true;
        var rb = go.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearVelocity = direction.normalized * speed;
        var proj = go.AddComponent<Projectile>();
        proj.Damage = damage;
        proj.Owner = owner;
        ArenaBuilder.ApplyColor(go, new Color(1f, 0.82f, 0.25f));
        Object.Destroy(go, proj.Life);
        var trail = go.AddComponent<TrailRenderer>();
        trail.time = 0.25f;
        trail.startWidth = 0.12f;
        trail.endWidth = 0.01f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = new Color(1f, 0.7f, 0.2f, 0.9f);
        trail.endColor = new Color(1f, 0.4f, 0.1f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Owner != null && (other.transform == Owner.transform || other.transform.IsChildOf(Owner.transform))) return;
        if (other.CompareTag("Projectile")) return;
        var health = other.GetComponentInParent<Health>();
        if (health != null) health.ApplyDamage(Damage, Owner);
        Explosion.Burst(transform.position, health != null ? 2.2f : 1.3f);
        Destroy(gameObject);
    }
}
