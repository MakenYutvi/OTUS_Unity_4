using Core;
using DG.Tweening;
using Moon.Asyncs;
using System;
using System.Collections.Generic;
using UnityEngine;


public class CardBehaviour : MonoBehaviour
{
    #region PrivateData
    private MeshRenderer faceImage;
    private Vector3 _origininalPosition;
    private Vector3 _origininalRotation;
    
    private bool _isFliped;
    private CardData _cardData;
    
    private int _dataCountFlipCardATime;
    #endregion
    public Action<CardBehaviour> OnFlipedCard;


    #region Properties
    public static bool IsTweenRunning { get; private set; }
    #endregion


    #region Unity Methods
    private void OnEnable()
    {
        _isFliped = false;
        OnFlipedCard += AddToFlipedList;
    }
    private void OnDisable()
    {
        OnFlipedCard -= AddToFlipedList;
    }

    private void Awake()
    {
        _cardData = Data.Instance.Card;
        _dataCountFlipCardATime = _cardData.GetСountFlipCardATime();
        faceImage = transform.GetChild(0).GetComponent<MeshRenderer>();
        
    }

    private void OnMouseDown()
    {
        if (_isFliped || IsTweenRunning || CardDealerController.IsDealing || CardDealerController.IsHandleFlipCards)
            return;

        Flip();
        TutorialHandBehaviour.onClickCard?.Invoke();
    }
    #endregion


    #region MoonAsync Methods
    public void HandleFlipedCards()
    {
        var asyncChain = Planner.Chain();
        asyncChain.AddAwait((AsyncStateInfo state) => state.IsComplete = !IsTweenRunning);

        CardDealerController.IsHandleFlipCards = true;
        var matches = new Dictionary<CardBehaviour, List<CardBehaviour>>();
        foreach (var fCard in CardDealerController.FlipedCards)
        {
            if (!matches.ContainsKey(fCard))
                matches.Add(fCard, CardDealerController.FlipedCards.FindAll(c => c.Equals(fCard)));
        }

        var isMatch = false;
        foreach (var mCard in matches.Values)
        {
            if (mCard.Count > 1)
            {
                isMatch = true;
                mCard.ForEach((CardBehaviour card)
                    => asyncChain
                            .JoinTween(card.MoveTo, new Vector3(0f, 2.86f, 10f))
                            .JoinFunc(card.SetActiveGameObject, false)
                      );
            }
            else
            {
                asyncChain
                    .AddTween(mCard[0].Rise)
                    .AddTween(mCard[0].ReRotate)
                    .AddTween(mCard[0].RePut)
                ;
            }
        }

        if (!isMatch)
        {
            asyncChain.AddAction(() => HPManagerBehavior.OnTakingLives?.Invoke());
            asyncChain.AddAction(() => CardShufflingController.onErrorCard?.Invoke());
        }

        asyncChain.AddAction(() =>
        {
            var activeCards = CardDealerController.CardsPool.FindAll(card => card.GameObject.activeSelf);

            var isMatchesExists = false;
            foreach (var aCard in activeCards)
            {
                if (activeCards.FindAll(c => c.GameObject.GetComponent<CardBehaviour>().Equals(aCard.GameObject.GetComponent<CardBehaviour>())).Count > 1)
                    isMatchesExists = true;
            }

            if (!isMatchesExists)
            {
                foreach (var card in activeCards)
                {
                    var cardComponent = card.GameObject.GetComponent<CardBehaviour>();
                    asyncChain
                            .JoinTween(cardComponent.MoveTo, new Vector3(0f, 2.86f, 10f))
                            .JoinFunc(cardComponent.SetActiveGameObject, false)
                        ;

                    asyncChain.AddFunc(() => cardComponent.SetActiveGameObject(false));
                }
                asyncChain.AddAction(() => CardDealerController.IsFinishGameRound = true);
            }
        });

        asyncChain.AddAwait((AsyncStateInfo state) => state.IsComplete = !IsTweenRunning);
        asyncChain.AddAction(CardDealerController.FlipedCards.Clear);
        asyncChain.onComplete += () => CardDealerController.IsHandleFlipCards = false;
    }
    public AsyncState MoveToTable(Vector3 movePosition)
    {
        return Planner.Chain()
                    .AddTween(MoveTo, movePosition)
                ;
    }

    public AsyncState SetActiveGameObject(bool isActive)
    {
        return Planner.Chain()
            .AddAwait((AsyncStateInfo state) => state.IsComplete = !IsTweenRunning)
            .AddAction(gameObject.SetActive, isActive);
    }
    public void Flip()
    {
        _isFliped = true;

        var asyncChain = Planner.Chain();
        asyncChain
            .AddTween(Rise)
            .AddTween(Rotate)
            .AddTween(Put)
            .AddAction(OnFlipedCard.Invoke, this)
        ;
    }

    #endregion


    #region  Methods
    
    public void AddToFlipedList(CardBehaviour card)
    {
        CardDealerController.FlipedCards.Add(card);
        if (CardDealerController.FlipedCards.Count == _dataCountFlipCardATime)
            HandleFlipedCards();
        
    }


    public void SetImage(Texture2D image)
    {
        faceImage.material.mainTexture = image;
    }

    public Texture2D GetImage()
    {
        return (Texture2D)faceImage.material.mainTexture;
    }
    public override bool Equals(object other)
    {
        var otherCard = other as CardBehaviour;
        return faceImage.material.mainTexture.name == otherCard.faceImage.material.mainTexture.name;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion


    #region DoTween Methods

    public Tween MoveTo(Vector3 movePosition)
    {
        IsTweenRunning = true;
        var tween = transform
                    .DOMove(movePosition, 1f)
                    .SetEase(Ease.InExpo);
        tween.onComplete += () => IsTweenRunning = false;
        return tween;
    }

    public Tween MoveTo(Vector3 movePosition, float duration, Ease easyType)
    {
        IsTweenRunning = true;
        var tween = transform
                    .DOMove(movePosition, duration)
                    .SetEase(easyType);
        tween.onComplete += () => IsTweenRunning = false;
        return tween;
    }

    public Tween Rise()
    {
        IsTweenRunning = true;
        _origininalPosition = transform.position;
        var tween = transform
                .DOMove(transform.position + Vector3.up * .5f, 1f);
        return tween;
    }

    public Tween ReRotate()
    {
        return transform
                .DOLocalRotateQuaternion(Quaternion.Euler(_origininalRotation), 1f);
    }

    public Tween Rotate()
    {
        _origininalRotation = transform.eulerAngles;
        return transform
                .DOLocalRotateQuaternion(Quaternion.Euler(90f, 0f, 0f), 1f);
    }

    public Tween RePut()
    {
        _isFliped = false;
        var tween = transform
                .DOMove(_origininalPosition, 1f);
        tween.onComplete += () => IsTweenRunning = false;
        return tween;
    }

    public Tween Put()
    {
        var tween = transform
                .DOMove(_origininalPosition, 1f);
        tween.onComplete += () => IsTweenRunning = false;
        return tween;
    }

    public Tween Shake()
    {
        IsTweenRunning = true;
        var tween = transform.DOPunchRotation(Vector3.up * 30f, 1f);
        tween.onComplete += () => IsTweenRunning = false;
        return tween;
    }

    #endregion
}