using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using BehaviorTree;
using Node = BehaviorTree.Node;
using Tree = BehaviorTree.Tree;

public class UIUpdate : MonoBehaviour
{
    [SerializeField] private TMP_Text   speedText;
    [SerializeField] private GameObject enemy;

    private Tree   tree;
    private string text; 
        
    void Start()
    {
        tree = enemy.GetComponent<Tree>();
        
    }

    // Update is called once per frame
    void Update()
    {
        text = "";
        GetNode(tree.root, 0);
        speedText.text = text;
    }

    private void GetNode(Node node, int level)
    {
        text += string.Concat(Enumerable.Repeat("\t", level)) + node.GetType() + " " + node.state + "\n";
        foreach (var child in node.children)
        {
            GetNode(child, level+1);
        }
    }
    
}
