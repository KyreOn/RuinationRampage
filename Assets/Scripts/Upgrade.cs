using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.UIElements.Cursor;
using Image = UnityEngine.UI.Image;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private Image    skillIcon;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillLevel;
    [SerializeField] private TMP_Text skillDesc;
    
    public void Init(Spell spell)
    {
        skillIcon.sprite = spell.enabledSprite;
        skillName.text = spell.title;
        skillLevel.text = "Уровень " + spell.level + " \u02c3\u02c3 " + (spell.level + 1);
        skillDesc.text = spell.GetDescription();
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        skillName.text = "";
        skillLevel.text = "";
        skillDesc.text = "";
        gameObject.SetActive(false);
    }
}
