using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BTree
{
    public class Sequence : Branch
    {
        [SerializeField]
        private bool haltOnFailure = true;

        [SerializeField, Input(dynamicPortList: true, connectionType: ConnectionType.Override)]
        protected TreeResponse input;

        private int index;
        private bool HasNextChild => index + 1 < children.Length;

        internal override void ResetNode()
        {
            index = 0;
            storedResponse = null;
        }

        public override object GetValue(NodePort port)
        {
            if (storedResponse != null) { return storedResponse; }

            index = 0;
            var response = GetChildResponseAtIndex(index);
            var resolved = Resolve(response);
            return ResolveConditions(resolved);
        }

        private TreeResponse Resolve(TreeResponse response)
        {
            if (response.Result == Result.Running)
            {
                return response;
            }

            if (response.Result != Result.Success && (haltOnFailure || response.Result != Result.Failure)) return response;
            if (HasNextChild)
            {
                index++;
                response = GetChildResponseAtIndex(index);
                return Resolve(response);
            }
            response.Result = Result.Success;

            return response;
        }
    }
}