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
    [SerializeField]private int speed;

    private NetworkVariable<Vector2> _onlineMovement = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);
    
    private void OnEnable()
    {
        inputMovement.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            _movement2D = inputMovement.ReadValue<Vector2 >();
            
            _onlineMovement.Value = new Vector3(_onlineMovement.Value.x + _movement2D.x * Time.deltaTime, _onlineMovement.Value.y + _movement2D.y * Time.deltaTime, 0);
            
            transform.position = (Vector3)_onlineMovement.Value * speed;
        }
    }

    private void HelloRPC(Vector2 data)
    {
        
    }
}
