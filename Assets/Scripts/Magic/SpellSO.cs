using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpellSO")]
public class SpellSO : ScriptableObject
{
    #region Private Data
    [SerializeField] private float _baseDamage;
    [SerializeField] private float _castTime = 1f;
    [SerializeField] private float _cooldown = 1f;
    [SerializeField] private float _radiusOfExplosion;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _manaCost = 10;
    [SerializeField] private GameObject _prefabOfSpell;
    [SerializeField] private float _maxDistanceToTarget = 20.0f;
    [SerializeField] private bool _spellFromCaster;
    #endregion

    #region Properties
    public float BaseDamege
    {
        get
        {
            return _baseDamage;
        }

    }

    public float CastTime
    {
        get
        {
            return _castTime;
        }
    }
    public float Cooldown
    {
        get
        {
            return _cooldown;
        }
    }
    public float RadiusOfExplosion
    {
        get
        {
            return _radiusOfExplosion;
        }
    }
    public LayerMask LayerMask
    {
        get
        {
            return _layerMask;
        }
    }
    public float ProjectileSpeed
    {
        get
        {
            return _projectileSpeed;
        }
    }

    public float ManaCost
    {
        get
        {
            return _manaCost;
        }
    }
    public GameObject PrefabOfSpell
    {
        get
        {
           // if (_prefabOfSpell == null)
          //  {
         //       _prefabOfSpell = Resources.Load("Spells/FireBallPrototype.prefab") as GameObject;
         //   }
            return _prefabOfSpell;
        }
    }
    public float MaxDistanceToTarget
    {
        get
        {
            return _maxDistanceToTarget;
        }
    }
    public bool SpellFromCaster
    {
        get
        {
            return _spellFromCaster;
        }
    }
    #endregion
}



