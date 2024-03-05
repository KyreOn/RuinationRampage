using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class PlayerTest : MonoBehaviour, ITreeContext
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Position
    {
        get
        {
            var p = transform.position;
            return p;
        }
    }
}
