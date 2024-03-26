using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillLevel;
    [SerializeField] private TMP_Text skillDesc;
    
    public void Init(Spell spell)
    {
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
