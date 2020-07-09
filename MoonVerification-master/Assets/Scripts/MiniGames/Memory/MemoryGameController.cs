using Core;
using Moon.Asyncs;
using UnityEngine;
using UnityEngine.UI;


namespace MiniGames.Memory
{
    public class MemoryGameController : MonoBehaviour
    {
        #region PrivateData
        private readonly IInitialization[] _allControllers;
        private Canvas _mainCanvas;
        private Controllers _controllers;
        private GameMenuBehaviour _gameMenu;
        private Button _helpButton;
        private TutorialHandBehaviour _tutorialHand;

        #endregion


        #region Fields
        [HideInInspector] public CameraAnimationController CameraAnimationController;
        public CardDealerController CardDealerController;
        public DifficultyController DifficultyController;
        #endregion


        #region Unity Methods
        private void Awake()
        {
            _controllers = new Controllers();
            Initialization();
            ScreenInterface.GetInstance().Execute(ScreenType.GameMenu);
            _mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            _gameMenu = _mainCanvas.GetComponentInChildren<GameMenuBehaviour>();
            _helpButton = _gameMenu.GameMenuHelpButton;
            _tutorialHand = _mainCanvas.GetComponentInChildren<TutorialHandBehaviour>();
            CardDealerController = (CardDealerController)_controllers._initializations[0];
            DifficultyController = (DifficultyController)_controllers._initializations[1];
            CameraAnimationController = GetComponentInChildren<CameraAnimationController>();
        }
        #endregion


        #region MoonAsync Methods
        public AsyncState RunGame(MemoryGameModel gameModel)
        {
            // game login entry point
            return Planner.Chain()
                    .AddAction(CardDealerController.SetImages, gameModel.images)
                    .AddAction(CardDealerController.SetDifficultyController, DifficultyController)
                    .AddFunc(CardDealerController.CardDealing, gameModel.numberOfCardPairs)
                    .AddFunc(_tutorialHand.StartTutorial)
                    .AddAction(() =>
                    {
                        if (gameModel.HelpCount != 0)
                        {
                            _helpButton.gameObject.SetActive(true);
                            CardDealerController.MaxHelpCount = gameModel.HelpCount;
                            CardDealerController.HelpButton = _helpButton;
                        }
                    })
                    .AddAwait(AwaitFunc)
                ;
        }

        private void AwaitFunc(AsyncStateInfo state)
        {
            // todo: game complete condition;
            state.IsComplete = CardDealerController.IsFinishGameRound;
        }
        
        public AsyncState GetDifficultyController()
        {
            return Planner.Chain()
                .AddAction(() => DifficultyControllerForAsync());

        }
        #endregion


        #region  Methods
        public void Initialization()
        {
            _controllers.Initialization();
        }
        private DifficultyController DifficultyControllerForAsync()
        {
            return DifficultyController;
        }
        #endregion
    }
}
