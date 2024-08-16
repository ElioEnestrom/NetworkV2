using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : NetworkBehaviour
{
    public InputAction inputMovement;
    private Vector2 _movement2D;
    private Vector2 _movement;
    [SerializeField] private int speed;

    private NetworkVariable<Vector2> _onlineMovement = new NetworkVariable<Vector2>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Server);

  
    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            inputMovement.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            // if (Input.GetKeyDown(KeyCode.K))
            // {
            //     MoveServerRpc(Vector3.left);
            // }
            _movement2D = inputMovement.ReadValue<Vector2 >();
        
            Vector2 newPosition = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            
            MoveServerRpc(newPosition);
        }
    }

    [Rpc(SendTo.Server)]
    private void MoveServerRpc(Vector2 newPosition)
    {
        transform.position += (Vector3)newPosition * Time.deltaTime;
    }
}
