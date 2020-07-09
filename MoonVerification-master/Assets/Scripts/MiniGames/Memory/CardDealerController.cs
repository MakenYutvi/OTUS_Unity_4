using Core;
using Core.Customs;
using Moon.Asyncs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardDealerController : IInitialization
{
    #region PrivateData
    private readonly CardData _cardData;
    private readonly GameObject _cardPrefab;
    private readonly int _dataCountFlipCardATime;
    private readonly CardShufflingController _cardShuffl;
    private DifficultyController _difficultyController;
    private Texture2D[] images;
    #endregion


    #region Fields
    public static List<CardBehaviour> FlipedCards = new List<CardBehaviour>();
    public static List<Card> CardsPool = new List<Card>();
    #endregion


    #region Properties
    public static bool IsTweenRunning { get; private set; }
    public static bool IsDealing { get; private set; }
    public static bool IsHandleFlipCards { get; set; }
    public static bool IsFinishGameRound { get; set; }

    public Button HelpButton { get; set; }
    public int MaxHelpCount { get; set; }
    #endregion


    #region Class LyfeCycle
    public CardDealerController()
    {
        _cardData  = Data.Instance.Card;
        CustomDebug.Log("Initialize CardDealerController");
        _cardPrefab = _cardData.GetСardPrefab();
        _dataCountFlipCardATime = _cardData.GetСountFlipCardATime();
        _cardShuffl = _cardData.GetCardShuffl();
    }
    #endregion


    #region MoonAsync Methods
    public AsyncState CardDealing(int numberOfPairs)
    {
        if (HelpButton != null)
            HelpButton.interactable = true;

        IsFinishGameRound = false;
        var asyncChain = Planner.Chain();
        asyncChain.AddAction(Debug.Log, "Dealing started");
        IsDealing = true;

        var movePosition = new Vector3(-1.0f, 0.0f, 0f);
        var moveOffset = Vector3.right * 0.8f;
        for (var i = 0; i < numberOfPairs * 2; i++)
        {
            CardBehaviour card = null;

            var instancePosition = new Vector3(0f, 0.2f, 10f);
            var instanceRotation = _cardPrefab.transform.rotation;

            if (CardsPool.Count > i && !CardsPool[i].GameObject.activeSelf)
            {
                CardsPool[i].Transform.position = instancePosition;
                CardsPool[i].Transform.rotation = instanceRotation;

                card = CardsPool[i].GameObject.GetComponent<CardBehaviour>();
                card.SetImage(GetImage(i));
                card.gameObject.SetActive(true);
                asyncChain.AddFunc(card.MoveToTable, movePosition);

                movePosition += moveOffset;
                continue;
            }
            var cardFactory = new CardFactory();
            var cardObject = cardFactory.CreateCard(instancePosition, instanceRotation);
            CardsPool.Add(cardObject);

            card = cardObject.GameObject.GetComponent<CardBehaviour>();
            card.SetImage(GetImage(i));
            asyncChain.AddFunc(card.MoveToTable, movePosition);

            movePosition += moveOffset;
        }

        asyncChain.AddFunc(_difficultyController.ShowCardsInBeginning, CardsPool);
        asyncChain.AddAction(Debug.Log, "Dealing finished");
        asyncChain.onComplete += () => IsDealing = false;
        return asyncChain;
    }

    public void HighlightMatcheCards()
    {
        var asyncChain = Planner.Chain();
        asyncChain.AddEmpty();
        var activeCards = CardsPool.FindAll(p => p.GameObject.activeSelf);
        for (int i = 0; i < activeCards.Count; i++)
        {
            var matches = activeCards.FindAll(c
                    => c.GameObject.GetComponent<CardBehaviour>().GetImage()
                    .Equals(activeCards[i].GameObject.GetComponent<CardBehaviour>().GetImage())
                );

            if (matches.Count >= _dataCountFlipCardATime)
            {
                for (int j = 0; j < _dataCountFlipCardATime; j++)
                    asyncChain.JoinTween(matches[j].GameObject.GetComponent<CardBehaviour>().Shake);

                MaxHelpCount--;
                if (MaxHelpCount == 0 && HelpButton != null)
                    HelpButton.interactable = false;
                return;
            }
        }
    }
    #endregion


    #region Methods
    private void SetCardShufflData()
    {
        if (_cardShuffl != null)
        {
            if (!_difficultyController.Config.IsShufflingCards)
                CardShufflingController.onErrorCard = null;

            _cardShuffl.SetMaxErrors(_difficultyController.Config.MaxCountErrors);
            _cardShuffl.Cards = CardsPool;
        }
    }

    public void SetDifficultyController(DifficultyController controler)
    {
        _difficultyController = controler;
        SetCardShufflData();
    }
    public void SetImages(Texture2D[] images)
    {
        this.images = images;
    }

    private Texture2D GetImage(int index)
    {
        Texture2D image = null;
        if (_difficultyController.Config.IsCardWithoutPairs)
            return images[UnityEngine.Random.Range(0, images.Length)];

        if (CardsPool.Count == 1 || index < CardsPool.Count / 2)
            image = images[UnityEngine.Random.Range(0, images.Length)];
        else
        {
            var ind = UnityEngine.Random.Range((index - CardsPool.Count / 2), CardsPool.Count / 2);
            image = CardsPool[ind].GameObject.GetComponent<CardBehaviour>().GetImage();
        }

        return image;
    }

    public void Initialization()
    {
        _cardData.Initialization();
    }
    #endregion
}
