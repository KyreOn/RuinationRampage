using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryHealthEffect : Effect
{
    private float _hp;
    public override float ApplyEffect()
    {
        return _hp;
    }

    public void ApplyDamage(float damage)
    {
        _hp -= damage;
    }
    
    public TemporaryHealthEffect(float hp, float duration) : base(duration)
    {
        effectId = 15;
        effectType = EffectType.TEMPORARY_HEALTH;
        _hp = hp;
    }
}
