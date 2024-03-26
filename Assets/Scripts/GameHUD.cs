using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Slider xpBar;
    
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
}
