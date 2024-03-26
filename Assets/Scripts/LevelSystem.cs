using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private float[] xpForLevel = new float[28];

    private int _curLevel ;
    private int _upgradeLevel;
    private int _curXP;
    
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
        if (_curLevel == xpForLevel.Length) return;
        if (!(_curXP >= xpForLevel[_curLevel])) return;
        if (_curLevel < 27) _curLevel++;

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
