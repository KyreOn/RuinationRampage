using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoctorHook : MonoBehaviour
{
    [SerializeField] private DoctorHookProjectile projectile;

    public void Init(GameObject source)
    {
        projectile.Init(source);
    }
}
