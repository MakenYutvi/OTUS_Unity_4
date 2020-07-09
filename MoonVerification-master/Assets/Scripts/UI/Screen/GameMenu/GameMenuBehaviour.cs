using MiniGames.Memory;
using UnityEngine;
using UnityEngine.UI;


namespace Core
{
    public sealed class GameMenuBehaviour : BaseUi
    {
        #region Private Data

        private MemoryGameController _controller;
        
        
        #endregion
        
        
        #region Fields

        public Button GameMenuHelpButton;
        

        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            _controller = GameObject.FindGameObjectWithTag("MemoryGameController").GetComponent<MemoryGameController>();
            
        }

        private void OnEnable()
        {
            GameMenuHelpButton.onClick.AddListener(HighlightCards);
            
        }

        private void OnDisable()
        {
            GameMenuHelpButton.onClick.RemoveListener(HighlightCards);
            
        }
        #endregion
                      

        #region Methods

        public override void Show()
        {
            gameObject.SetActive(true);
            ShowUI.Invoke();
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            HideUI.Invoke();
        }

        private void HighlightCards()
        {
            _controller.CardDealerController.HighlightMatcheCards();
        }
        
        #endregion
    }
}
