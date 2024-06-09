using XNode;

namespace BTree
{
    public class Selector : Branch
    {
        private int _index;

        private bool HasNextChild => _index + 1 < children.Length;

        internal override void ResetNode()
        {
            _index = 0;
            storedResponse = null;
        }

        public override object GetValue(NodePort port)
        {
            if (storedResponse != null) { return storedResponse; }

            _index = 0;
            var response = GetChildResponseAtIndex(_index);

            if (response.Result == Result.Success)
            {
                storedResponse = response;
                return storedResponse;
            }

            var resolved = Resolve(response);
            return ResolveConditions(resolved);
        }

        private TreeResponse Resolve(TreeResponse response)
        {
            switch (response.Result)
            {
                case Result.Running:
                    return response;
                case Result.Failure when HasNextChild:
                    _index++;
                    response = GetChildResponseAtIndex(_index);
                    return Resolve(response);
                default:
                    return response;
            }
        }
    }
}