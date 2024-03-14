using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class PlayerTest : MonoBehaviour, ITreeContext
{
    [SerializeField] private Transform model;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    public Vector3 Forward => model.forward;

    public Vector3 Position
    {
        get
        {
            var p = transform.position;
            return p;
        }
    }
}
