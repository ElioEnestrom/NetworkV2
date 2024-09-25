using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("SpawnedEnemy");
        SpawnEnemyServerRpc();
    }
    
    [ServerRpc]
    private void SpawnEnemyServerRpc()
    {
        // Instantiate the bullet on the server
        GameObject bullet = Instantiate(enemy, new Vector3(Random.Range(-5, 5), 3), Quaternion.identity);

        // Spawn the bullet over the network so it appears on all clients
        bullet.GetComponent<NetworkObject>().Spawn();
        
        StartCoroutine(SpawnEnemy());
    }
}
