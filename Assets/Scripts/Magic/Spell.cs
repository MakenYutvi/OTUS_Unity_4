using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spell : MonoBehaviour
{

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
                Invoke(nameof(ReadySpellCast), _SpellSO.Cooldown);
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
        _mana.Count -= _SpellSO.ManaCost;
        _InstantiateGameObject = PhotonNetwork.Instantiate(_SpellSO.PrefabOfSpell.name, gameObject.transform.position, gameObject.transform.rotation,0);
        //
        if (_SpellSO.SpellFromCaster)
        {
            _InstantiateGameObject.transform.position = gameObject.transform.position;
        }
        else
        {
            _InstantiateGameObject.transform.position = ray.GetPoint(_SpellSO.MaxDistanceToTarget); ;
            foreach (var hit in Physics.RaycastAll(ray, _SpellSO.MaxDistanceToTarget))
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
