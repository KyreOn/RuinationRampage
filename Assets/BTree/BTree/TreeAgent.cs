using System;
using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    public class TreeAgent : MonoBehaviour, ITreeContext
    {
        [SerializeField]
        public TreeLogger treeLogger;

        [SerializeField, Tooltip("Reference to a TreeAsset ScriptableObject. Will get copied to a property on Awake.")]
        private TreeAsset treeAsset;

        protected Dictionary<string, ITreeContext> context;
        protected TreeResponse current;

        public TreeAsset Tree { get; protected set; }
        public bool debugTree = false;

        public Vector3 Position
        {
            get
            {
                var p = transform.position;
                return new Vector3(p.x, 0, p.z);
            }
        }

        protected virtual void Awake()
        {
            if (treeAsset == null)
            {
                Debug.LogError($"TreeAgent on GameObject {name} has no TreeAsset assigned!");
                return;
            }

            context = new Dictionary<string, ITreeContext>();
            Tree = (TreeAsset)treeAsset.Copy();
            Tree.Initialize(this);
        }

        protected virtual void Start()
        {
            if (!TryEvaluate())
            {
                Debug.LogError(name + " failed to find a runnable Leaf in its Tree.");
            }

        }

        protected virtual void Update()
        {
            OnUpdate();
            if (current.CheckConditions())
            {
                current.Origin.Execute();
            }

            if (current.Result == Result.Running) { return; }

            current.Origin.Exit();
            TryEvaluate();
        }

        protected virtual void OnUpdate()
        {
            
        }
        
        protected bool TryEvaluate()
        {
            if (Tree.Evaluate(out TreeResponse next))
            {
                if (current != null)
                {
                    current.Origin.OnExceptionFail -= TryEvaluate;
                }

                next.Origin.OnExceptionFail += TryEvaluate;
                next.Origin.Enter();
                current = next;
                return true;
            }

            Restart();
            return TryEvaluate();
        }
        
        protected void Restart()
        {
            context.Clear();
            Tree.ResetNodes();
        }
        
        public void TriggerInterrupt(string interruptId, ITreeContext context)
        {
            if (!Tree.TryInterrupt(interruptId, context, out Interrupt interruption)) { return; }

            if (debugTree) { Debug.Log($"{name} was interrupted with Id {interruptId}"); }

            if (!string.IsNullOrEmpty(interruption.OutContext))
            {
                if (context == null)
                {
                    if (debugTree)
                    {
                        Debug.LogWarning($"{name}.{interruption} outContext {interruption.OutContext} not set.");
                    }
                }
                else if (TryAddContext(interruption.OutContext, context, interruption.OverwriteOut))
                {
                    if (debugTree)
                    {
                        Debug.LogWarning($"{name}.{interruption} outContext {interruption.OutContext} already exists.");
                    }
                }
            }

            if (current.Origin != null)
            {
                current.Origin.Fail();
                Tree.Evaluate(out current);
                current.Origin.Enter();
            }
            else
            {
                current.Result = Result.Failure;
            }
        }
        
        public bool TryAddContext(string key, ITreeContext context, bool overwrite)
        {
            if (!overwrite && this.context.ContainsKey(key)) { return false; }

            this.context[key] = context;
            return true;
        }
        
        public bool TryGetContext(string key, out ITreeContext context)
        {
            if (this.context.TryGetValue(key, out context)) { return true; }

            return false;
        }
        
        public bool RemoveContext(ITreeContext context)
        {
            List<string> keysToRemove = new List<string>();
            bool removed = false;

            foreach (var pair in this.context)
            {
                if (pair.Value == context)
                {
                    keysToRemove.Add(pair.Key);
                    removed = true;
                }
            }

            foreach (var key in keysToRemove)
            {
                this.context.Remove(key);
            }

            return removed;
        }
        
        public bool RemoveContext(string key)
        {
            return this.context.Remove(key);
        }
    }
}