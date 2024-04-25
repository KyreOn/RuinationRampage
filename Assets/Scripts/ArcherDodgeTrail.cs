using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ArcherDodgeTrail : MonoBehaviour
{
    [SerializeField] private GameObject   trailTarget;
    [SerializeField] private VisualEffect effect;
    
    private float _lifeTime;
    private float _lifeTimer;
    
    public void Init(GameObject player, float lifeTime)
    {
        trailTarget.transform.SetParent(player.transform);
        trailTarget.transform.localPosition = Vector3.zero;
        _lifeTime = lifeTime;
        //Destroy(trailTarget, 2);
    }

    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer > _lifeTime)
            effect.Stop();
    }
}
