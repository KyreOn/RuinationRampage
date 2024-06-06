using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Spell : MonoBehaviour
{
    [SerializeField] protected float  baseCooldown;
    [SerializeField] protected int    maxCharges = 1;
    [SerializeField] public    string title;
    [SerializeField] public    int    maxLevel;
    [SerializeField] public    int    id;
    [SerializeField] public    Sprite disabledSprite;
    [SerializeField] public    Sprite enabledSprite;
    
    protected LevelSystem  levelSystem;
    protected float        cooldownTimer;
    protected int          curCharges;
    protected EffectSystem effectSystem;
    protected GameHUD      gameHUD;
    protected bool         isPreparing;
    protected bool         isBlocked;

    public int  level;
    public bool isUlt;

    public void Prepare()
    {
        if (curCharges == 0 || level == 0 || isBlocked || effectSystem.CheckIfStunned()) return;
        isPreparing = true;
        OnPrepare();
    }

    protected virtual void OnPrepare()
    {

    }

    public void Cast()
    {
        if (curCharges == 0 || !isPreparing) return;
        isPreparing = false;
        curCharges--;
        OnCast();
    }

    protected virtual void OnCast()
    {

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        gameHUD.UpdateSkill(id, cooldownTimer, baseCooldown, curCharges, maxCharges);
        OnUpdate();
        if (curCharges == maxCharges) return;
        cooldownTimer = Mathf.Clamp(cooldownTimer + Time.deltaTime, 0, baseCooldown);
        if (cooldownTimer < baseCooldown) return;
        curCharges++;
        cooldownTimer = 0;
    }
    
    protected virtual void OnUpdate()
    {

    }

    public void Upgrade()
    {
        level++;
        OnUpgrade();
        gameHUD.UpdateSkill(id, disabledSprite, enabledSprite, level);
        levelSystem.OnUpgrade();
    }

    protected virtual void OnUpgrade()
    {
        
    }
    
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        levelSystem = GetComponent<LevelSystem>();
        gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
        gameHUD.UpdateSkill(id, disabledSprite, enabledSprite, level);
        curCharges = maxCharges;
        effectSystem = GetComponent<EffectSystem>();
    }

    

    public virtual string GetDescription()
    {
        return "";
    }
}
