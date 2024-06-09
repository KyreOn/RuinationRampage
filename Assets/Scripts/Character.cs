using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Character : MonoBehaviour
{
    [SerializeField] private Texture2D crosshair;
    [SerializeField] private bool      centered;
    
    private void Start()
    {
        Cursor.SetCursor(crosshair, centered ? new Vector2(crosshair.width/2, crosshair.height/2) : Vector2.zero, CursorMode.Auto);
    }
}
