using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ControllerStunArea : MonoBehaviour
{
    [SerializeField] private float     lifeSpan;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float     timeBetweenTicks;

    private float _tickTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
            Destroy(gameObject);
        _tickTimer -= Time.deltaTime;
        if (_tickTimer >= 0) return;
        var players = Physics.OverlapSphere(transform.position, 2, playerLayer);
        foreach (var player in players)
        {
            player.gameObject.GetComponent<EffectSystem>().AddEffect(new SlowEffect(timeBetweenTicks, 2), false);
        }

        _tickTimer = timeBetweenTicks;
    }
}
