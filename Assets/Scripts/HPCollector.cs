using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCollector : MonoBehaviour
{
    ParticleSystem                particleSys;
    List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        
        var triggeredParticles = particleSys.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
        for (var i = 0; i < triggeredParticles; i++)
        {
            var p = _particles[i];
            p.remainingLifetime = 0;
            _particles[i] = p;

            particleSys.trigger.GetCollider(0).GetComponent<DamageSystem>().ApplyHeal(10);
        }
        
        particleSys.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
    }
}
