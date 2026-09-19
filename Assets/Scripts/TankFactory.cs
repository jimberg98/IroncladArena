using UnityEngine;

public static class TankFactory
{
    public static GameObject SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        var tank = BuildHull("PlayerTank", position, rotation, new Color(0.22f, 0.38f, 0.22f), true);
        tank.tag = "Player";
        var controller = tank.AddComponent<PlayerTankController>();
        var health = tank.AddComponent<Health>();
        health.MaxHitPoints = 140f;
        health.IsPlayer = true;
        var turret = CreateTurret(tank.transform, new Color(0.18f, 0.32f, 0.18f));
        controller.Turret = turret.transform;
        controller.Muzzle = turret.transform.Find("Muzzle");
        CreateCamera(turret.transform);
        return tank;
    }

    public static GameObject SpawnEnemy(Vector3 position, Quaternion rotation)
    {
        var tank = BuildHull("EnemyTank", position, rotation, new Color(0.45f, 0.16f, 0.12f), false);
        tank.tag = "Enemy";
        var health = tank.AddComponent<Health>();
        health.MaxHitPoints = 70f;
        var turret = CreateTurret(tank.transform, new Color(0.38f, 0.12f, 0.10f));
        var ai = tank.AddComponent<EnemyTankAI>();
        ai.Turret = turret.transform;
        ai.Muzzle = turret.transform.Find("Muzzle");
        GameManager.Ensure().RegisterEnemy();
        return tank;
    }

    private static GameObject BuildHull(string name, Vector3 position, Quaternion rotation, Color color, bool player)
    {
        var root = new GameObject(name);
        root.transform.SetPositionAndRotation(position, rotation);
        var body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "Hull";
        body.transform.SetParent(root.transform, false);
        body.transform.localScale = new Vector3(3.2f, 1.1f, 4.6f);
        body.transform.localPosition = new Vector3(0f, 0.15f, 0f);
        Object.Destroy(body.GetComponent<BoxCollider>());
        ArenaBuilder.ApplyColor(body, color);
        var skirt = GameObject.CreatePrimitive(PrimitiveType.Cube);
        skirt.name = "Tracks";
        skirt.transform.SetParent(root.transform, false);
        skirt.transform.localScale = new Vector3(3.6f, 0.55f, 4.8f);
        skirt.transform.localPosition = new Vector3(0f, -0.45f, 0f);
        Object.Destroy(skirt.GetComponent<BoxCollider>());
        ArenaBuilder.ApplyColor(skirt, new Color(0.12f, 0.12f, 0.12f));
        var rb = root.AddComponent<Rigidbody>();
        rb.mass = player ? 18f : 14f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        var col = root.AddComponent<BoxCollider>();
        col.size = new Vector3(3.6f, 1.6f, 4.8f);
        return root;
    }

    private static GameObject CreateTurret(Transform hull, Color color)
    {
        var turret = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        turret.name = "Turret";
        turret.transform.SetParent(hull, false);
        turret.transform.localPosition = new Vector3(0f, 0.95f, -0.15f);
        turret.transform.localScale = new Vector3(2.1f, 0.35f, 2.1f);
        Object.Destroy(turret.GetComponent<Collider>());
        ArenaBuilder.ApplyColor(turret, color);
        var barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        barrel.name = "Barrel";
        barrel.transform.SetParent(turret.transform, false);
        barrel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        barrel.transform.localPosition = new Vector3(0f, 0f, 1.6f);
        barrel.transform.localScale = new Vector3(0.18f, 1.5f, 0.18f);
        Object.Destroy(barrel.GetComponent<Collider>());
        ArenaBuilder.ApplyColor(barrel, new Color(0.15f, 0.15f, 0.15f));
        var muzzle = new GameObject("Muzzle");
        muzzle.transform.SetParent(turret.transform, false);
        muzzle.transform.localPosition = new Vector3(0f, 0f, 3.15f);
        return turret;
    }

    private static void CreateCamera(Transform turret)
    {
        var existing = Camera.main;
        GameObject camGo;
        if (existing != null) camGo = existing.gameObject;
        else
        {
            camGo = new GameObject("PlayerCamera");
            camGo.AddComponent<Camera>();
            camGo.AddComponent<AudioListener>();
            camGo.tag = "MainCamera";
        }
        camGo.transform.SetParent(turret, false);
        camGo.transform.localPosition = new Vector3(0f, 1.15f, 0.15f);
        camGo.transform.localRotation = Quaternion.identity;
        var cam = camGo.GetComponent<Camera>();
        cam.nearClipPlane = 0.12f;
        cam.fieldOfView = 70f;
        cam.backgroundColor = new Color(0.48f, 0.62f, 0.78f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        if (camGo.GetComponent<CameraShake>() == null)
            camGo.AddComponent<CameraShake>();
    }
}
