using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows;

namespace BTree
{
    public abstract class Branch : TreeNode
    {
        protected List<Condition> conditionNodes;
        protected TreeResponse storedResponse;

        private bool CheckOwnConditions() => conditionNodes.All(condition => condition.Check());

        protected TreeResponse ResolveConditions(TreeResponse response)
        {
            if (response.Result == Result.Failure) return response;
            if (!CheckOwnConditions())
            {
                response.Result = Result.Failure;
                response.Conditions.Clear();
                storedResponse = response;
                return storedResponse;
            }
            response.Conditions.AddRange(conditionNodes);

            return response;
        }

        protected override TreeNode[] GetChildNodes()
        {
            var connectedChildren = new List<TreeNode>();
            conditionNodes = new List<Condition>();

            foreach (var port in Inputs)
            {
                if (port.Connection == null) { continue; }

                if (port.Connection.node is not TreeNode node) continue;
                if (node is Condition condition)
                {
                    condition.Host = this;
                    conditionNodes.Add(condition);
                }
                else
                    connectedChildren.Add(node);
            }
            return connectedChildren.ToArray();
        }
    }
}
