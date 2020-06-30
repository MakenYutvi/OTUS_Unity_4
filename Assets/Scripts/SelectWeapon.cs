using Photon.Pun;
using UnityEngine;

public sealed class SelectWeapon : MonoBehaviourPunCallbacks
{
    public Gun Gun;
    public GrenadeLauncher GrenadeLauncher;
    public Mortar Mortar;
    public PlayerAnimation PlayerAnimation;
    public FireBallGenerator _fireBallGenerator;
    bool isBot;

    private void Start()
    {
        isBot = GetComponent<BotUtility>() != null;
        
        Gun.SetActive(isBot);
        if (!photonView.IsMine)
        {
            Destroy(this);
            return;
        }

        Gun.SetPlayerAnimation(PlayerAnimation);
        if(!isBot)
        {
            WeaponsSetActiveFalse();
            GrenadeLauncher.SetPlayerAnimation(PlayerAnimation);
            Mortar.SetPlayerAnimation(PlayerAnimation);
            _fireBallGenerator.SetPlayerAnimation(PlayerAnimation);
        }
    }

    private void Update()
    {
        if (isBot)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponsSetActiveFalse();
            Gun.SetActive(true);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponsSetActiveFalse();
            GrenadeLauncher.SetActive(true);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponsSetActiveFalse();
            Mortar.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WeaponsSetActiveFalse();
            _fireBallGenerator.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            WeaponsSetActiveFalse();
        }
    }

    public void WeaponsSetActiveFalse()
    {
        Gun.SetActive(false);
        GrenadeLauncher.SetActive(false);
        Mortar.SetActive(false);
        _fireBallGenerator.SetActive(false);
    }
}
