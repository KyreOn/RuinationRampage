using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ArenaPattern : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPatternsLevel0;
    [SerializeField] private List<GameObject> enemyPatternsLevel1;
    [SerializeField] private List<GameObject> enemyPatternsLevel2;
    [SerializeField] private List<GameObject> enemyPatternsLevel3;
    [SerializeField] private List<GameObject> enemyPatternsLevel4;
    [SerializeField] private List<GameObject> enemyPatternsLevel5;
    [SerializeField] private List<GameObject> enemyPatternsLevel6;
    [SerializeField] private List<GameObject> enemyPatternsLevel7;

    private GameObject _enemies;
    private int        _enemyCount;

    public void LoadEnemies(int wave)
    {
        //var level = wave / 5;
        var level = 0;
        switch (level)
        {
            case 0: 
                LoadEnemiesFromLevel(enemyPatternsLevel0);
                break;
            case 1: 
                LoadEnemiesFromLevel(enemyPatternsLevel1);
                break;
            case 2: 
                LoadEnemiesFromLevel(enemyPatternsLevel2);
                break;
            case 3: 
                LoadEnemiesFromLevel(enemyPatternsLevel3);
                break;
            case 4: 
                LoadEnemiesFromLevel(enemyPatternsLevel4);
                break;
            case 5: 
                LoadEnemiesFromLevel(enemyPatternsLevel5);
                break;
            case 6: 
                LoadEnemiesFromLevel(enemyPatternsLevel6);
                break;
            case 7: 
                LoadEnemiesFromLevel(enemyPatternsLevel7);
                break;
        }
        
    }

    private void LoadEnemiesFromLevel(IReadOnlyList<GameObject> patterns)
    {
        var pattern = Random.Range(0, patterns.Count);
        _enemies = Instantiate(patterns[pattern], transform);
        _enemyCount = _enemies.transform.childCount;
    }
    
    public bool OnEnemyDeath()
    {
        _enemyCount--;
        return _enemyCount == 0;
    }
}
