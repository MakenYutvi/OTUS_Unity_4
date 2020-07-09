using DG.Tweening;
using Moon.Asyncs;
using System;
using UnityEngine;


public class TutorialHandBehaviour : MonoBehaviour
{
    #region  PrivateData
    [SerializeField] private GameObject _hand;
    [SerializeField] private Camera _uICamera;
    private CardBehaviour _targetCard;
    private Canvas canvas;
    #endregion


    #region  Fields
    public static Action onClickCard;
    #endregion


    #region Unity Methods
    private void OnEnable()
    {
        onClickCard += () => gameObject.SetActive(false);
    }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        
    }
    private void OnDisable()
    {
        onClickCard -= () => gameObject.SetActive(false);
    }
    #endregion


    #region MoonAsync Methods
    public AsyncState StartTutorial()
    {
        var asyncChain = Planner.Chain();
        if (PlayerPrefs.HasKey("IsFirstStart"))
            return asyncChain.AddEmpty();

        asyncChain
                .AddAction(gameObject.SetActive, true)
                .AddAction(() => _targetCard = FindObjectOfType<CardBehaviour>())
                .AddAwait((AsyncStateInfo state) => state.IsComplete = _targetCard != null)
                .AddFunc(MoveHand)
                .AddAction(() => PlayerPrefs.SetInt("IsFirstStart", 1))
            ;
        return asyncChain;
    }

    private AsyncState MoveHand()
    {
        var asyncChain = Planner.Chain();
        asyncChain.AddTween(MoveTo);
        return asyncChain;
    }
    #endregion


    #region DoTween  Methods
    private Tween MoveTo()
    {
        var position = WorldToUISpace(canvas, _targetCard.transform.position);
        return _hand.GetComponent<RectTransform>().DOMove(position, 1f);
    }
    #endregion


    #region  Methods
    public Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        worldPos -= Vector3.one * 1.5f; //...
        Vector3 screenPos = _uICamera.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out Vector2 movePos);
        return parentCanvas.transform.TransformPoint(movePos);
    }
    #endregion
    
}