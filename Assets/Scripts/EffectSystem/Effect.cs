using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable CommentTypo


public enum EffectType
{
    SPEED,
    STUN,
    ROOT,
    SLOW_IMMUNE,
    DISABLE_IMMUNE,
    DOT,
    DISPLACEMENT,
    PULLING,
    INVINCIBILITY,
    INCOME_DAMAGE,
    OUTCOME_DAMAGE,
    PARRY,
    TEMPORARY_HEALTH,
    ASSASSIN_DODGE,
}
public class Effect
{
    private readonly float _duration;      //Длительность эффекта
    private          float _durationTimer; //Таймер длительности
    
    public int        effectId;//Идентификатор эффекта
    public EffectType effectType;//Тип эффекта
    //Конструктор класса
    protected Effect(float duration)
    {
        _duration = duration;
    }
    
    public void UpdateEffect()
    {
        if (_duration == -1) return;//значение -1 используется для бесконечных эффектов
        _durationTimer += Time.deltaTime;
    }

    public virtual float ApplyEffect()
    {
        return 0;
    }

    public bool CheckForEnd()
    {
        if (_duration         == -1) return false;//сразу возвращает результат ложь, если эффект бесконеный
        return _durationTimer >= _duration;
    }
}
