using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AOEDamge : MonoBehaviour
{
    #region PrivateData
    private bool _IsTarget;
    private Vector3 _origin;
    private Vector3 _direction;
    #endregion

    #region Methods
    private void AOEDamage( GameObject _gameObject,float _radius, LayerMask _layerMask, float _damage)
    {

        _origin = _gameObject.transform.position;
        _direction = _gameObject.transform.forward;
        RaycastHit[] _hits = Physics.SphereCastAll(_origin, _radius, _direction, _radius, _layerMask);
        _gameObject.GetComponentInChildren<ParticleSystem>().Play();
        for (var i = 0; i < _hits.Length; i++)
        {
            _IsTarget = _hits[i].collider.GetComponent<PlayerAnimation>() != null;
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

