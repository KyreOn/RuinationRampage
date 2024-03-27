using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Slider  xpBar;
    [SerializeField] private Skill[] skills;
    [SerializeField] private HPBar   hpBar;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateXP(float value)
    {
        xpBar.value = value;
    }

    public void InitSkill(int id, Sprite disabledSprite, Sprite enabledSprite, int level)
    {
        skills[id].Init(disabledSprite, enabledSprite, level);
    }

    public void UpdateSkill(int id, float cd, float maxCD, int charges, int maxCharges)
    {
        skills[id].UpdateData(cd, maxCD, charges, maxCharges);
    }

    public void UpdateHP(float curHP, float maxHP)
    {
        hpBar.HPUpdate(curHP, maxHP);
    }
}
