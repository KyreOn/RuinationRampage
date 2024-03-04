using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

public class ZombieSimpleAttack : Leaf<ITreeContext>
{
    private Zombie       _zombie;
    private ZombieAttack _zombieAttack;
    private bool         _started;
    
    protected override void OnSetup()
    {
        _zombie = Agent as Zombie;
        _zombieAttack = _zombie.GetComponent<ZombieAttack>();
    }

    protected override void OnEnter()
    {
        if (_zombie.CheckIsIdle())
        {
            var direction = _zombie.Player.Position - _zombie.transform.position;
            direction.Scale(new Vector3(1, 0, 1));
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            _zombie.transform.rotation = rotation;
            _zombieAttack.StartAttack(_zombie.Player.gameObject);
            _started = true;
        }
        else
        {
            Response.Result = Result.Failure;
        }
    }

    protected override void OnExecute()
    {
        if (_zombie.Player == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        if (_started && !_zombieAttack.isAttacking)
        {
            Response.Result = Result.Success;
        }
    }

    protected override void OnExit()
    {
        _started = false;
    }

    protected override void OnReset()
    {
        
    }

    protected override void OnFail()
    {
        
    }
}
