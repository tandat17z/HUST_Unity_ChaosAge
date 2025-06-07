using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonLevel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _levelText;

    [SerializeField]
    private Button _buttonSelectLevel;

    [SerializeField]
    private GameObject _pLock;

    [SerializeField]
    private GameObject _pUnlock;

    [SerializeField]
    private GameObject[] _stars;

    private int _level;
    private bool _isLock = false;

    public void SetLevel(int level)
    {
        _level = level;
        _levelText.text = level.ToString();
        _buttonSelectLevel.onClick.AddListener(OnSelectLevel);

        var currentLevel = PlayerPrefs.GetInt("Battle_CurrentLevel", 1);
        var numStar = PlayerPrefs.GetInt($"Battle_Level_{level}", -1);

        _isLock = level > currentLevel;
        _pLock.SetActive(_isLock);
        _pUnlock.SetActive(!_isLock);
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].SetActive(i <= numStar);
        }
    }

    private void OnSelectLevel()
    {
        if (_isLock)
        {
            GameManager.Instance.Log($"Level {_level} is locked");
            return;
        }
        else
        {
            GameManager.Instance.SwitchToBattleAI(_level);
        }
    }
}
