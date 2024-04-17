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
        xp += StatsManager.GetKills() + 2 * WaveManager.currentWave;
        result.text = $"+{StatsManager.GetKills() + 2 * WaveManager.currentWave} ОМ";
        PlayerPrefs.SetInt($"Xp{charId}", xp);
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
