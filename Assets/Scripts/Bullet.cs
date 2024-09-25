using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float booletSpeed = 20f;
    private int i = 0;

    private Camera mainCamera;
    private Vector3 mousePosition;
    private Rigidbody rb;
    
    
    public override void OnNetworkSpawn()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
        base.OnNetworkSpawn();
        
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        if (IsOwner)
        {
            if (mainCamera != null) mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseDirection = mousePosition - transform.position;
            //Vector3 rotation = transform.position - mousePosition;
            
            
            
            
            //rb.velocity = new Vector3(mouseDirection.x, mouseDirection.y).normalized * booletSpeed;
            FireBulletServerRpc(mouseDirection);
        }
        
    }
    
    
    [ServerRpc]
    private void FireBulletServerRpc(Vector3 direction)
    {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }
            rb.velocity = new Vector3(direction.x, direction.y).normalized * booletSpeed;

            FireBulletClientRpc(rb.velocity);
    }

    
    
    [ClientRpc]
    private void FireBulletClientRpc(Vector3 velocity)
    {
        if (!IsServer)
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }
            rb.velocity = velocity;
        }
    }

}   
