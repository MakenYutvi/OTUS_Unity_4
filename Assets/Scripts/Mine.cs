using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mine : MonoBehaviour
{
    // Start is called before the first frame update
    public float _radius;
    public LayerMask _layerMask;
    public float _damage;
    private Transform start;
    private GameObject _Mine;
    private Vector3 _end;
    public float _AddForce;
    private float current;  // от 0.0 до 1.0
    private float dir;
    bool Launched = false;
    private Vector3 _origin;
    private Vector3 _direcrion;
    private Mortar _mortar;
    private bool Istarget;
    private ParticleSystem _particle;
    private float _Particleduration;
    Rigidbody _rigidBody;
    public float _liveTime;


    void Start()
    {
        _mortar = GetComponentInParent<Mortar>();
        current = 0.0f;
        dir = 1.0f;
        _particle = GetComponentInChildren<ParticleSystem>();
        _Particleduration = _particle.main.duration;
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_Mine == null)
        {
            return;
        }
        if (Launched)
        {
            StartCoroutine(_WaitExplosion());
        }

    }
    public void LaunchMine(Vector3 _cursor)
    {
        if (Launched)
        {
            Debug.Log("not ready");
            return;
        }

        Debug.Log("fire");
        start = gameObject.gameObject.transform;
        _end = _cursor;
        Launched = true;
        _mortar.NotReadyShoot();
        _Mine = Instantiate(gameObject);
        _Mine.transform.position = gameObject.transform.position;
        _Mine.gameObject.GetComponent<Rigidbody>().AddForce((_cursor-start.position).normalized*_AddForce, ForceMode.Impulse);
        //_Mine.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward* _AddForce, ForceMode.Impulse);
        _Mine.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //Launched = false;

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerAnimation>() != null)
        {
            Debug.Log(collision.gameObject.name);
            // if (!collision.gameObject.CompareTag("Player"))
            explosionBegin();
            StopCoroutine(_WaitExplosion());
        }

    }

    private void explosionBegin()
    {
        if (_Mine == null)
        {
            return;
        }
        _origin = _Mine.transform.position;
        _direcrion = _Mine.transform.forward;
        RaycastHit[] _hits = Physics.SphereCastAll(_origin, _radius, _direcrion, _radius, _layerMask);
        Debug.Log("_hits:");
        Debug.Log(_hits.Length);
        //_particle.Play();
        _Mine.GetComponentInChildren<ParticleSystem>().Play();
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
        _mortar.ReadyShoot();
        StartCoroutine(_WaitParticle());
        //
    }

    IEnumerator _WaitParticle()
    {

        yield return new WaitForSecondsRealtime(_Particleduration);
        Launched = false;
        _mortar.ReadyShoot();
        Destroy(_Mine);

    }

    IEnumerator _WaitExplosion()
    {
       
            yield return new WaitForSecondsRealtime(_liveTime);
        
        Debug.Log("endlivetime");
        explosionBegin();

    }

}
