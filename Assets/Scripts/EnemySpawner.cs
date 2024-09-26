using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyGuy;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnEnemyCall;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemyCall()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        GameObject enemy = Instantiate(enemyGuy, new Vector3(Random.Range(-5, 5), 3), Quaternion.identity);
        
        enemy.GetComponent<NetworkObject>().Spawn();
        
        StartCoroutine(SpawnEnemy());
    }
    
    //[ServerRpc]
    //private void SpawnEnemyServerRpc()
    //{
    //    Debug.Log("SpawnedEnemy");
    //    
    //    GameObject bullet = Instantiate(enemy, new Vector3(Random.Range(-5, 5), 3), Quaternion.identity);
    //    
    //    bullet.GetComponent<NetworkObject>().Spawn();
    //    
    //    StartCoroutine(SpawnEnemy());
    //}
}
