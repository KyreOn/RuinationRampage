using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BTree;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Conditions
{
    public class CheckRange : Condition
    {
        [SerializeField] private float minDistance = 0;
        [SerializeField] private float maxDistance = float.MaxValue;
        protected override bool OnCheck()
        {
            if (Agent == null) { return false; }
            var enemy   = Agent as Enemy;
            var distance = Vector3.Distance(enemy.Player.Position, enemy.transform.position);
            return distance <= maxDistance && distance >= minDistance;
        }
    }
}

