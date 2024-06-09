using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown   resDropdown;
    [SerializeField] private TMP_Dropdown   screenDropdown;
    [SerializeField] private TMP_Dropdown   qualityDropdown;
    [SerializeField] private Slider         frameCapSlider;
    [SerializeField] private TMP_InputField frameCapInput;
    [SerializeField] private Slider         musicSlider;
    [SerializeField] private TMP_InputField musicInput;
    [SerializeField] private Slider         sfxSlider;
    [SerializeField] private TMP_InputField sfxInput;
    [SerializeField] private Toggle         cameraShakeToggle;
    [SerializeField] private Toggle         cameraCenterToggle;
    
    private Dictionary<string, int> _defaultSettings = new Dictionary<string, int>();
    
    private void Awake()
    {
        var nativeRes = Screen.resolutions.Last();
        _defaultSettings.Add("ResolutionWidth", nativeRes.width);
        _defaultSettings.Add("ResolutionHeight", nativeRes.height);
        _defaultSettings.Add("ScreenMode", 0);
        _defaultSettings.Add("Quality", 2);
        _defaultSettings.Add("FrameCap", 0);
        _defaultSettings.Add("MusicVolume", 100);
        _defaultSettings.Add("SFXVolume", 100);
    }
    void Start()
    {
        Init();
        Confirm();
        if (SceneManager.GetActiveScene().buildIndex == 0)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        foreach (var res in Screen.resolutions)
        {
            if (res.refreshRateRatio.value != Screen.currentResolution.refreshRateRatio.value) continue;
            var option = new TMP_Dropdown.OptionData($"{res.width}x{res.height}");
            resDropdown.options.Add(option);
            if (res.width     == PlayerPrefs.GetInt("ScreenWidth",  _defaultSettings["ResolutionWidth"]) 
                && res.height == PlayerPrefs.GetInt("ScreenHeight", _defaultSettings["ResolutionHeight"]))
                resDropdown.value = resDropdown.options.IndexOf(option);
        }

        screenDropdown.value = PlayerPrefs.GetInt("ScreenMode", _defaultSettings["ScreenMode"]);
        qualityDropdown.value = PlayerPrefs.GetInt("Quality",   _defaultSettings["Quality"]);
        frameCapSlider.value = PlayerPrefs.GetInt("FrameCap",   _defaultSettings["FrameCap"]);
        UpdateInput(frameCapInput);

        musicSlider.value = PlayerPrefs.GetInt("MusicVolume", _defaultSettings["MusicVolume"]);
        sfxSlider.value = PlayerPrefs.GetInt("SFXVolume",     _defaultSettings["SFXVolume"]);
        UpdateInput(musicInput);
        UpdateInput(sfxInput);
        
        cameraCenterToggle.isOn =
            !PlayerPrefs.HasKey("CameraShake") || PlayerPrefs.GetInt("CameraShake") == 1;
        cameraCenterToggle.isOn =
            !PlayerPrefs.HasKey("CameraCentered") || PlayerPrefs.GetInt("CameraCentered") == 1;
    }

    public void Confirm()
    {
        var res        = resDropdown.options[resDropdown.value].text.Split("x");
        var screenMode = FullScreenMode.ExclusiveFullScreen;
        switch (screenDropdown.value)
        {
            case 0:
                break;
            case 1:
                screenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                screenMode = FullScreenMode.Windowed;
                break;
        }
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(int.Parse(res[0]), int.Parse(res[1]), screenMode);
        Application.targetFrameRate = (int)frameCapSlider.value;
        
        PlayerPrefs.SetInt("ScreenWidth",    int.Parse(res[0]));
        PlayerPrefs.SetInt("ScreenHeight",   int.Parse(res[1]));
        PlayerPrefs.SetInt("ScreenMode",     screenDropdown.value);
        PlayerPrefs.SetInt("Quality",        qualityDropdown.value);
        PlayerPrefs.SetInt("FrameCap",       (int)frameCapSlider.value);
        PlayerPrefs.SetInt("MusicVolume",    (int)musicSlider.value);
        PlayerPrefs.SetInt("SFXVolume",      (int)sfxSlider.value);
        PlayerPrefs.SetInt("CameraShake",    cameraShakeToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("CameraCentered", cameraCenterToggle.isOn ? 1 : 0);
    }

    public void UpdateSlider(Slider slider)
    {
        slider.value = int.Parse(slider.transform.parent.Find("Input").GetComponent<TMP_InputField>().text);
    }

    public void UpdateInput(TMP_InputField input)
    {
        input.text = input.transform.parent.Find("Slider").GetComponent<Slider>().value.ToString();
    }
}
