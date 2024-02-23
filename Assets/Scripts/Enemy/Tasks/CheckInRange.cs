using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckInRange : Node
{
    private GameObject _target;
    private GameObject _self;
    private float      _maxDistance;
    private float      _minDistance;
    
    public CheckInRange(float maxDistance, float minDistance = 0)
    {
        _maxDistance = maxDistance;
        _minDistance = minDistance;
    }
    public override NodeState Evaluate()
    {
        _target = (GameObject)GetData("player");
        _self = (GameObject)GetData("self");

        var distance = (_target.transform.position - _self.transform.position).magnitude;
        if (distance >= _minDistance && distance < _maxDistance)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FAILURE;
        return state;
    }
}
