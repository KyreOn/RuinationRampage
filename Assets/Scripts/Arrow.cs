using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward);
    }
}
