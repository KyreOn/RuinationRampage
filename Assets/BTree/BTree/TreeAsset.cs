using UnityEngine;
using XNode;

namespace BTree
{
    [CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "BTree/New Behaviour Tree")]
    
    public class TreeAsset : NodeGraph
    {
        private TreeAgent agent;
        private bool isInterrupted = false;
        private bool forcedReset = false;
        private Interrupt interruption = null;

        public Root Root { get; private set; }

        internal bool TryInterrupt(string interruptId, ITreeContext context, out Interrupt interruption)
        {
            isInterrupted = Root.GetInterrupt(interruptId, out interruption);
            this.interruption = interruption;
            return isInterrupted;
        }
        
        internal bool Evaluate(out TreeResponse response)
        {
            if (forcedReset)
            {
                ResetNodes();
            }

            if (isInterrupted)
            {
                if (agent.debugTree) { Debug.Log(agent + " has been interrupted with " + interruption.Id); }

                isInterrupted = false;
                forcedReset = interruption.ForceReset;
                response = interruption.Response;
                return true;
            }

            if (agent.debugTree) { Debug.Log(agent + " evaluating tree..."); }
            
            response = Root.Response;

            if (response.Result != Result.Running || response.Origin == null)
            {
                if (agent.debugTree) { Debug.Log($"{agent} received {response.Result} from {response.Origin}"); }
                return false;
            }

            if (agent.debugTree) { DebugResult(response); }

            return true;
        }

        internal void Initialize(TreeAgent agent)
        {
            this.agent = agent;
            Root = nodes.Find(x => x is Root) as Root;
            Root.RecursiveSetup(agent);
        }

        internal void ResetNodes()
        {
            if (agent.debugTree) { Debug.Log(agent + " is resetting its behavior tree."); }

            forcedReset = false;
            interruption = null;
            isInterrupted = false;

            foreach (var n in nodes)
            {
                var tn = n as TreeNode;
                tn.ResetNode();
            }
        }

        private void DebugResult(TreeResponse result)
        {
            if (result?.Origin != null)
            {
                Debug.Log($"{agent} Tree result: {result.Result} from {result.Origin}");
            }
        }
    }
}

