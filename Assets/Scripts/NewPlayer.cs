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
    

    [Rpc(SendTo.Server)]
    private void MoveServerRpc(Vector2 newInput)
    {
        moveInput = newInput;
    }
    
    
    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(boolet, transform.position + transform.forward, Quaternion.identity);

        bullet.GetComponent<NetworkObject>().Spawn();
    }
}