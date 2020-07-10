using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightningSpell : MonoBehaviour
{
    #region PrivateData
    private float _Particleduration = 1;
    [SerializeField] private SpellSO _spellSO;
    #endregion

    #region UnityMethods
    void Start()
    {
        AOEDamge.AOEDamage(gameObject, _spellSO.RadiusOfExplosion, _spellSO.LayerMask, _spellSO.BaseDamege);
        StartCoroutine(_Wait());
    }
    #endregion

    #region Methods

    IEnumerator _Wait()
    {
        yield return new WaitForSecondsRealtime(_Particleduration);
        Destroy(gameObject);
    }
    #endregion
}
