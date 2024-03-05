using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;

namespace Conditions
{
    public class CheckInSight : Condition
    {
        protected override bool OnCheck()
        {
            if (Agent == null) { return false; }

            var enemy = Agent as Enemy;
            return enemy.PlayerInSight;
        }
    }
}

