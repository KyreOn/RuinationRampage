using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace BTree
{
    [NodeWidth(275)]
    public abstract class TreeNode : Node
    {
        [SerializeField, Output(connectionType = ConnectionType.Override)]
        protected TreeResponse output;

        protected TreeNode[] children;
        protected bool initialized = false;

        protected TreeAgent Agent { get; private set; }
        protected virtual void Setup(TreeAgent agent)
        {
            Agent = agent;
            children = GetChildNodes();
            initialized = true;
        }

        protected virtual TreeNode[] GetChildNodes()    // Find all nodes connected to childPort ports.
        {
            List<TreeNode> connectedChildren = new List<TreeNode>();
            var inputs = Inputs.ToArray();

            foreach (var input in inputs)
            {
                if (input.Connection == null) { continue; }

                var node = input.Connection.node as TreeNode;

                if (node != null)
                {
                    connectedChildren.Add(node);
                }
            }

            return connectedChildren.ToArray();
        }
        
        protected TreeResponse GetChildResponse()
        {
            return GetChildResponseAtIndex(0);
        }
        
        protected TreeResponse GetChildResponseAtIndex(int i)
        {
            if (children == null || children.Length == 0) { return null; }  // Check to prevent editor errors

            // Fetch the port on the child that leads to this node and return the value it gets.
            var child = children[i];
            var childOutput = child.GetOutputPort(nameof(child.output));
            var result = child.GetValue(childOutput) as TreeResponse;
            return result;
        }

        internal void RecursiveResetChildren()
        {
            if (children == null) { return; }

            foreach (var treeNode in children)
            {
                treeNode.ResetNode();
                treeNode.RecursiveResetChildren();
            }
        }

        internal void RecursiveFail()
        {
            if (this is ILeaf leaf)
            {
                leaf.Fail();
            }
            else
            {
                foreach (var child in children)
                {
                    child.RecursiveFail();
                }
            }
        }
        
        internal void RecursiveSetup(TreeAgent agent)
        {
            Setup(agent);

            foreach (var port in Inputs)
            {
                if (port.Connection == null) { continue; }

                if (port.Connection.node is TreeNode node)
                {
                    node.RecursiveSetup(agent);
                }
            }
        }
        
        internal virtual void ResetNode() { }
    }
}