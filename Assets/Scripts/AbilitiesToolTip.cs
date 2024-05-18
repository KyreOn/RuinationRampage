using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilitiesToolTip : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descText;
    
    private RectTransform _transform;
    private bool          _isLeft;
    
    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        _transform.position = new Vector3(mousePos.x + (_isLeft ? -1 : 1) * _transform.rect.width / 1.8f, mousePos.y + _transform.rect.height / 1.8f);
    }

    public void Init(string title, string description, bool isLeft)
    {
        titleText.text = title;
        descText.text = description;
        _isLeft = isLeft;
    }
}
