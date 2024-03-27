using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private TMP_Text currentHP;
    [SerializeField] private TMP_Text maximumHP;

    private Slider hpBar;

    private void Awake()
    {
        hpBar = GetComponent<Slider>();
    }

    public void HPUpdate(float curHP, float maxHP)
    {
        currentHP.text = curHP.ToString();
        maximumHP.text = maxHP.ToString();
        hpBar.value = curHP / maxHP;
    }
}
