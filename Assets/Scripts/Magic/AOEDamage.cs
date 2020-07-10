using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public static class AOEDamge 
{

    #region Methods
    public static void AOEDamage( GameObject _gameObject,float _radius, LayerMask _layerMask, float _damage)
    {

        Vector3 _origin = _gameObject.transform.position;
        Vector3 _direction = _gameObject.transform.forward;
        RaycastHit[] _hits = Physics.SphereCastAll(_origin, _radius, _direction, _radius, _layerMask);
      //  _gameObject.GetComponentInChildren<ParticleSystem>().Play();
        for (var i = 0; i < _hits.Length; i++)
        {
            bool _IsTarget = _hits[i].collider.GetComponent<PlayerAnimation>() != null;
            if (_IsTarget)
            {

                if (_hits[i].collider.TryGetComponent<PhotonView>(out PhotonView view))
                {
                    view.RPC("GetDamageRPC", RpcTarget.All, _damage);
                }

            }
        }
       
    }
    #endregion
}

