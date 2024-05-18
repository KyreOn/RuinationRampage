using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Slider     xpBar;
    [SerializeField] private Skill[]    skills;
    [SerializeField] private HPBar      hpBar;
    [SerializeField] private HPBar      tempHpBar;
    [SerializeField] private GameObject resultScreen;
    
    public void UpdateXP(float value)
    {
        xpBar.value = value;
    }

    public void UpdateSkill(int id, Sprite disabledSprite, Sprite enabledSprite, int level)
    {
        skills[id].Init(disabledSprite, enabledSprite, level);
    }

    public void UpdateSkill(int id, float cd, float maxCD, int charges, int maxCharges)
    {
        skills[id].UpdateData(cd, maxCD, charges, maxCharges);
    }

    public void UpdateHP(float curHP, float tempHP, float maxHP)
    {
        hpBar.HPUpdate(curHP, maxHP);
        tempHpBar.HPUpdate(tempHP, maxHP);
    }

    public void OnDeath()
    {
        resultScreen.SetActive(true);
    }

    public void OnComplete()
    {
        resultScreen.SetActive(true);
    }
}
