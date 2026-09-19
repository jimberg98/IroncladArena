using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHitPoints = 100f;
    public bool IsPlayer;
    public bool IsDead { get; private set; }
    public float Current { get; private set; }
    public float Normalized => MaxHitPoints <= 0f ? 0f : Current / MaxHitPoints;

    private void Awake() { Current = MaxHitPoints; }

    public void ApplyDamage(float amount, GameObject source)
    {
        if (IsDead) return;
        Current = Mathf.Max(0f, Current - amount);
        if (IsPlayer) CameraShake.Kick(0.22f, 0.16f);
        if (Current <= 0f) Die(source);
    }

    private void Die(GameObject source)
    {
        IsDead = true;
        Explosion.Burst(transform.position + Vector3.up, 4.5f);
        GameManager.Ensure().NotifyDestroyed(this, source);
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddExplosionForce(18f, transform.position + Random.insideUnitSphere, 4f, 1.5f, ForceMode.Impulse);
        }
        foreach (var col in GetComponentsInChildren<Collider>()) col.enabled = false;
        Destroy(gameObject, IsPlayer ? 2.5f : 3.5f);
    }
}
