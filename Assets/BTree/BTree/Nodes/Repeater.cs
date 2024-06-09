using UnityEngine;
using XNode;

namespace BTree
{
    public class Repeater : Branch
    {
        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        protected TreeResponse input;

        [SerializeField, Tooltip("0 < repeats indefinitely")]
        private int repeat = 0;

        private int _counter;

        internal override void ResetNode()
        {
            _counter = 0;
        }

        public override object GetValue(NodePort port)
        {
            var response = GetChildResponse();

            if (response.Result == Result.Running || (repeat >= 0 && _counter >= repeat))
                return ResolveConditions(response);
            _counter++;
            response.Result = Result.Running;
            RecursiveResetChildren();

            return ResolveConditions(response);
        }
    }
}