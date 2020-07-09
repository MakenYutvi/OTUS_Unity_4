using Core;
using Core.Customs;
using Moon.Asyncs;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MiniGames.Common
{
    public class GameScenarioExecutor: MonoBehaviour
    {
        #region  PrivateData
        private int _nextLoadSceneNumber = 1;
        private GameProgress _gameProgress;
        private GameScenarioBase _gameScenarioBase;

        private int _nextSceneNumber;
        #endregion


        #region Unity Methods
        private void Awake()
        {
            _gameScenarioBase = FindObjectOfType<GameScenarioBase>();
            _gameProgress = FindObjectOfType<GameProgress>();
                        
            _nextSceneNumber = SceneManager.GetActiveScene().buildIndex + _nextLoadSceneNumber;
            if (_gameScenarioBase == null)
            {
                var scenario = CustomResources.Load<GameScenarioBase>
                            (AssetsPathGameObject.GameObjects[GameObjectType.GameScenarioExecutor]);

                _gameScenarioBase = Instantiate(scenario);
                if (_gameScenarioBase == null)
                {
                    CustomDebug.LogError("GameScenarioExecutor has no scenario", this);
                    return;
                }
                    
            }
            _gameScenarioBase.Inject(_gameProgress);

            Planner.Chain()
                    .AddFunc(_gameScenarioBase.ExecuteScenario)
                    .AddAction(LoadNextScene)
                ;
        }
        #endregion


        #region  Methods
        private void LoadNextScene()
        {
            // TODO: fade in
            SceneManager.LoadScene(_nextSceneNumber);
            // TODO: fade out
        }
        #endregion
    }
}
