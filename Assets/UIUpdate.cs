using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    [SerializeField] private TMP_Text   speedText;
    [SerializeField] private GameObject player;

    private MovementSystem _playerMS;
    void Start()
    {
        _playerMS = player.GetComponent<MovementSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = _playerMS.GetSpeed().ToString();
    }
}
