using Core;
using MiniGames.Memory;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RestartMenuBehaviour : BaseUi
{
    #region Private Data

    private MemoryGameController _controller;


    #endregion


    #region Fields

    public Button RestartButton;

    #endregion


    #region UnityMethods
    
    private void OnEnable()
    {
        RestartButton.onClick.AddListener(RestartLevel);

    }

    private void OnDisable()
    {
        RestartButton.onClick.RemoveListener(RestartLevel);

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

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    #endregion
}
