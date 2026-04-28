using UnityEngine;
using Unity.Netcode;

public class PlayerFireBullet : NetworkBehaviour
{
    [SerializeField] private GameObject serverProjectile;
    [SerializeField] private GameObject clientProjectile;
    [SerializeField] private Transform firePoint;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(clientProjectile, firePoint.position, firePoint.rotation);

            FireRpc(firePoint.position, firePoint.rotation);

        }
    }

    [Rpc(SendTo.Server)]
    void FireRpc(Vector3 pos, Quaternion rot)
    {
        Instantiate(serverProjectile, pos, rot);
        FireClientsRPC(firePoint.position, firePoint.rotation);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void FireClientsRPC (Vector3 pos, Quaternion rot)
    {
        if (IsOwner) return;

        Instantiate(clientProjectile, pos, rot);
    }
}
