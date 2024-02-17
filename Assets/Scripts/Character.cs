using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Character : MonoBehaviour
{
    [SerializeField] private Texture2D  crosshair;
    
    private void Start()
    {
        Cursor.SetCursor(crosshair, new Vector2(crosshair.width/2, crosshair.height/2), CursorMode.Auto);
    }
}
