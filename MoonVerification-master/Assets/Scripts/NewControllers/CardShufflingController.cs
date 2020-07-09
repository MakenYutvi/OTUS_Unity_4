using Core.Customs;
using DG.Tweening;
using Moon.Asyncs;
using System;
using System.Collections.Generic;
using UnityEngine;


public class CardShufflingController : MonoBehaviour
{
    #region Private Data
    private int maxErrors;
    #endregion


    #region Fields
    public static Action onErrorCard;
    #endregion


    #region Properties
    public bool IsShuffle { get; private set; }

    public List<Card> Cards { private get; set; }
    #endregion


    #region Unity Methods
    private void OnEnable()
    {
        onErrorCard += ErrorCard;
    }
    private void OnDisable()
    {
        onErrorCard -= ErrorCard;
    }
    #endregion


    #region Methods
    public void SetMaxErrors(int maxErrors)
    {
        this.maxErrors = maxErrors;
    }

    public void ErrorCard()
    {
        maxErrors--;
        if (maxErrors == 0)
            Shuffle();
    }
    #endregion


    #region MoonAsync Methods
    public AsyncState Shuffle()
    {
        var asyncChain = Planner.Chain();
        asyncChain.AddEmpty();

        asyncChain.AddAwait((AsyncStateInfo state) => state.IsComplete = !CardBehaviour.IsTweenRunning);
        IsShuffle = true;
        asyncChain.AddAction(CustomDebug.Log, "Start shuffl");

        var activeCards = Cards.FindAll(c => c.GameObject.activeSelf == true);
        for (int i = 0; i < activeCards.Count; i++)
        {
            var j = UnityEngine.Random.Range(i, activeCards.Count);

            asyncChain.AddFunc(Switching, activeCards, i, j);
            asyncChain.AddAwait((AsyncStateInfo state) => state.IsComplete = !CardBehaviour.IsTweenRunning);
        }

        asyncChain.AddAction(CustomDebug.Log, "End Shuffle");
        return asyncChain;
    }

    private AsyncState Switching(List<Card> list, int i, int j)
    {
        var asyncChain = Planner.Chain();

        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;

        asyncChain.AddTween(list[j].GameObject.GetComponent<CardBehaviour>().MoveTo, list[i].Transform.position, 0.25f, Ease.InSine);
        asyncChain.AddTween(list[i].GameObject.GetComponent<CardBehaviour>().MoveTo, list[j].Transform.position, 0.25f, Ease.InSine);
        asyncChain.AddAwait((AsyncStateInfo state) => state.IsComplete = !CardBehaviour.IsTweenRunning);
        return asyncChain;
    }
    #endregion
}