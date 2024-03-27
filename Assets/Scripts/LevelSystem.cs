using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private float[] xpForLevel = new float[28];
    
    private GameHUD _gameHUD;
    private int _curLevel ;
    private int _upgradeLevel;
    private int _curXP;

    private void Awake()
    {
        _gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CollectXP(int amount)
    {
        _curXP += amount;
        var needXP  = xpForLevel[_curLevel] - _curXP;
        var levelXP = xpForLevel[_curLevel] - (_curLevel == 0 ? 0 : xpForLevel[_curLevel - 1]);
        _gameHUD.UpdateXP(1 - (needXP / levelXP));
        if (_curLevel == xpForLevel.Length) return;
        if (!(_curXP >= xpForLevel[_curLevel])) return;
        if (_curLevel < 26) _curLevel++;
        needXP  = xpForLevel[_curLevel] - _curXP;
        levelXP = xpForLevel[_curLevel] - (_curLevel == 0 ? 0 : xpForLevel[_curLevel - 1]);
        _gameHUD.UpdateXP(1 - (needXP / levelXP));
    }

    public bool CheckForUpgrades()
    {
        return _curLevel > _upgradeLevel;
    }

    public void OnUpgrade()
    {
        _upgradeLevel++;
    }
}
