using System.Collections.Generic;


namespace Core
{
    public sealed class AssetsPathGameObject
    {
        #region Fields

        public static readonly Dictionary<GameObjectType, string> GameObjects = new Dictionary<GameObjectType, string>
        {
            {
                GameObjectType.Canvas, "GUI/GUI_Canvas"
            },
            {
                GameObjectType.GameModel, "Prefabs/MemoryGameModel/Prefabs_MemoryGameModel"
            },
            {                
                GameObjectType.Card, "Prefabs/Card/Prefabs_Card"
            },
            {
                GameObjectType.Difficulty, "Prefabs/Difficulty/Prefabs_Difficulty"
            },
            {
                GameObjectType.HPManager, "Prefabs/HPManager/Prefabs_HPManager"
            },
            {
                GameObjectType.ProgressBar, "Prefabs/ProgressBar/Prefabs_ProgressBar"
            },
            {
                GameObjectType.ProgressBarEffect, "Prefabs/Effects/Prefabs_ProgressBarEffect"
            },
            {
                GameObjectType.CardShufflingController, "Prefabs/Controllers/Prefabs_CardShufflingController" 
            },
            {
                GameObjectType.GameScenarioExecutor, "Prefabs/Controllers/Prefabs_GameScenarioExecutor"
            }
            ,
            {
                GameObjectType.HPprefab, "Prefabs/HP/Prefabs_HP"
            }

        };

        #endregion
    }
}
