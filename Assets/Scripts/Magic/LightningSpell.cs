using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightningSpell : MonoBehaviour
{
    // Start is called before the first frame update
    public float _radius;
    public LayerMask _layerMask;
    public float _damage;
    private Vector3 _origin;
    private Vector3 _direcrion;
    private bool Istarget;
    private float _Particleduration = 1;


    void Start()
    {
        explosionBegin();
    }


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerAnimation>() != null)
        {
            explosionBegin();
        }

    }

    private void explosionBegin()
    {

        _origin = gameObject.transform.position;
        _direcrion = gameObject.transform.forward;
        RaycastHit[] _hits = Physics.SphereCastAll(_origin, _radius, _direcrion, _radius, _layerMask);
        for (var i = 0; i < _hits.Length; i++)
        {
            Istarget = _hits[i].collider.GetComponent<PlayerAnimation>() != null;
            if (Istarget)
            {

                Debug.Log(_hits[i].collider.gameObject.name);
                if (_hits[i].collider.TryGetComponent<PhotonView>(out PhotonView view))
                {
                    view.RPC("GetDamageRPC", RpcTarget.All, _damage);
                }

            }
        }

        StartCoroutine(_Wait());
    }

    IEnumerator _Wait()
    {

        yield return new WaitForSecondsRealtime(_Particleduration);
        Destroy(gameObject);

    }

}
