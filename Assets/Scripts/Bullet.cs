using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float booletSpeed = 20f;

    private Camera mainCamera;
    private Vector3 mousePosition;
    
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = mousePosition - transform.position;
        //Vector3 rotation = transform.position - mousePosition;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        
        
        rb.velocity = new Vector3(mouseDirection.x, mouseDirection.y).normalized * booletSpeed;
    }
    
    
    //[Rpc(SendTo.Server)]
    //private void MoveBulletRpc(Vector2 newInput)
    //{
    //    
    //}

}   
