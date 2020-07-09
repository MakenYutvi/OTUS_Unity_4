using Core.Customs;
using MiniGames.Memory;
using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Core
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Data")]
    public sealed class Data : ScriptableObject
    {
        #region PrivateData
        
        [SerializeField] private string _cardDataPath;
        [SerializeField] private string _memoryGameModelDataPath;
        [SerializeField] private string _difficultyDataPath; 

        private static CardData _cardData;
        private static MemoryGameModel _memoryGameModel;
        private static DifficultyData _difficultyData;
        private static readonly Lazy<Data> _instance = new Lazy<Data>(() => Load<Data>("Data/" + typeof(Data).Name));
        
        #endregion
        

        #region Properties

        public static Data Instance => _instance.Value;

        public CardData Card
        {
            get
            {
                if (_cardData == null)
                {
                    _cardData = Load<CardData>("Data/" + Instance._cardDataPath);
                }

                return _cardData;
            }
        }

        public DifficultyData DifficultyData
        {
            get
            {
                if (_difficultyData == null)
                {
                    _difficultyData = Load<DifficultyData>("Data/" + Instance._difficultyDataPath);
                }

                return _difficultyData;
            }
        }
        public MemoryGameModel MemoryGameModel
        {
            get
            {
                if (_memoryGameModel == null)
                {
                    _memoryGameModel = Load<MemoryGameModel>("Data/" + Instance._memoryGameModelDataPath);
                }

                return _memoryGameModel;
            }
        }


        #endregion


        #region Methods

        private static T Load<T>(string resourcesPath) where T : Object =>
            CustomResources.Load<T>(Path.ChangeExtension(resourcesPath, null));
    
        #endregion
    }
}
