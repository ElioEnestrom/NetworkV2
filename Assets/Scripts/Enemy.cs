using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            var networkObject = other.gameObject.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                DestroyEnemyServerRpc(networkObject.NetworkObjectId);
            }   
        }
    }
    
    [Rpc(SendTo.Server)]
    private void DestroyEnemyServerRpc(ulong networkObjectId)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null)
        {
            Destroy(networkObject.gameObject);
            Destroy(NetworkManager.Singleton.SpawnManager.SpawnedObjects[gameObject.GetComponent<NetworkObject>().NetworkObjectId].gameObject);
        }
    }
}
