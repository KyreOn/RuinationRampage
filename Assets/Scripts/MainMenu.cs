using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] rightParts;

    public void ShowRightPart(int id)
    {
        foreach (var part in rightParts)
        {
            part.SetActive(false);
        }
        rightParts[id].SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
