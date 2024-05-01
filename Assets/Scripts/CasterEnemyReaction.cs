using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemyReaction : Reaction
{
    [SerializeField] private GameObject illusion;
    [SerializeField] private GameObject spawnEffect;
    
    private Animator   _animator;

    protected override void OnAwake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void OnReact(GameObject stimuli)
    {
        var rotationDir = stimuli.transform.position - transform.position;
        rotationDir.y = 0;
        var rotation = Quaternion.LookRotation(rotationDir);
        transform.rotation = rotation;
        _animator.SetTrigger("React");
        _animator.speed = 4;
    }

    public void CastReact()
    {
        _animator.speed = 1;

        var choice    = Random.value;

        var firstPos  = transform.position;
        var secondPos = transform.position + transform.right * 2;
        var thirdPos  = transform.position - transform.right * 2;
        switch (choice)
        {
            case < 0.33f:
                transform.position = secondPos;
                Instantiate(illusion, firstPos, transform.rotation);
                Instantiate(illusion, thirdPos, transform.rotation);
                break;
            case > 0.67f:
                transform.position = thirdPos;
                Instantiate(illusion, firstPos,  transform.rotation);
                Instantiate(illusion, secondPos, transform.rotation);
                break;
            default:
                Instantiate(illusion, secondPos, transform.rotation);
                Instantiate(illusion, thirdPos,  transform.rotation);
                break;
        }
        Destroy(Instantiate(spawnEffect, firstPos,  Quaternion.identity), 0.3f);
        Destroy(Instantiate(spawnEffect, secondPos, Quaternion.identity), 0.3f);
        Destroy(Instantiate(spawnEffect, thirdPos,  Quaternion.identity), 0.3f);
    }
}
