using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class Node
    {
        public NodeState state;

        public    Node       parent;
        public List<Node> children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();
        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (var child in children)
            {
                Attach(child);
            }
        }

        public void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
            {
                return value;
            }
            
            var node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            var node = parent;
            while (node != null)
            {
                var cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }

            return false;
        }

        public bool hasChildren() => children.Count > 0;
    }
}

