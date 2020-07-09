using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightningGenerator : MonoBehaviour
{
    public float ManaCost = 1f;
    private int _layerMask;
    private PlayerAnimation _playerAnimation;
    private readonly float _dedicateDistance = 20.0f;
    private bool _isReady = true;
    private float _rechergeTime = 0.2f;
    Mana _mana;
    bool isBot;
    public GameObject _PrefabLlightning;
    GameObject _Llightning;
    private Camera _mainCamera;
    private Vector2 _center;


    public void Start()
    {
        _mana = GetComponentInParent<Mana>();
        isBot = GetComponent<BotUtility>() != null;
        _mainCamera = Camera.main;
        _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
        _layerMask = 0;//1 << 8;
        _layerMask = ~_layerMask;
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

    public void Shoot(Ray ray)
    {
        _mana.Count -= ManaCost;
        _Llightning = Instantiate(_PrefabLlightning);
        foreach (var hit in Physics.RaycastAll(ray, _dedicateDistance, _layerMask))
        {
            if (hit.collider)
            {
                _Llightning.transform.position = hit.point;
                break;
            }
        }

    }

    private void Update()
    {
        if (isBot)
            return;
        if (Input.GetMouseButton(0) && HasEnoughMana() && _isReady)
        {
            Shoot(_mainCamera.ScreenPointToRay(_center));
            _isReady = false;
            Invoke(nameof(ReadyShoot), _rechergeTime);
        }

        if (Input.GetMouseButtonDown(0) && HasEnoughMana() && gameObject.activeInHierarchy)
        {
            BeginAnimateShoot();
            _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
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

