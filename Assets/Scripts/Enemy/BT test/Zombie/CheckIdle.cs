using System.Collections;
using System.Collections.Generic;
using BTree;
using Unity.VisualScripting;
using UnityEngine;

namespace Conditions
{
    public class CheckIdle : Condition
    {
        protected override bool OnCheck()
        {
            if (Agent == null) { return false; }
            var zombie       = Agent as Zombie;
            var go           = zombie.gameObject;
            var jumpAttack   = go.GetComponent<JumpAttack>();
            var simpleAttack = go.GetComponent<ZombieAttack>();
            return !(jumpAttack.isJump || simpleAttack.isAttacking);
        }
    }
}

