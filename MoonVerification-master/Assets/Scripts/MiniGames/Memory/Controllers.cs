using Core;


public class Controllers : IInitialization
{
    #region Fields
    public IInitialization[] _initializations;
    #endregion


    #region Class LifeCycle
    public Controllers()
    {
        _initializations = new IInitialization[2];
        _initializations[0] = new CardDealerController();
        _initializations[1] = new DifficultyController();
    }
    #endregion


    #region Methods
    public void Initialization()
    {
        for (var i = 0; i < _initializations.Length; i++)
        {
            var initialization = _initializations[i];
            initialization.Initialization();
        }
    }
    #endregion
}
