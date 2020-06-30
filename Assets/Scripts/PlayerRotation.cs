using UnityEngine;
using UnityEngine.AI;

public class PlayerRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerAnimation _playerAnimation;
    private NavMeshAgent _agent;
    private float _angle;
    private float _angle_prev;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _angle_prev = _agent.transform.rotation.w;
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent == null)
            return;
        _angle = _agent.transform.rotation.w;
 
        if ((_angle - _angle_prev)>0.1)
        {
            //Debug.Log("left");
            _playerAnimation.OnRotateLeftBegin();

            _playerAnimation.OnRotateRightEnd();
            _angle_prev = _angle;
        }
        else if ((_angle - _angle_prev) < -0.1)
        {
           // Debug.Log("right");
            _playerAnimation.OnRotateRightBegin();

            _playerAnimation.OnRotateLeftEnd();
            _angle_prev = _angle;
        }
        
    }
}
