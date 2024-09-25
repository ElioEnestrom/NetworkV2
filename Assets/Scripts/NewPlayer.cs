using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class NewPlayer : NetworkBehaviour
{

    public InputAction moveAction, shootBullet;
    [SerializeField] private GameObject boolet;
    
    //Only used by the server so make sure they are set by the server is they need to change.
    [SerializeField] private int speed;
    private Vector2 moveInput;

    private void Start()
    {
        if (IsLocalPlayer)
        {
            moveAction.Enable();
            shootBullet.Enable();
        }
        else
        {
            moveAction.Disable();
            shootBullet.Disable();
        }
    }

    void Update()
    {
        if (IsServer)
        {
            Vector3 newPos = Time.deltaTime * speed * moveInput;
            transform.position += newPos;

        }
        if(IsLocalPlayer)
        {
            MoveServerRpc(moveAction.ReadValue<Vector2>());
            
            if (shootBullet.WasPressedThisFrame())
            {
                var direction = Input.mousePosition - transform.position;
                
                ShootBulletServerRpc();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            var networkObject = other.gameObject.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                DestroyPlayerServerRpc(networkObject.NetworkObjectId);
            }   
        }
    

    }
    
    [Rpc(SendTo.Server)]
    private void DestroyPlayerServerRpc(ulong networkObjectId)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null)
        {
            Debug.Log("What");
            Destroy(networkObject.gameObject);
        }
    }

    [Rpc(SendTo.Server)]
    private void MoveServerRpc(Vector2 newInput)
    {
        moveInput = newInput;
    }
    
    
    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        // Instantiate the bullet on the server
        GameObject bullet = Instantiate(boolet, transform.position + transform.forward, Quaternion.identity);

        // Spawn the bullet over the network so it appears on all clients
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}