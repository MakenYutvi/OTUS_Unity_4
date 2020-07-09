using Moon.Asyncs;
using UnityEngine;


public class CameraAnimationController : MonoBehaviour
{

    #region Private Data
    private float _animationSpeed = 1.0f;
    private Camera _mainCamera;
    private GameObject _rootRewardCamera;
    private Animator _cameraAnimator;
    #endregion


    #region Unity Methods
    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraAnimator = _mainCamera.GetComponentInParent<Animator>();
        _rootRewardCamera = GameObject.FindGameObjectWithTag("RewardCamera");
        _rootRewardCamera.SetActive(false);
        
    }
    #endregion


    #region MoonAsyncs Methods
    public AsyncState StartCutScene()
    {
        return Planner.Chain()
                .AddAction(() => _cameraAnimator.SetTrigger("StartGameCameraTrigger"))
                .AddAwait(IsRoundStart)
            ;
    }

    public AsyncState RewardCutScene()
    {
        return Planner.Chain()
                .AddAction(() => _cameraAnimator.SetTrigger("RewardGameCameraTrigger"))
                .AddAwait(IsRoundEnd)
            ;
    }
        
    private void IsRoundStart(AsyncStateInfo state)
    {
        state.IsComplete = _cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("StartGameCamera")
            && _cameraAnimator.GetCurrentAnimatorStateInfo(0).speed >= _animationSpeed;
    }
    
    private void IsRoundEnd(AsyncStateInfo state)
    {
        state.IsComplete = _cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("RewardGameCamera")
            && _cameraAnimator.GetCurrentAnimatorStateInfo(0).speed <= _animationSpeed;
    }
    #endregion

}
