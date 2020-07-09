using Core;
using UnityEngine;


public class Card : IModel
{
    #region PrivateData
    private bool _isActive;
    #endregion


    #region Fields
    public GameObject GameObject { get; }
    public Transform Transform { get; }
    public CardBehaviour CardBehaviour { get; }
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

        }
    }
    #endregion


    #region Class LifeCycle
    public Card(GameObject cardObject)
    {        
        GameObject = cardObject;
        Transform = GameObject.transform;
        CardBehaviour = GameObject.GetComponent<CardBehaviour>();
    }
    #endregion


    #region  Methods
    public virtual void SetActive(bool value)
    {
        IsActive = value;
        if (value)
        {
            Transform.SetParent(null);
            GameObject.SetActive(true);

        }
        else
        {
            GameObject.SetActive(false);
            Transform.position = Vector3.zero;
            
        }
    }
    #endregion

}
