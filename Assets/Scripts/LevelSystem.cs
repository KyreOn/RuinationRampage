using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private float[] xpForLevel = new float[28];
    
    private GameHUD      _gameHUD;
    private int          _upgradeLevel;
    private int          _curXP;
    private DamageSystem _damageSystem;
    
    public int curLevel;

    private void Awake()
    {
        _gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
        _damageSystem = GetComponent<DamageSystem>();
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
        var needXP  = xpForLevel[curLevel] - _curXP;
        var levelXP = xpForLevel[curLevel] - (curLevel == 0 ? 0 : xpForLevel[curLevel - 1]);
        _gameHUD.UpdateXP(1 - (needXP / levelXP));
        if (curLevel == xpForLevel.Length) return;
        if (!(_curXP >= xpForLevel[curLevel])) return;
        if (curLevel < 26) curLevel++;
        needXP  = xpForLevel[curLevel] - _curXP;
        levelXP = xpForLevel[curLevel] - (curLevel == 0 ? 0 : xpForLevel[curLevel - 1]);
        _gameHUD.UpdateXP(1 - (needXP / levelXP));
    }

    public bool CheckForUpgrades()
    {
        return curLevel > _upgradeLevel;
    }

    public void OnUpgrade()
    {
        _upgradeLevel++;
        _damageSystem.OnUpgrade(_upgradeLevel);
    }
}
