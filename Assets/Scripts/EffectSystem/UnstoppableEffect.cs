using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstoppableEffect : Effect
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnstoppableEffect(float duration) : base(duration)
    {
        effectId = 6;
        effectType = EffectType.UNSTOPPABLE;
    }
}
