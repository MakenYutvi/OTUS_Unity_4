using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spell : MonoBehaviour
{

    #region SO Data
    private float _baseDamage;
    private float _castTime = 1f;
    private float _cooldown = 1f;
    private float _radiusOfExplosion;
    private LayerMask _layerMask;
    private float _projectileSpeed;
    private float _manaCost = 10;
    private GameObject _prefabOfSpell;
    private float _maxDistanceToTarget = 20.0f;
    private bool _spellFromCaster;
    #endregion

    private PlayerAnimation _playerAnimation;
    private bool _isReady = true;
    private Mana _mana;
    private bool _isBot;
    private GameObject _InstantiateGameObject;
    private Camera _mainCamera;
    private Vector2 _center;
    private SpellSO _SpellSO;
    [HideInInspector] public float CastDelay;
    [HideInInspector] public float CooldownDelay;

    #region UnityMethods
    void Start()
    {
        _mana = GetComponentInParent<Mana>();
        _isBot = GetComponent<BotUtility>() != null;
        _mainCamera = Camera.main;
        _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);

    }

    void Update()
    {
        {
            if (_isBot)
                return;
            if (Input.GetMouseButton(0) && HasEnoughMana() && _isReady)
            {
                SpellCast(_mainCamera.ScreenPointToRay(_center));
                _isReady = false;
                Invoke(nameof(ReadySpellCast), _cooldown);
            }

            if (Input.GetMouseButtonDown(0) && HasEnoughMana() && gameObject.activeInHierarchy)
            {
                BeginAnimateSpellCast();
                _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
            }
            if (Input.GetMouseButtonUp(0))
            {
                EndAnimateSpellCast();
            }
        }
    }
    #endregion

    #region Methods
    public void SetSOData(SpellSO _SO)
    {
    _SpellSO = _SO;
    _baseDamage = _SpellSO.BaseDamege;
    _castTime = _SpellSO.CastTime;
    _cooldown = _SpellSO.Cooldown;
    _radiusOfExplosion = _SpellSO.RadiusOfExplosion;
    _layerMask = _SpellSO.LayerMask;
    _projectileSpeed= _SpellSO.ProjectileSpeed;
    _manaCost = _SpellSO.ManaCost;
    _prefabOfSpell = _SpellSO.PrefabOfSpell;
    _maxDistanceToTarget = _SpellSO.MaxDistanceToTarget;
    _spellFromCaster = _SpellSO.SpellFromCaster;
}
    public void SetPlayerAnimation(PlayerAnimation playerAnimation)
    {
        _playerAnimation = playerAnimation;
    }

    public bool HasEnoughMana()
    {
        return _mana.Count > 0;
    }

    public void BeginAnimateSpellCast()
    {
        _playerAnimation.OnFireEnable();
    }

    public void EndAnimateSpellCast()
    {
        _playerAnimation.OnFireDisable();
    }

    public void SpellCast(Ray ray)
    {
        _mana.Count -= _manaCost;
        _InstantiateGameObject = PhotonNetwork.Instantiate(_prefabOfSpell.name, gameObject.transform.position, gameObject.transform.rotation,0);
        if (_spellFromCaster)
        {
            _InstantiateGameObject.transform.position = gameObject.transform.position;
        }
        else
        {
            _InstantiateGameObject.transform.position = ray.GetPoint(_maxDistanceToTarget); ;
            foreach (var hit in Physics.RaycastAll(ray, _maxDistanceToTarget))
            {
                if (hit.collider)
                {
                    _InstantiateGameObject.transform.position = hit.point;
                    break;
                }
            }
        }
       

    }
    public void ReadySpellCast()
    {
        _isReady = true;
    }
    public void NotReadySpellCast()
    {
        _isReady = false;
    }
    #endregion

}
