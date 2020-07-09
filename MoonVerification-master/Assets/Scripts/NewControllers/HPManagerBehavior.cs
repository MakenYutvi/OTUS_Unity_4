using Core;
using Moon.Asyncs;
using System;
using System.Collections.Generic;
using UnityEngine;


public class HPManagerBehavior : MonoBehaviour
{
    #region PrivateData
    private List<GameObject> _hpCounts = new List<GameObject>();
    private int maxHP;
    private int _indexNumber = 1;
    private float _offsetX = 140.0f;
    private float _offsetY = 200.0f;
    private float _offsetZ = 0.0f;
    private float _imageSpacing = 75.0f;
    private string _HPobjectName = "HP";
    private GameMenuBehaviour _gameMenu;
    #endregion


    #region Fields
    public static Action OnTakingLives;
    #endregion


    #region Unity Methods
    private void OnEnable()
    {
        OnTakingLives += TakingLives;
    }

    private void OnDisable()
    {
        OnTakingLives -= TakingLives;
    }
    #endregion


    #region MoonAsync Methods
    public AsyncState Execute(GameObject prefab, int maxHP)
    {
        var asyncChain = Planner.Chain();
        asyncChain.AddEmpty();
        this.maxHP = maxHP;
        if (_hpCounts.Count == maxHP)
            return asyncChain;
        _gameMenu = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<GameMenuBehaviour>();
        gameObject.SetActive(true);
        for (int i = 0; i < maxHP; i++)
        {
            var hpObject = Instantiate(prefab, _gameMenu.transform);
            hpObject.transform.localPosition = new Vector3(_offsetX + i * _imageSpacing, _offsetY, _offsetZ);
            hpObject.name = _HPobjectName + (i + _indexNumber);
            _hpCounts.Add(hpObject);
        }

        return asyncChain;
    }
    #endregion


    #region Methods
    public void TakingLives()
    {
        maxHP--;
        _hpCounts.FindLast(h => h.activeSelf == true).SetActive(false);
        
        if (maxHP <= 0)
            ScreenInterface.GetInstance().Execute(ScreenType.RestartMenu);
    }
    #endregion
}
