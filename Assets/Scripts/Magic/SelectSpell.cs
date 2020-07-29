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
        _spell.SetSOData(GetFireBallSO());
        if (!photonView.IsMine)
        {
            Destroy(this);
            return;
        }

        _spell.SetPlayerAnimation(PlayerAnimation);
        if (isBot)
        {
            float i = Random.Range(-1.0f,1.0f) ;
            if ( i < 0.0f)
            {
                _spell.SetSOData(GetLightningSO());
            }
            else
            {
                _spell.SetSOData(GetFireBallSO());
            }
            
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
