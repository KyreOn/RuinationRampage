using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private float minDistance;
    
    private Transform _playerTransform;
    private Image     _image;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform.Find("CameraOffset").transform;
        _image = GetComponent<Image>();
    }
    
    void Update()
    {
        var playerPos = _playerTransform.position;
        _image.enabled = playerPos.magnitude >= minDistance;
        var playerPosNormalized = playerPos.normalized;
        var xPos      = -playerPosNormalized.x * 160 * 2  + Screen.width /2;
        var yPos      = -playerPosNormalized.z * 90 * 2 + Screen.height /2;
        transform.position = new Vector3(xPos, yPos);
    }
}
