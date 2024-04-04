using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private float _spawnTimer;
    
    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= 1)
        {
            Destroy(gameObject);
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
            
    }
}
