using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightManager : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    
    private Enemy      _enemy;
    private GameObject _player;

    public bool sight;
    
    void Start()
    {
        _enemy = GetComponent<Enemy>();
        _player = _enemy.Player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var rayDirection = _player.transform.position - transform.position;
        Debug.DrawLine(_player.transform.position, transform.position);
        //rayDirection.y = 0;
        if (Physics.Raycast(transform.position, rayDirection, out var hit, float.PositiveInfinity, layerMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                _enemy.PlayerInSight = true;
                sight = true;
                return;
            }
        }

        _enemy.PlayerInSight = false;
        sight = false;
    }
}
