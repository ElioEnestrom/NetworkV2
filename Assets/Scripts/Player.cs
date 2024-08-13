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

    
    private void OnEnable()
    {
        inputMovement.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner)
        {
            _movement2D = inputMovement.ReadValue<Vector2 >();
        
            Vector2 newPosition = new Vector3(_onlineMovement.Value.x + _movement2D.x, _onlineMovement.Value.y + _movement2D.y, 0);
            
            MoveServerRpc(newPosition);
            print(newPosition);
        }
        transform.position = (Vector3)_onlineMovement.Value * (speed * Time.deltaTime);
    }

    [ServerRpc]
    private void MoveServerRpc(Vector2 newPosition)
    {
        _onlineMovement.Value = newPosition;
    }
}
