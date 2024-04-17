using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Image[]      abilities;
    [SerializeField] private GameObject[] availableCharacters;
    [SerializeField] private GameObject[] chosenPerksUI;
    [SerializeField] private GameObject[] perksToChose;
    [SerializeField] private GameObject[] characterPerks;
    [SerializeField] private TMP_Text     goldCount;
    [SerializeField] private TMP_Text     perkCount;
    [SerializeField] private TMP_Text     level;
    [SerializeField] private TMP_Text     nextLevel;
    [SerializeField] private Slider       levelProgress;
    [SerializeField] private GameObject   abilityToolTip;
    
    [SerializeField] private int[] xpToLvlUp = new int[10];
    
    private int       _curChar;
    private int       _level;
    private Perk[]    _perks;
    private List<int> _chosenPerks;
    private List<int> _unlockedPerks;
    private Spell[]   _spells;
    
    private void Awake()
    {
        _chosenPerks = new List<int>();
    }

    private void Start()
    {
        var availableChars = PlayerPrefs.GetString("AvailableChars");
        var chars          = availableChars.Split(",").Select(int.Parse);
        for (var i = 0; i < availableCharacters.Length; i++)
        {
            availableCharacters[i].GetComponent<Selectable>().interactable = chars.Contains(i);
        }
        var prefString = PlayerPrefs.HasKey($"UnlockedPerks{_curChar}") ? PlayerPrefs.GetString($"UnlockedPerks{_curChar}") : "";
        _unlockedPerks = prefString == "" ? new List<int>() : prefString.Split(",").Select(int.Parse).ToList();

        var lastSelected = PlayerPrefs.GetInt("LastSelected");
        SetCharacter(lastSelected);
    }
    
    public void SetAbilities(int id)
    {
        PlayerPrefs.SetInt("LastSelected", id);
        _curChar = id;
        _spells     = characters[id].GetComponents<Spell>();
        var prefString = PlayerPrefs.HasKey($"UnlockedPerks{id}") ? PlayerPrefs.GetString($"UnlockedPerks{id}") : "";
        _unlockedPerks = prefString == "" ? new List<int>() : prefString.Split(",").Select(int.Parse).ToList();
        for (var i = 0; i < 6; i++)
        {
            abilities[i].sprite = _spells.First(x => x.id == i).enabledSprite;
        }
    }

    public void SetCharacter(int id)
    {
        SetAbilities(id);
        SetPerks(id);
        SetChosenPerks(id);
        SetGoldCount(id);
        SetXP(id);
        SetPerkCount(id);
    }
    
    public void SetPerks(int id)
    {
        var perks         = characterPerks[id].GetComponents<Perk>();
        _perks = perks;
        for (var i = 0; i < 10; i++)
        {
            var perkImage  = perksToChose[i].GetComponent<Image>();
            var perk       = _perks.First(x => x.id == i);
            perkImage.sprite = _unlockedPerks.Contains(i) ? perk.enabledSprite : perk.disabledSprite;
        }
    }

    public void SetChosenPerks(int id)
    {
        var prefString  = PlayerPrefs.HasKey($"ChosenPerks{id}") ? PlayerPrefs.GetString($"ChosenPerks{id}") : "";
        var chosenPerks = prefString == "" ? new List<int>() : prefString.Split(",").Select(int.Parse);
        var chosenList  = new List<int>();
        _chosenPerks.Clear();
        foreach (var perk in chosenPerks)
        {
            _chosenPerks.Add(perk);
        }

        for (var i = 0; i < 4; i++)
        {
            chosenPerksUI[i].GetComponent<Image>().sprite = null;
        }
        
        for (var i = 0; i < _chosenPerks.Count; i++)
        {
            //Debug.Log(i);
            var perk = _perks.First(x => x.id == _chosenPerks[i]);
            chosenPerksUI[i].GetComponent<Image>().sprite = perk.enabledSprite;
        }
    }

    public void SetXP(int charId)
    {
        var xp = PlayerPrefs.HasKey($"Xp{charId}") ? PlayerPrefs.GetInt($"Xp{charId}") : 0;
        xp = Mathf.Clamp(xp, 0, 1000);
        for (var i = 0; i < xpToLvlUp.Length; i++)
        {
            if (xp >= xpToLvlUp[i]) continue;
            _level = i;
            Debug.Log(i);
            break;
        }

        level.text = _level.ToString();
        nextLevel.text = _level == 10 ? "" : (_level + 1).ToString();

        var needXP  = xpToLvlUp[_level] - xp;
        var levelXP = xpToLvlUp[_level] - (_level == 0 ? 0 : xpToLvlUp[_level - 1]);
        levelProgress.value = 1 - (float)needXP / (float)levelXP;
        var perkCount = _level - _unlockedPerks.Count;
        PlayerPrefs.SetInt($"Perk{charId}", perkCount);
    }

    public void SetGoldCount(int charId)
    {
        goldCount.text = PlayerPrefs.HasKey($"Gold{charId}") ? PlayerPrefs.GetInt($"Gold{charId}").ToString() : "0";
    }
    
    public void SetPerkCount(int charId)
    {
        perkCount.text = PlayerPrefs.HasKey($"Perk{charId}") ? PlayerPrefs.GetInt($"Perk{charId}").ToString() : "0";
    }
    
    public void AddPerk(int perkId)
    {
        if (_chosenPerks.Count == 4) return;
        if (_chosenPerks.Contains(perkId)) return;
        if (!_unlockedPerks.Contains(perkId)) return;
        _chosenPerks.Add(perkId);
        _chosenPerks.Sort();
        var sb = new StringBuilder();
        sb.AppendJoin(',', _chosenPerks.Select(x => x.ToString()));
        PlayerPrefs.SetString($"ChosenPerks{_curChar}", sb.ToString());
        SetChosenPerks(_curChar);
    }

    public void RemovePerk(int perkId)
    {
        if (_chosenPerks.Count <= perkId) return;
        _chosenPerks.RemoveAt(perkId);
        var sb = new StringBuilder();
        sb.AppendJoin(',', _chosenPerks.Select(x => x.ToString()));
        PlayerPrefs.SetString($"ChosenPerks{_curChar}", sb.ToString());
        SetChosenPerks(_curChar);
    }

    public void UnlockPerk(int perkId)
    {
        if (_unlockedPerks.Contains(perkId)) return;
        var perkCount = PlayerPrefs.GetInt($"Perk{_curChar}");
        if (perkCount <= 0) return;
        perkCount--;
        PlayerPrefs.SetInt($"Perk{_curChar}", perkCount);
        SetPerkCount(_curChar);
        _unlockedPerks.Add(perkId);
        var sb = new StringBuilder();
        sb.AppendJoin(',', _unlockedPerks.Select(x => x.ToString()));
        PlayerPrefs.SetString($"UnlockedPerks{_curChar}", sb.ToString());
        SetPerks(_curChar);
    }

    public void ShowAbilityToolTip(int id)
    {
        //abilityToolTip.SetActive(true);
    }

    public void HideAbilityToolTip()
    {
        abilityToolTip.SetActive(false);
    }
}
