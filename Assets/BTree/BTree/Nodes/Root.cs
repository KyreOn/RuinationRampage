﻿using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    public class Root : TreeNode
    {
        [SerializeField, Input(dynamicPortList: true, connectionType: ConnectionType.Override)]
        public TreeResponse interruptions;

        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        public TreeResponse input;

        public TreeResponse Response => GetChildResponse();

        private Dictionary<string, Interrupt> interruptNodes = null;

        internal bool GetInterrupt(string interruptId, out Interrupt interrupt)
        {
            if (interruptNodes.TryGetValue(interruptId, out interrupt))
            {
                return true;
            }

            if (Agent.debugTree)
            {
                Debug.LogWarning($"{Agent}.{this} could not find interrupt {interruptId}");
            }

            return false;
        }

        protected override TreeNode[] GetChildNodes()
        {
            var connectedChildren = new List<TreeNode>();
            interruptNodes = new Dictionary<string, Interrupt>();

            foreach (var port in Inputs)
            {
                if (port.Connection?.node is TreeNode node)
                {
                    if (node is Interrupt interrupt)    // add interrupts to a separate list.
                    {
                        interruptNodes.Add(interrupt.Id, interrupt);
                    }
                    else
                    {
                        connectedChildren.Add(node);
                    }
                }
            }

            return connectedChildren.ToArray();
        }

        protected override void Init()
        {
            name = $"{nameof(Root)} ({graph.name})";

            if (graph.nodes.FindAll(x => x is Root).Count > 1)
            {
                Debug.LogError($"{Agent} found multiple root nodes in tree {graph.name}! Only one is allowed!");
            }
        }
    }
}