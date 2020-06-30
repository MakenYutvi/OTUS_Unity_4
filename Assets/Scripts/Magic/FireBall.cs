using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireBall : MonoBehaviour
{
    // Start is called before the first frame update
    public float _radius;
    public LayerMask _layerMask;
    public float _damage;
    public float speed;
    private Transform start;
    private Vector3 _end;
    private float current;  // от 0.0 до 1.0
    private float dir;
    private Vector3 _origin;
    private Vector3 _direcrion;
    private bool Istarget;
    private ParticleSystem _particle;
    private float _Particleduration;
    private Camera _mainCamera;
    private Vector2 _center;
   

    void Start()
    {
        current = 0.0f;
        dir = 1.0f;
        _particle = GetComponentInChildren<ParticleSystem>();
        _Particleduration = _particle.main.duration;
        start = gameObject.transform;
        _mainCamera = Camera.main;
        _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
    _end = _mainCamera.ScreenPointToRay(_center).GetPoint(29);
    }

    void Update()
    {
        current += dir * speed * Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(start.position, _end, current);
            if (current > 1.0)
            {
                explosionBegin();
                Debug.Log("End");
                transform.position = start.position;
                current = 0.0f;
            }   
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
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
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
