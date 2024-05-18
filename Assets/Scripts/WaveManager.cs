using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> arenaPatterns;
    [SerializeField] private List<Material>   arenaMaterials;
    [SerializeField] private GameObject       bossArena;
    [SerializeField] private float            loadSpeed;
    [SerializeField] private GameObject       startPoint;
    [SerializeField] private GameObject       player;
    [SerializeField] private GameObject       upgradeWindow;
    [SerializeField] private GameObject       particleSystem;

    private static CameraController    _camera;
    private static GameHUD             _gameHUD;
    private static List<GameObject>    _arenas;
    private static GameObject          _bossArena;
    private static GameObject          _curArena;
    private static float               _shaderProgress;
    private static bool                _isLoading;
    private static bool                _isLoaded;
    private static Collider            _collider;
    private static CharacterController _cc;
    private static LevelSystem         _levelSystem;
    private static UpgradeWindow       _upgradeWindow;
    private static ParticleSystem      _particleSystem;
    private static bool                _readyToLoad = true;
    
    public static WaveManager Instance { get; private set; }
    public static int         currentWave = 4;
    
    
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
        _arenas = arenaPatterns;
        _bossArena = bossArena;
        _collider = startPoint.GetComponent<Collider>();
        //_cc = player.GetComponent<CharacterController>();
        _levelSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
        _upgradeWindow = upgradeWindow.GetComponent<UpgradeWindow>();
        _particleSystem = particleSystem.GetComponent<ParticleSystem>();
        _camera = FindObjectOfType<CameraController>();
        _gameHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameHUD>();
        LoadArena();
    }

    private void Update()
    {
        if (_isLoaded) return;
        if (_isLoading && _shaderProgress >= Mathf.PI * 0.5f)
        {
            //_cc.enabled = true;
            _isLoaded = true;
        }
        if (!_isLoading && _shaderProgress <= 0)
        {
            Destroy(_curArena);
            currentWave++;
            if (currentWave == 20)
                currentWave = 41;
            if (currentWave == 42)
                currentWave = 20;
            LoadArena();
        }
        _shaderProgress += Time.deltaTime * loadSpeed * (_isLoading ? 1 : -1) * (_readyToLoad ? 1 : 0);
        foreach (var material in arenaMaterials)
        {
            material.SetFloat("_Cutoff_Height", 100 * Mathf.Abs(Mathf.Sin(_shaderProgress)));
        }
    }

    private static void LoadArena()
    {
        _isLoading = true;
        _isLoaded = false;
        var pattern = Random.Range(0, _arenas.Count);
        if (currentWave == 41)
        {
            _curArena = Instantiate(_bossArena, Vector3.zero, Quaternion.identity);
            _curArena.GetComponent<ArenaPattern>().LoadEnemies(currentWave);
        }
        else
        {
            _curArena = Instantiate(_arenas[pattern], Vector3.zero, Quaternion.identity);
            _curArena.GetComponent<ArenaPattern>().LoadEnemies(currentWave);
        }
        CheckForUpgrade();
    }
    
    public static void CheckForEnemies()
    {
        if (_curArena.GetComponent<ArenaPattern>().OnEnemyDeath())
        {
            //_isLoading = false;
            _collider.enabled = true;
            if (currentWave == 39)
            {
                _gameHUD.OnComplete();
                _camera.OnDeath();
            }
        }
    }

    public static void StartUnload()
    {
        _isLoading = false;
        _isLoaded = false;
        _shaderProgress = 0.5f;
        _collider.enabled = false;
        //_cc.enabled = false;
        //Time.timeScale = 0;
    }

    public static void CheckForUpgrade()
    {
        if (_levelSystem.CheckForUpgrades())
        {
            _readyToLoad = false;
            _upgradeWindow.Open();
            Time.timeScale = 0;
        }
        else
        {
            _readyToLoad = true;
            Time.timeScale = 1;
        }
    }
}
