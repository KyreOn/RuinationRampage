using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RotateToTargetTask : Node
{
    private GameObject _self;
    private GameObject _target;

    public RotateToTargetTask(GameObject self, GameObject target)
    {
        _self = self;
        _target = target;
    }
    
    public override NodeState Evaluate()
    {
        var direction = _target.transform.position - _self.transform.position;
        direction.Scale(new Vector3(1, 0, 1));
        var toRotation = Quaternion.LookRotation(direction, Vector3.up);
        _self.transform.rotation = Quaternion.RotateTowards(_self.transform.rotation, toRotation, 1000);
        state = NodeState.SUCCESS;
        return state;
    }
}
