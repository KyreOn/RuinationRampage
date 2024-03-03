using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }
        
        public override NodeState Evaluate()
        {
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

        public override NodeState CheckState()
        {
            var successNodeFound = false;
            foreach (var node in children)
            {
                var nodeState = node.CheckState();
                if (nodeState == NodeState.FAILURE)
                {
                    continue;
                }

                if (nodeState == NodeState.SUCCESS)
                {
                    successNodeFound = true;
                }
            }

            if (successNodeFound)
            {
                state = NodeState.SUCCESS;
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}