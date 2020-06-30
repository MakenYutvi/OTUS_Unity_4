using Photon.Pun;
using UnityEngine;

public sealed class Mortar : MonoBehaviour
{
   // public float Damage = 2.0f;
 //   private ParticleSystem _particle;
    private PlayerAnimation _playerAnimation;
    private Camera _mainCamera;
    private Vector2 _center;
    private readonly float _dedicateDistance = 20.0f;
    private int _layerMask;
    private bool _isReady = true;
    private float _rechergeTime = 0.2f;
    GunAmmo ammo;
    Mine _mine;
    bool isBot;

    public void Start()
    {
        ammo = GetComponentInParent<GunAmmo>();
        isBot = GetComponent<BotUtility>() != null;
       // _particle = GetComponentInChildren<ParticleSystem>();
        _mainCamera = Camera.main;
        _layerMask = 0;//1 << 8;
        _layerMask = ~_layerMask;
        _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
        _mine = GetComponentInChildren<Mine>();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetPlayerAnimation(PlayerAnimation playerAnimation)
    {
        _playerAnimation = playerAnimation;
    }

    public bool HasEnoughAmmo()
    {
        return ammo.Count > 0;
    }

    public void BeginAnimateShoot()
    {
        _playerAnimation.OnFireEnable();
    }

    public void EndAnimateShoot()
    {
        _playerAnimation.OnFireDisable();
    }

    public void Shoot(Ray ray)
    {
        --ammo.Count;
        _mine.LaunchMine(ray.GetPoint(20));
    }

    private void Update()
    {
        if (isBot)
            return;
        if (Input.GetMouseButton(0) && HasEnoughAmmo() && !isBot)
        {
            
            if (_isReady)
            {
                Debug.Log(ammo.Count);
                Shoot(_mainCamera.ScreenPointToRay(_center));
                _isReady = false;
                Invoke(nameof(ReadyShoot), _rechergeTime);
            }
        }

        if (Input.GetMouseButtonDown(0) && HasEnoughAmmo() && gameObject.activeInHierarchy)
        {
            _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
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
