using Core;
using Core.Customs;
using DG.Tweening;
using Moon.Asyncs;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    #region  PrivateData
    private GameObject _effectPrefab;
    private Image[] _images;
    
    [SerializeField] private float fillAmountDuration = 2f;

    private ParticleSystem effectInstance;
    #endregion


    #region Unity Methods

    private void OnEnable()
    {
        ToggleBarImages(false);
    }
    
    private void Awake()
    {
        var effect = CustomResources.Load<GameObject>
                        (AssetsPathGameObject.GameObjects[GameObjectType.ProgressBarEffect]);

        _effectPrefab = Instantiate(effect);
        _images = GetComponentsInChildren<Image>();
        transform.localScale = Vector3.zero;
    }
    #endregion


    #region MonnAsync Methods
    public AsyncState Show()
    {
        return Planner.Chain()
                    .AddAction(ShowParticleEffect)
                    .AddAction(ToggleBarImages, true)
                    .AddTween(ShowEffect)
                    .AddAwait((AsyncStateInfo state) => state.IsComplete = effectInstance == null)
                    
                ;
    }

    public AsyncState Close()
    {
        return Planner.Chain()
                    .AddAction(ShowParticleEffect)
                    .AddTween(CloseEffect)
                    .AddAction(ToggleBarImages, false)
                    .AddAwait((AsyncStateInfo state) => state.IsComplete = effectInstance == null)
                    
                ;
    }
    #endregion


    #region Methods
    public void ShowParticleEffect()
    {
        effectInstance = Instantiate(_effectPrefab, transform).GetComponent<ParticleSystem>();
        var totalDuration = effectInstance.main.duration + effectInstance.main.startLifetime.constant;
        Destroy(effectInstance.gameObject, totalDuration);
    }

    private void ToggleBarImages(bool isEnable)
    {
        _images[0].enabled = isEnable;
        _images[1].enabled = isEnable;
    }
    #endregion


    #region DoTween Methods
    public Tween SetCurrentValue(float value)
    {
        return _images[1]
                .DOFillAmount(_images[1].fillAmount + value, fillAmountDuration)
            ;
    }
    public Tween ShowEffect()
    {
        return transform.DOScale(Vector3.one, 1f);
    }

    public Tween CloseEffect()
    {
        return transform.DOScale(Vector3.zero, 1f);
    }
    #endregion
}