using Core;
using Core.Customs;
using Moon.Asyncs;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : IInitialization
{
    #region PrivateData
    private readonly DifficultyData _difficultyData;
    //private readonly HPManagerBehavior _hpManager;
    #endregion


    #region Properties
    public DifficultyData Config { get => _difficultyData; }
    #endregion


    #region ClassLifeCycle
    public DifficultyController()
    {
        CustomDebug.Log("Initialize DifficultyController");
        _difficultyData = Data.Instance.DifficultyData;
        //_hpManager =  GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<HPManagerBehavior>();

    }
    #endregion


    #region MoonAsycs Methods
    public AsyncState ShowCardsInBeginning(List<Card> cardsPool)
    {
        var asyncChain = Planner.Chain();
        if (!_difficultyData.IsShowCardInBeginningRound)
            return asyncChain.AddEmpty();

        foreach (var cardP in cardsPool)
        {
            var card = cardP.GameObject.GetComponent<CardBehaviour>();
            asyncChain
                .JoinTween(card.Rise)
                .JoinTween(card.Rotate)
                ;
        }

        asyncChain.AddAction(CustomDebug.Log, "Show");
        asyncChain.AddTimeout(_difficultyData.ShowTime);
        asyncChain.AddAction(CustomDebug.Log, "Close");
        foreach (var cardP in cardsPool)
        {
            var card = cardP.GameObject.GetComponent<CardBehaviour>();
            asyncChain
                .JoinTween(card.ReRotate)
                .JoinTween(card.RePut);
        }

        return asyncChain;
    }

    //public AsyncState HandleHP()
    //{
    //    var asyncChain = Planner.Chain();
    //    if (!_difficultyData.IsHPHandle)
    //        return asyncChain.AddEmpty();

    //    asyncChain.AddFunc(_hpManager.Execute, _difficultyData.HpPrefab, _difficultyData.MaxHP);
    //    return asyncChain;
    //}
    #endregion


    #region Methods
    public void Initialization()
    {
        _difficultyData.Initialization();
    }
    #endregion
}