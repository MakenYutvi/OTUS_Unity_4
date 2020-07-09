using Core;
using Core.Customs;
using MiniGames.Common;
using Moon.Asyncs;
using UnityEngine;


namespace MiniGames.Memory
{
    public class MemoryScenario : GameScenarioBase
    {
        #region PrivateData
        private MemoryGameController _memoryGameController;
        //TODO: select gameModel with difficulty controller class
        private DifficultyController _difficultyController;

        private MemoryGameModel _gameModelSO;
        private CardData _cardData;
        private float _timeOutForSpawnCardAfterCameraAnimation = 8.2f;
        #endregion


        #region Unity Methods
        private void Awake()
        {
            _memoryGameController = GetComponentInParent<MemoryGameController>();
            _difficultyController = _memoryGameController.DifficultyController;
            _gameModelSO = Data.Instance.MemoryGameModel;
            _cardData = _gameModelSO.GetCardData();

        }
        #endregion


        #region MoonAsync Methods
        protected override AsyncState OnExecute()
        {
            return Planner.Chain()
                    .AddFunc(Intro)
                    .AddFunc(GameCircle)
                    .AddFunc(Outro)
                ;
        }

        private AsyncState Intro()
        {
            return Planner.Chain()
                    .AddAction(CustomDebug.Log, "start intro")
                    .AddFunc(_memoryGameController.CameraAnimationController.StartCutScene)
                    .AddTimeout(_timeOutForSpawnCardAfterCameraAnimation)
                    .AddAction(CustomDebug.Log, "intro finished")
                ;
        }

        
        private AsyncState GameCircle()
        {
            var asyncChain = Planner.Chain();
            asyncChain.AddAction(Debug.Log, "game started");
            // TODO: implement game circle using game controller
            // TODO: move hardcoded "5" count to game config
            asyncChain.AddFunc(progress.ShowProgressBar);
            asyncChain.AddAction(() => progress.NumberOfRounds = _gameModelSO.numberOfRounds);

            for (var i = 0; i < _gameModelSO.numberOfRounds; i++)
            {
                asyncChain
                        .AddAction(_gameModelSO.SetDifficultyController, _difficultyController)
                        .AddAction(() => progress.HandleHP())
                        .AddFunc(_memoryGameController.RunGame, _gameModelSO)
                        .AddFunc(progress.IncrementProgress)
                    ;
            }

            asyncChain.AddFunc(progress.CloseProgressBar);
            asyncChain.AddAction(Debug.Log, "game finished");
            return asyncChain;
        }

        private AsyncState Outro()
        {
            // TODO: outro cut scene. Run camera back to origin position. Await animation finish
            return Planner.Chain()
                    .AddAction(CustomDebug.Log, "start outro")
                    .AddFunc(_memoryGameController.CameraAnimationController.RewardCutScene)
                    .AddTimeout(1f)
                    .AddAction(CustomDebug.Log, "outro finished")
                ;
        }
        #endregion
    }
}