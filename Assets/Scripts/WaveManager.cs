using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject arena;
    [SerializeField] private GameObject enemies;
    void Start()
    {
        Instantiate(arena, Vector3.zero, Quaternion.identity);
        Instantiate(enemies, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
