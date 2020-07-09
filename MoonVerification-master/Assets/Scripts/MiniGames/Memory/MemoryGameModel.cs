using UnityEngine;
using UnityEngine.U2D;
using Core;
using UnityEngine.UI;


namespace MiniGames.Memory
{
    [CreateAssetMenu(fileName = "MemoryGameModel", menuName = "MiniGames/Memory/MemoryGameModel")]
    public class MemoryGameModel: ScriptableObject
    {
        #region PrivateData
        [SerializeField] private CardData _сardDataSO;
        [SerializeField] private RoundParams[] _rounds;
        private DifficultyController _difficultyController;
        #endregion


        #region Fields
        public int numberOfCardPairs = 3;
        public int numberOfRounds = 5;
        public int HelpCount = 1;

        public Texture2D[] images;
        #endregion


        #region Methods
        public void SetCardData(CardData сardData)
        {
            this._сardDataSO = сardData;
        }

        public CardData GetCardData()
        {
            return _сardDataSO;
        }
        public void SetDifficultyController(DifficultyController difficultyController)
        {
            this._difficultyController = difficultyController;
        }

        public DifficultyController GetController()
        {
            return _difficultyController;
        }
        
        public RoundParams[] GetRounds()
        {
            return _rounds;
        }

        public float GetHelpCount()
        {
            return HelpCount;
        }
        #endregion


        [System.Serializable]
        public class RoundParams
        {
            #region PrivateData
            //[SerializeField] private int _numberOfCardPairs;
            //[SerializeField] private SpriteAtlas _spriteAtlas;

            [SerializeField] private CardType _type = CardType.Animals;
            [SerializeField] private Image _images;
            #endregion
        }

    }
}