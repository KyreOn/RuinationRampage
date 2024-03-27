using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsManager : MonoBehaviour
{ 
    public static StatsManager Instance { get; private set; }
    public static float        timer;
    public static int[]        kills;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        kills = new int[9];
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public static void AddKill(int id)
    {
        kills[id]++;
    }

    public static int GetKills()
    {
        return kills.Sum();
    }
}
