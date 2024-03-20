using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WaveManager.StartUnload();
    }
}
