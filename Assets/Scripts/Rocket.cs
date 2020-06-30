using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    public float _radius;
    public LayerMask _layerMask;
    public float _damage;
    private Transform start;
    private GameObject _Rocket;
    private Vector3 _end;
    public float speed;
    private float current;  // от 0.0 до 1.0
    private float dir;
    bool Launched = false;
    private Vector3 _origin;
    private Vector3 _direcrion;
    private GrenadeLauncher _grenadeLauncher;
    private bool Istarget;
    private ParticleSystem _particle;
    private float _Particleduration;
    

    void Start()
    {
        _grenadeLauncher = GetComponentInParent<GrenadeLauncher>();
        current = 0.0f;
        dir = 1.0f;
        _particle = GetComponentInChildren<ParticleSystem>();
        _Particleduration = _particle.main.duration;
    }

    // Update is called once per frame
    void Update()
    {
        if(_Rocket == null)
        {
            return;
        }
        if (Launched)
        {
            current += dir * speed * Time.deltaTime;
            _Rocket.transform.position = Vector3.Lerp(start.position, _end, current);
            if (current > 1.0)
            {
                explosionBegin();        
                Debug.Log("End");
                transform.position = start.position;
                current = 0.0f;
            }
        }
        
    }
    public void LaunchRocket(Vector3 _cursor)
    {
        if(Launched)
        {
            Debug.Log("not ready");
            return;
        }
        Debug.Log("fire");
        start = gameObject.gameObject.transform;
        _end = _cursor;
        Launched = true;
        _grenadeLauncher.NotReadyShoot();
        _Rocket = Instantiate(gameObject);
        _Rocket.transform.position = gameObject.transform.position;
        //Launched = false;

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerAnimation>() != null)
        {
            Debug.Log(collision.gameObject.name);
            // if (!collision.gameObject.CompareTag("Player"))
            explosionBegin();
        }

    }

    private void explosionBegin()
    {
        if (_Rocket == null)
        {
            return;
        }
        _origin = _Rocket.transform.position;
        _direcrion = _Rocket.transform.forward;
        RaycastHit[] _hits = Physics.SphereCastAll(_origin, _radius, _direcrion, _radius, _layerMask);
        Debug.Log("_hits:");
        Debug.Log(_hits.Length);
        //_particle.Play();
        _Rocket.GetComponentInChildren<ParticleSystem>().Play();
        for (var i = 0; i < _hits.Length; i++)
        {
            Istarget = _hits[i].collider.GetComponent<PlayerAnimation>() != null;
            if (Istarget)
            {
                
                Debug.Log(_hits[i].collider.gameObject.name);
                if (_hits[i].collider.TryGetComponent<PhotonView>(out PhotonView view))
                {
                    view.RPC("GetDamageRPC", RpcTarget.All, _damage);
                    Debug.Log("damage");


                }
               
            }
        }
        
        Launched = false;
        _grenadeLauncher.ReadyShoot();
        StartCoroutine(_Wait());
        //
    }

    IEnumerator _Wait()
    {
        
        yield return new WaitForSecondsRealtime(_Particleduration);
        Launched = false;
        _grenadeLauncher.ReadyShoot();
        Destroy(_Rocket);
        
    }

}
