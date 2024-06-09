using UnityEngine;
using XNode;

namespace BTree
{
    [NodeTint(0.15f, 0.25f, 0.25f)]
    public abstract class Condition : TreeNode
    {
        [SerializeField] protected bool invert;

        internal Branch Host { get; set; }

        internal bool Check()
        {
            if (!initialized) { return false; }

            var check = OnCheck();
            var result = invert ? !check : check;

            if (Agent.debugTree && !result) { Debug.Log($"{Agent}.{this} failed."); }

            return result;
        }

        protected abstract bool OnCheck();

        public override object GetValue(NodePort port) => new TreeResponse(Check() ? Result.Success : Result.Failure);
    }
}