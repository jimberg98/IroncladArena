using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTankController : MonoBehaviour
{
    public Transform Turret;
    public Transform Muzzle;
    public float MoveSpeed = 13f;
    public float ReverseSpeed = 7f;
    public float TurnSpeed = 70f;
    public float TurretYawSpeed = 140f;
    public float TurretPitchSpeed = 80f;
    public float MinPitch = -8f;
    public float MaxPitch = 18f;
    public float FireInterval = 0.85f;
    public float ShellSpeed = 62f;
    public float ShellDamage = 34f;

    private Rigidbody _body;
    private float _pitch;
    private float _nextFire;
    private Health _health;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (_health != null && _health.IsDead) return;
        if (GameManager.Ensure().IsGameOver) return;
        HandleLook();
        HandleFire();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = locked;
        }
    }

    private void FixedUpdate()
    {
        if (_health != null && _health.IsDead) return;
        if (GameManager.Ensure().IsGameOver) return;
        float throttle = Input.GetAxisRaw("Vertical");
        float steer = Input.GetAxisRaw("Horizontal");
        float speed = throttle >= 0f ? MoveSpeed : ReverseSpeed;
        Vector3 move = transform.forward * throttle * speed;
        _body.linearVelocity = new Vector3(move.x, _body.linearVelocity.y, move.z);
        if (Mathf.Abs(steer) > 0.05f)
            _body.MoveRotation(_body.rotation * Quaternion.Euler(0f, steer * TurnSpeed * Time.fixedDeltaTime, 0f));
    }

    private void HandleLook()
    {
        if (Turret == null) return;
        float mx = Input.GetAxis("Mouse X") * TurretYawSpeed * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * TurretPitchSpeed * Time.deltaTime;
        Turret.Rotate(0f, mx, 0f, Space.World);
        _pitch = Mathf.Clamp(_pitch - my, MinPitch, MaxPitch);
        Vector3 e = Turret.localEulerAngles;
        Turret.localRotation = Quaternion.Euler(_pitch, e.y, 0f);
    }

    private void HandleFire()
    {
        if (!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space)) return;
        if (Time.time < _nextFire || Muzzle == null) return;
        _nextFire = Time.time + FireInterval;
        Projectile.Spawn(Muzzle.position, Muzzle.forward, ShellSpeed, ShellDamage, gameObject);
        CameraShake.Kick(0.18f, 0.12f);
    }
}
