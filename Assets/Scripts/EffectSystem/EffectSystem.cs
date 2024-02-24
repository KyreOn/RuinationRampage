using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    private List<Effect> _effects = new();

    // Update is called once per frame
    void Update()
    {
        foreach (var effect in _effects)
        {
            effect.UpdateEffect();
            if (effect.CheckForEnd())
            {
                _effects.Remove(effect);
            }
                
        }
    }

    public void AddEffect(Effect effect, bool stackable = true)
    {
        if (!stackable)
        {
            if (CheckForEffectById(effect.effectId))
                _effects.Remove(GetEffectById(effect.effectId));
        }
        _effects.Add(effect);
    }

    public float CalculateSpeedModifiers()
    {
        var resultModifier = 1f;
        foreach (var effect in _effects)
        {
            if (effect.effectType != EffectType.SPEED) continue;
            resultModifier *= effect.ApplyEffect();
        }

        return resultModifier;
    }

    private Effect GetEffectById(int id)
    {
        foreach (var effect in _effects)
        {
            if (effect.effectId == id)
                return effect;
        }

        return null;
    }
    
    public bool CheckForEffectById(int id)
    {
        foreach (var effect in _effects)
        {
            if (effect.effectId == id)
                return true;
        }

        return false;
    }
}
