using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class NewPlayer : NetworkBehaviour
{

    public InputAction moveAction, shootBullet;
    
    [SerializeField] private NetworkVariable<GameObject> bullet = new NetworkVariable<GameObject>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Server);
    
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

            if (shootBullet.WasPressedThisFrame())
            {
                Instantiate(bullet);
            }
        }
        if(IsLocalPlayer)
        {
            MoveServerRpc(moveAction.ReadValue<Vector2>());
        }
    }

    [Rpc(SendTo.Server)]
    private void MoveServerRpc(Vector2 newInput)
    {
        moveInput = newInput;
    }
}