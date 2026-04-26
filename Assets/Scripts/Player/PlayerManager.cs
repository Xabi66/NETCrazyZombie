using TMPro;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    public const int BULLET_DAMAGE = 10;

    public NetworkVariable<int> spawns;
    public NetworkVariable<FixedString128Bytes> username;

    [SerializeField] TMP_Text m_UsernameLabel;
    private GameObject playerSpawner;
    public TextMeshProUGUI txtSpawns;
    
    [SerializeField] private PlayerHealth health; //Referencia a la vida del jugador

    private void Awake()
    {
        username = new NetworkVariable<FixedString128Bytes>(Utilities.GetRandomUsername());
        playerSpawner = GameObject.Find("PlayerSpawner");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            health.OnDie += HandleDeath;
        }

        spawns.OnValueChanged += OnSpawnsChanged;
        username.OnValueChanged += OnClientUsernameChanged;
        ChangeNameRpc(Utilities.GetRandomUsername());
        gameObject.transform.position = playerSpawner.GetComponent<SpawnPointManager>().GetRandomSpawnPoint();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsServer)
        {
            health.OnDie -= HandleDeath;
        }

        username.OnValueChanged -= OnClientUsernameChanged;
    }

    private void OnClientUsernameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        m_UsernameLabel.text = newValue.ToString();
    }
    
    [Rpc(SendTo.Server)]
    public void ChangeNameRpc(FixedString128Bytes newValue)
    {
        if(!IsServer) return;
        username.Value = newValue;        
    }

    void OnSpawnsChanged(int previousValue, int newValue)
    {
        txtSpawns.text = newValue.ToString();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(IsServer){
            if (collision.gameObject.CompareTag("Bullet"))
            {
                health.TakeDamage(BULLET_DAMAGE);
            }
        }
    }

    private void HandleDeath(PlayerHealth h)
    {
        Respawn();
    }

    private void Respawn()
    {
        if (!IsServer) return;

        Rigidbody rb= GetComponent<Rigidbody>();

        rb.linearVelocity=Vector3.zero;
        rb.angularVelocity=Vector3.zero;

        gameObject.transform.position = playerSpawner.GetComponent<SpawnPointManager>().GetRandomSpawnPoint();
    
        health.RestoreFull(); //Al respawnear llama a health para curar la vida al maximo
        spawns.Value++;
    }
}