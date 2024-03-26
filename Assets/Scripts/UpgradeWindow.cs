using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private Upgrade[] upgrades = new Upgrade[3];
    
    private GameObject  _player;
    private Spell[]     _playerSpells;
    private List<Spell> _availableForUpgrade = new List<Spell>();
    private List<int>   _spellsForUpgrade;
    private int         _selectedSkill;
    private int         _availableCount;
    
    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerSpells = _player.GetComponents<Spell>();
        foreach (var spell in _playerSpells)
        {
            _availableForUpgrade.Add(spell);
        }
        _spellsForUpgrade = new List<int>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        gameObject.SetActive(true);
        GetSpellsForUpgrade();
        for (var i = 0; i < 3; i++)
            upgrades[i].Clear();
        for (var i = 0; i < _availableCount; i++)
            upgrades[i].Init(_availableForUpgrade[_spellsForUpgrade[i]]);
    }
    
    
    private void GetSpellsForUpgrade()
    {
        _availableCount = 0;
        foreach (var spell in _playerSpells)
        {
            if (spell.level == spell.maxLevel)
                _availableForUpgrade.Remove(spell);
        }

        _availableCount = Mathf.Clamp(_availableForUpgrade.Count, 0, 3);
        _spellsForUpgrade.Clear();
        while (_spellsForUpgrade.Count < _availableCount)
        {
            var spellId = Random.Range(0, _availableForUpgrade.Count);
            if (_spellsForUpgrade.Contains(spellId)) continue;
            //if (_availableForUpgrade[spellId].isUlt && _availableForUpgrade[spellId].level >= WaveManager.currentWave / 10) continue;
            _spellsForUpgrade.Add(spellId);
        }
    }

    public void SetSelectedSkill(int id)
    {
        _availableForUpgrade[_spellsForUpgrade[id]].Upgrade();
        gameObject.SetActive(false);
        WaveManager.CheckForUpgrade();
    }
}
