using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallGenerator : MonoBehaviour
{
    public int ManaCost = 10;
    //   private ParticleSystem _particle;
    private PlayerAnimation _playerAnimation;
    //private Camera _mainCamera;
   // private Vector2 _center;
    private readonly float _dedicateDistance = 20.0f;
   // private int _layerMask;
    private bool _isReady = true;
    private float _rechergeTime = 0.2f;
    Mana _mana;
    //FireBall _fireball;
    bool isBot;
    public GameObject _PrefabFireBall;
    GameObject _fireBall;


    public void Start()
    {
        _mana = GetComponentInParent<Mana>();
        isBot = GetComponent<BotUtility>() != null;
        // _particle = GetComponentInChildren<ParticleSystem>();
       // _mainCamera = Camera.main;
      //  _layerMask = 0;//1 << 8;
       // _layerMask = ~_layerMask;
      //  _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
        //_fireball = GetComponentInChildren<FireBall>();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetPlayerAnimation(PlayerAnimation playerAnimation)
    {
        _playerAnimation = playerAnimation;
    }

    public bool HasEnoughMana()
    {
        return _mana.Count > 0;
    }

    public void BeginAnimateShoot()
    {
        _playerAnimation.OnFireEnable();
    }

    public void EndAnimateShoot()
    {
        _playerAnimation.OnFireDisable();
    }

    public void Shoot()
    {
        _mana.Count -= ManaCost;
        _fireBall = Instantiate(_PrefabFireBall);
        _fireBall.transform.position = gameObject.transform.position;
     //   _rocket.LaunchRocket(ray.GetPoint(20));

    }

    private void Update()
    {
        if (isBot)
            return;
        if (Input.GetMouseButton(0) && HasEnoughMana() && _isReady)
        {

                Shoot();
                _isReady = false;
                Invoke(nameof(ReadyShoot), _rechergeTime);

        }

        if (Input.GetMouseButtonDown(0) && HasEnoughMana() && gameObject.activeInHierarchy)
        {
            //_center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
            BeginAnimateShoot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndAnimateShoot();
        }
    }

    public void ReadyShoot()
    {
        _isReady = true;
    }
    public void NotReadyShoot()
    {
        _isReady = false;
    }
}
