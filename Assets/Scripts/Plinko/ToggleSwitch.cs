using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider setup")]
    [SerializeField, Range(0, 1f)]
    protected float sliderValue;
    public bool CurrentValue { get; private set; }

    private bool _previousValue;
    private Slider _slider;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.25f;
    [SerializeField]
    private AnimationCurve slideEase =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine _animateSliderCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    protected Action transitionEffect;

    private void SetupSliderComponent()
    {
        _slider = GetComponent<Slider>();

        if (!_slider)
        {
            Debug.Log("No slider found!", this);
            return;
        }

        _slider.interactable = false;
        var sliderColors = _slider.colors;
        sliderColors.disabledColor = Color.white;
        _slider.colors = sliderColors;
        _slider.transition = Selectable.Transition.None;
    }
    private void Awake()
    {
        SetupSliderComponent();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        SetStateAndStartAnimation(!CurrentValue);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        _previousValue = CurrentValue;
        CurrentValue = state;

        if (_previousValue != CurrentValue)
        {
            if (CurrentValue)
            {
                onToggleOn?.Invoke();
                Observer.Instance.Notify(EventName.AutoPlay, true);
            }
            else
            {
                onToggleOff?.Invoke();
                Observer.Instance.Notify(EventName.AutoPlay, false);
            }
        }

        if (_animateSliderCoroutine != null)
            StopCoroutine(_animateSliderCoroutine);

        _animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }


    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                transitionEffect?.Invoke();

                yield return null;
            }
        }

        _slider.value = endValue;
    }
}
