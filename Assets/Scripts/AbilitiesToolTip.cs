using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesToolTip : MonoBehaviour
{
    private RectTransform _transform;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        _transform.position = new Vector3(mousePos.x + _transform.rect.width / 1.8f, mousePos.y + _transform.rect.height / 1.8f);
    }
}
