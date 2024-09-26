using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float booletSpeed = 20f;

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
            //mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //mousePosition.z = 0;
            
            FireBulletServerRpc();
        }
    }
    
    [ServerRpc]
    private void FireBulletServerRpc()
    {
        Vector3 direction = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) - transform.position;
        
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        
        rb.velocity = new Vector3(direction.x, direction.y).normalized * booletSpeed;

        // Optionally, you could also sync the bullet velocity with clients
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
