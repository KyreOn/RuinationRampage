using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

namespace Conditions
{
    public class CheckRange : Condition
    {
        [SerializeField] private float minDistance = 0;
        [SerializeField] private float maxDistance = float.MaxValue;
        protected override bool OnCheck()
        {
            if (Agent == null) { return false; }
            var zombie   = Agent as Zombie;
            var distance = Vector3.Distance(zombie.Player.Position, zombie.transform.position);
            
            return distance <= maxDistance && distance >= minDistance;
        }
    }
}

