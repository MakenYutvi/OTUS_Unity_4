using Core;
using Core.Customs;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "Data/DifficultyData")]
public class DifficultyData : ScriptableObject
{
    #region Fields
    [Header("Game Settigs")]
    public bool IsShowCardInBeginningRound = true;
    public float ShowTime = 1.0f;

    [Header("Error Settigs")]
    public bool IsShufflingCards = true;
    public int MaxCountErrors = 1;
    public bool IsCardWithoutPairs = true;

    [Header("HP Settigs")]
    //public HPManagerBehavior HPManager;
    public bool IsHPHandle = true;
    public int MaxHP = 3;
    public GameObject HpPrefab;
    #endregion



    #region Methods
    public void Initialization()
    {
        
        if (HpPrefab == null)
        {
            var manager = CustomResources.Load<GameObject>
                        (AssetsPathGameObject.GameObjects[GameObjectType.HPprefab]);

            HpPrefab = Instantiate(manager);
        }


    }
    #endregion
}

