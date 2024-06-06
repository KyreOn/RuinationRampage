using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text result;

    private void OnEnable()
    {
        timeText.text = FormatTime(StatsManager.timer);
        waveText.text = WaveManager.currentWave.ToString();
        killsText.text = StatsManager.GetKills().ToString();
        var charId = PlayerPrefs.GetInt("LastSelected");
        var xp     = PlayerPrefs.HasKey($"Xp{charId}") ? PlayerPrefs.GetInt($"Xp{charId}") : 0;
        var xpAdd = 10 * StatsManager.GetKills() + 2 * WaveManager.currentWave +
                    (int)(0.5f * WaveManager.currentWave / StatsManager.timer);
        xp += xpAdd;
        PlayerPrefs.SetInt($"Xp{charId}", xp);
        var gold = PlayerPrefs.GetInt("Gold", 0);
        gold += xpAdd / 10;
        PlayerPrefs.SetInt("Gold", gold);
        result.text = $"+{xpAdd/10}G +{xpAdd} ОМ";
    }

    private string FormatTime(float time)
    {
        var intTime = (int)time;
        var minutes = intTime / 60;
        var seconds = intTime % 60;
        return $"{minutes}м {seconds}с";
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
