using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallGenerator : MonoBehaviour
{
    public float ManaCost = 10f;
    private PlayerAnimation _playerAnimation;
    private readonly float _dedicateDistance = 20.0f;
    private bool _isReady = true;
    private float _rechergeTime = 0.2f;
    Mana _mana;
    bool isBot;
    public GameObject _PrefabFireBall;
    GameObject _fireBall;


    public void Start()
    {
        _mana = GetComponentInParent<Mana>();
        isBot = GetComponent<BotUtility>() != null;
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
