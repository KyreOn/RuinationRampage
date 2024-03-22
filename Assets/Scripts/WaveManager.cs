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
    [SerializeField] private float            loadSpeed;
    [SerializeField] private GameObject       startPoint;
    [SerializeField] private GameObject       player;
    
    private static List<GameObject>    _arenas;
    private static GameObject          _curArena;
    private static float               _shaderProgress;
    private static bool                _isLoading;
    private static bool                _isLoaded;
    private static Collider            _collider;
    private static CharacterController _cc;
    
    public static WaveManager Instance { get; private set; }
    public static int         currentWave;
    
    
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
        _collider = startPoint.GetComponent<Collider>();
        //_cc = player.GetComponent<CharacterController>();
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
            LoadArena();
        }
        _shaderProgress += Time.deltaTime * loadSpeed * (_isLoading ? 1 : -1);
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
        _curArena = Instantiate(_arenas[pattern], Vector3.zero, Quaternion.identity);
        _curArena.GetComponent<ArenaPattern>().LoadEnemies(currentWave);
    }
    
    public static void CheckForEnemies()
    {
        var count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (count == 0)
        {
            //_isLoading = false;
            _collider.enabled = true;
        }
            
    }

    public static void StartUnload()
    {
        _isLoading = false;
        _isLoaded = false;
        _shaderProgress = 0.5f;
        _collider.enabled = false;
        //_cc.enabled = false;
    }
}
