using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireBall : MonoBehaviour
{
    #region PrivateData
    [SerializeField] private SpellSO _spellSO;
    private Transform _start;
    private Vector3 _end;
    private float _current;  // от 0.0 до 1.0
    private float _dir;
    private ParticleSystem _particle;
    private float _Particleduration;
    private Camera _mainCamera;
    private Vector2 _center;
    #endregion

    #region UnityMethods
    void Start()
    {
        _current = 0.0f;
        _dir = 1.0f;
        _particle = GetComponentInChildren<ParticleSystem>();
        _Particleduration = _particle.main.duration;
        _start = gameObject.transform;
        _mainCamera = Camera.main;
        _center.Set(Screen.width / 2.0f, Screen.height / 2.0f);
        _end = _mainCamera.ScreenPointToRay(_center).GetPoint(29);
    }

    void Update()
    {
        _current += _dir * _spellSO.ProjectileSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(_start.position, _end, _current);
            if (_current > 1.0)
            {
                explosionBegin1();
                transform.position = _start.position;
                _current = 0.0f;
            }   
    }
    #endregion

    #region Methods
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerAnimation>() != null)
        {
            explosionBegin1();
        }

    }
    public void SetData(SpellSO _SO)
    {

    }
    private void explosionBegin1()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
        AOEDamge.AOEDamage(gameObject, _spellSO.RadiusOfExplosion, _spellSO.LayerMask, _spellSO.BaseDamege);
        StartCoroutine(_Wait());
    }


    IEnumerator _Wait()
    {
        yield return new WaitForSecondsRealtime(_Particleduration);
        Destroy(gameObject);
    }
    #endregion
}
