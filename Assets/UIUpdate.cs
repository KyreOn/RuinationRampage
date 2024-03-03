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
    [SerializeField] private TMP_Text   cdText;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;

    private Tree         tree;
    private string       _btText;
    private string       _cdText;
    private CombatSystem _combatSystem;
        
    void Start()
    {
        tree = enemy.GetComponent<Tree>();
        _combatSystem = player.GetComponent<CombatSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        _btText = "";
        GetNode(tree.root, 0);
        speedText.text = _btText;
        //UpdateCD();
        //cdText.text = _cdText;
    }

    private void GetNode(Node node, int level)
    {
        _btText += string.Concat(Enumerable.Repeat("\t", level)) + node.GetType() + " " + node.state + "\n";
        foreach (var child in node.children)
        {
            GetNode(child, level+1);
        }
    }

    private void UpdateCD()
    {
        //_cdText = _combatSystem.GetSpellsCD();
    }
}
