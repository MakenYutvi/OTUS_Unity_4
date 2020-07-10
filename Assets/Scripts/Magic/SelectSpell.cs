using Photon.Pun;
using UnityEngine;

public sealed class SelectSpell : MonoBehaviourPunCallbacks
{
    #region Data
    public PlayerAnimation PlayerAnimation;
    private Spell _spell;
    bool isBot;
    private SpellSO _SpellSO;
    #endregion

    #region UnityMethods
    private void Start()
    {
        _spell = GetComponentInChildren<Spell>();
        isBot = GetComponent<BotUtility>() != null;

      //  Gun.SetActive(isBot);
        if (!photonView.IsMine)
        {
            Destroy(this);
            return;
        }

       // Gun.SetPlayerAnimation(PlayerAnimation);
        if (!isBot)
        {
            WeaponsSetActiveFalse();
            _spell.SetPlayerAnimation(PlayerAnimation);
        }
    }

    private void Update()
    {
        if (isBot)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _spell.SetSOData(GetFireBallSO());

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _spell.SetSOData(GetLightningSO());

        }
     
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }
    #endregion

    #region Methods
    public void WeaponsSetActiveFalse()
    {
  
    }


    private SpellSO GetFireBallSO()
    {
           SpellSO _SpellSO =
                Resources.Load<SpellSO>
                    (SpellSOPaths.Spells[SpellType.FireBall]);
        

        return _SpellSO;
    }

    private SpellSO GetLightningSO()
    {
         _SpellSO =
             Resources.Load<SpellSO>
                 (SpellSOPaths.Spells[SpellType.Lightning]);


        return _SpellSO;
    }

    #endregion

}
