using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    
    public void UpdateHP(float curHp, float maxHp)
    {
        bar.value = curHp / maxHp;
    }
}
