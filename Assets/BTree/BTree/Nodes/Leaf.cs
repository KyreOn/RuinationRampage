using System;
using UnityEngine;
using XNode;

namespace BTree
{
    [NodeTint(0.15f, 0.175f, 0.15f)]
    public abstract class Leaf<T> : TreeNode, ILeaf where T : class, ITreeContext
    {
        [SerializeField, Tooltip("Negative value means indefinite.")]
        protected float maxDuration = -1f;

        [SerializeField, Tooltip("Try to get a context with this name when entering this node.")]
        private string inContext;

        [SerializeField, Tooltip("Try to add a context with this name when exiting this node.")]
        private string outContext;

        [SerializeField, Tooltip("If the Out Context already exists, overwrite it.")]
        private bool overwriteOut = true;

        [SerializeField, Tooltip("Fail if In Context is set to null")]
        private bool failNullCtx = false;

        private float elapsed = 0;
        private T context;

        public TreeResponse Response { get; set; }

        public event Func<bool> OnExceptionFail;

        protected T Context
        {
            get
            {
                if (context == null) { throw new ContextNullException("Context possibly destroyed while running " + this); }
                return context;
            }
            set
            {
                if (failNullCtx && value == null) { throw new ContextNullException(this + " received a null Context"); }
                context = value;
            }
        }

        protected sealed override void Setup(TreeAgent agent)
        {
            Response = new TreeResponse(this);
            base.Setup(agent);
            OnSetup();
        }
        
        protected abstract void OnSetup();

        public void Enter()
        {
            try
            {
                if (!string.IsNullOrEmpty(inContext) && Agent.TryGetContext(inContext, out ITreeContext context))
                    Context = context as T;
                OnEnter();
            }
            catch (ContextNullException)
            {
                ExceptionFailure();
            }
        }
        
		protected abstract void OnEnter();

        public void Execute()
        {
            try
            {
                OnExecute();
            }
            catch (ContextNullException)
            {
                ExceptionFailure();
                return;
            }

            if (maxDuration < 0) { return; }

            elapsed += Time.deltaTime;

            if (elapsed > maxDuration) Response.Result = Result.Failure;
        }
        
        protected abstract void OnExecute();

        public void Exit()
        {
            try
            {
                OnExit();

                if (!string.IsNullOrEmpty(outContext) && !Agent.TryAddContext(outContext, Context, overwriteOut))
                    Debug.LogWarning($"{Agent}.{this} outContext {outContext} already exists.");
            }
            catch (ContextNullException)
            {
                ExceptionFailure();
            }
        }
        
        protected abstract void OnExit();

        internal sealed override void ResetNode()
        {
            Response.Result = Result.Running;
            context = null;
            elapsed = 0;
            OnReset();
        }
        
        protected abstract void OnReset();
        
        public void Fail()
        {
            Response.Result = Result.Failure;
            OnFail();
        }

        private void ExceptionFailure()
        {
            Response.Result = Result.Failure;
            bool handled = OnExceptionFail.Invoke();

            if (handled)
            {
            }
        }
        
        protected abstract void OnFail();
        
        public sealed override object GetValue(NodePort port) => Response;
    }
}