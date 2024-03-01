using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTEffect : Effect
{
    private float _tickTime;
    private float _damage;
    private float _tickTimer;

    public override float ApplyEffect()
    {
        _tickTimer += Time.deltaTime;
        if (!(_tickTimer >= _tickTime)) return 0;
        _tickTimer = 0;
        return _damage;

    }

    public DOTEffect(float duration, float tickTime, float damage) : base(duration)
    {
        effectId = 3;
        effectType = EffectType.DOT;
        _tickTime = tickTime;
        _damage = damage;
    }
}
