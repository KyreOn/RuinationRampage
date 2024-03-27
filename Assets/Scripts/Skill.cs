using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] private Image      skillIcon;
    [SerializeField] private Image[]    levelIndicators;
    [SerializeField] private Slider     skillShade;
    [SerializeField] private TMP_Text   cdText;
    [SerializeField] private GameObject charge;
    [SerializeField] private TMP_Text   chargeCount;

    public void Init(Sprite disabledSprite, Sprite enabledSprite, int level)
    {
        skillIcon.sprite = level == 0 ? disabledSprite : enabledSprite;
        foreach (var indicator in levelIndicators)
        {
            indicator.enabled = false;
        }
        if (level == 0) return;
        for (var i = 0; i < level; i++)
        {
            levelIndicators[i].enabled = true;
        }
    }
    
    public void UpdateData(float cd, float maxCD, int charges, int maxCharges)
    {
        skillShade.value = 1 - cd / maxCD;
        var cooldown = Mathf.Round(maxCD - cd);
        cdText.text = cooldown == 0 ? "" : cooldown.ToString();
        charge.SetActive(maxCharges != 1);
        chargeCount.text = charges.ToString();
        if (cd == 0)
        {
            skillShade.value = 0;
            cdText.text = "";
        }
    }
}
