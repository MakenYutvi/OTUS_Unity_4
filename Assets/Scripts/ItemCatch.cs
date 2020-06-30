using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatch : MonoBehaviour
{
    public float repeat_time = 3f; 
    private float curr_time;
    Vector3 _current_pos;
    bool _catch = false;
    float _distance = 10.0f;
    void Start()
    {
        curr_time = repeat_time;
        _current_pos = gameObject.transform.position;
    }


    void Update()
    {
        if (_catch)
        {
            curr_time -= Time.deltaTime;
        if (curr_time <= 0)
            {
            curr_time = repeat_time;
                _current_pos.y += _distance;
                gameObject.transform.position = _current_pos;
                _catch = false;
            }
        }
    }
    public void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
             Debug.Log("Catch");
            _catch  = true;
            _current_pos.y -= _distance;
            gameObject.transform.position = _current_pos;
        }


    }
}
