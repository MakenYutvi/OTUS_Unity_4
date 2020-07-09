using Core;
using Core.Customs;
using Moon.Asyncs;
using UnityEngine;


namespace MiniGames.Common
{
    public class GameProgress : MonoBehaviour
    {
        #region PrivateData
        private DifficultyData _difficultyData;
        private HPManagerBehavior _hpManager;
        private ProgressBar _progressBar;
        private GameMenuBehaviour _gameMenu;
        private Canvas _canvas;
        #endregion


        #region Properties
        public float NumberOfRounds { private get; set; }
        #endregion


        #region Unity Methods
        private void Awake()
        {
            _difficultyData = Data.Instance.DifficultyData;
            _canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            _gameMenu = _canvas.GetComponentInChildren<GameMenuBehaviour>(); //GameObject.FindGameObjectWithTag("MainCanvas").
            _hpManager = _canvas.GetComponentInChildren<HPManagerBehavior>();
            var progressBarBehaviour = CustomResources.Load<ProgressBar>
                            (AssetsPathGameObject.GameObjects[GameObjectType.ProgressBar]);

            _progressBar = Instantiate(progressBarBehaviour, _gameMenu.transform);
            
        }
        #endregion


        #region MoonAsync Methods
        public AsyncState IncrementProgress()
        {
            return Planner.Chain()
                    // TODO: run progress animation, await finish
                    .AddTween(_progressBar.SetCurrentValue, 1f / NumberOfRounds)
                    .AddTimeout(1f)
                ;
        }

        public AsyncState ShowProgressBar()
        {
            return Planner.Chain()
                    .AddFunc(_progressBar.Show)
                ;
        }

        public AsyncState CloseProgressBar()
        {
            return Planner.Chain()
                        .AddFunc(_progressBar.Close)
                    ;
        }
        #endregion


        #region  Methods
        public void ResetProgress(int count)
        {
            // TODO: reset progress to zero. Set progress max
        }

        public AsyncState HandleHP()
        {
            var asyncChain = Planner.Chain();
            if (!_difficultyData.IsHPHandle)
                return asyncChain.AddEmpty();

            asyncChain.AddFunc(_hpManager.Execute, _difficultyData.HpPrefab, _difficultyData.MaxHP);
            return asyncChain;
        }
        #endregion
    }
}