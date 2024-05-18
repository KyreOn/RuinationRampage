using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject basePart;
    [SerializeField] private GameObject settingsPart;
    [SerializeField] private GameObject exitPrompt;
    
    private static GameObject _pauseMenu;
    private static GameObject _basePart;
    private static GameObject _settingsPart;
    private static GameObject _exitPrompt;
    private static bool       _isPaused;
    private static bool       _isSettings;
    private static bool       _isExitPrompt;
    
    public static PauseManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _pauseMenu = pauseMenu;
        _basePart = basePart;
        _settingsPart = settingsPart;
        _exitPrompt = exitPrompt;
    }

    public static void OnPause()
    {
        if (_isSettings)
        {
            OpenSettings();
        }
        else if (_isExitPrompt)
        {
            ExitPrompt();
        }
        else
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
            _pauseMenu.SetActive(_isPaused);
        }
    }

    public static void OpenSettings()
    {
        _isSettings = !_isSettings;
        _basePart.SetActive(!_isSettings);
        _settingsPart.SetActive(_isSettings);
    }

    public static void ExitPrompt()
    {
        _isExitPrompt = !_isExitPrompt;
        _exitPrompt.SetActive(_isExitPrompt);
    }

    public static void QuitMenu()
    {
        SceneManager.LoadScene(0);
    }
}
