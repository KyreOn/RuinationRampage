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
        var xPos                = -playerPosNormalized.x * 160 * 5;
        var yPos                = -playerPosNormalized.z * 90  * 5;
        var angle               = Vector2.SignedAngle(new Vector2(-xPos, yPos), Vector2.up);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = new Vector3(xPos + Screen.width /2, yPos + Screen.height /2);
    }
}
