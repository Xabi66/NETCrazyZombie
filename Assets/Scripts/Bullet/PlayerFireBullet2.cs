using Unity.Netcode;
using UnityEngine;

public class PlayerFireBullet2 : NetworkBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject serverBulletPrefab;
    [SerializeField] private GameObject clientBulletPrefab;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.3f;

    private float timer;

    void Update()
    {
        if (!IsOwner) return;

        if (timer > 0)
            timer -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && timer <= 0)
        {
            Vector3 pos = transform.position;
            Vector3 dir = transform.up;

            // 🔵 bala local (instantánea)
            SpawnClientBullet(pos, dir);

            // 🔴 servidor (autoridad)
            FireServerRpc(pos, dir);

            timer = fireRate;
        }
    }

    private void SpawnClientBullet(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(clientBulletPrefab, position, Quaternion.identity);
        bullet.transform.up = direction;

        if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }

    [Rpc(SendTo.Server)]
    private void FireServerRpc(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(serverBulletPrefab, position, Quaternion.identity);
        bullet.transform.up = direction;

        bullet.GetComponent<NetworkObject>().Spawn(true);

        if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}