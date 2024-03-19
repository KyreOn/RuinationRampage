using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunImmuneEffect : Effect
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public StunImmuneEffect(float duration) : base(duration)
    {
        effectId = 11;
        effectType = EffectType.DISABLE_IMMUNE;
    }
}
