using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    private float _trauma;
    private float _dampen = 4f;

    private void Awake() { _instance = this; }

    public static void Kick(float amount, float duration)
    {
        if (_instance == null) return;
        _instance._trauma = Mathf.Max(_instance._trauma, amount);
        _instance._dampen = 1f / Mathf.Max(0.05f, duration);
    }

    private void LateUpdate()
    {
        if (_trauma <= 0f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 8f);
            return;
        }
        _trauma = Mathf.Max(0f, _trauma - Time.deltaTime * _dampen);
        float s = _trauma * _trauma;
        float yaw = (Mathf.PerlinNoise(Time.time * 28f, 0.1f) - 0.5f) * 8f * s;
        float pitch = (Mathf.PerlinNoise(0.3f, Time.time * 28f) - 0.5f) * 8f * s;
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
