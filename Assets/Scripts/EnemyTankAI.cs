using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyTankAI : MonoBehaviour
{
    public Transform Turret;
    public Transform Muzzle;
    public float MoveSpeed = 7.5f;
    public float TurnSpeed = 55f;
    public float SightRange = 55f;
    public float FireRange = 38f;
    public float FireInterval = 1.6f;
    public float ShellSpeed = 48f;
    public float ShellDamage = 18f;

    private Rigidbody _body;
    private Health _health;
    private Transform _player;
    private float _nextFire;
    private Vector3 _wanderTarget;
    private float _repathAt;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();
        PickWander();
    }

    private void Update()
    {
        if (_health != null && _health.IsDead) return;
        if (_player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) _player = p.transform;
        }
    }

    private void FixedUpdate()
    {
        if (_health != null && _health.IsDead) return;
        if (GameManager.Ensure().IsGameOver) return;
        Vector3 dest;
        bool engaging = false;
        if (_player != null)
        {
            float dist = Vector3.Distance(transform.position, _player.position);
            if (dist < SightRange)
            {
                dest = _player.position;
                engaging = true;
                AimAt(_player.position + Vector3.up * 1.1f);
                if (dist < FireRange) TryFire();
            }
            else dest = _wanderTarget;
        }
        else dest = _wanderTarget;

        if (!engaging && (Time.time > _repathAt || Vector3.Distance(transform.position, _wanderTarget) < 4f))
            PickWander();

        Vector3 to = dest - transform.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.01f) return;
        Quaternion look = Quaternion.LookRotation(to.normalized, Vector3.up);
        _body.MoveRotation(Quaternion.RotateTowards(_body.rotation, look, TurnSpeed * Time.fixedDeltaTime));
        float approach = engaging && Vector3.Distance(transform.position, dest) < 16f ? 0.35f : 1f;
        Vector3 vel = transform.forward * MoveSpeed * approach;
        _body.linearVelocity = new Vector3(vel.x, _body.linearVelocity.y, vel.z);
    }

    private void AimAt(Vector3 worldPoint)
    {
        if (Turret == null) return;
        Vector3 dir = worldPoint - Turret.position;
        if (dir.sqrMagnitude < 0.01f) return;
        Turret.rotation = Quaternion.RotateTowards(Turret.rotation, Quaternion.LookRotation(dir.normalized, Vector3.up), 120f * Time.deltaTime);
    }

    private void TryFire()
    {
        if (Time.time < _nextFire || Muzzle == null || _player == null) return;
        if (Vector3.Angle(Muzzle.forward, _player.position - Muzzle.position) > 12f) return;
        _nextFire = Time.time + FireInterval + Random.Range(0f, 0.4f);
        Projectile.Spawn(Muzzle.position, Muzzle.forward, ShellSpeed, ShellDamage, gameObject);
    }

    private void PickWander()
    {
        _wanderTarget = new Vector3(Random.Range(-50f, 50f), transform.position.y, Random.Range(-50f, 50f));
        _repathAt = Time.time + Random.Range(3f, 7f);
    }
}
