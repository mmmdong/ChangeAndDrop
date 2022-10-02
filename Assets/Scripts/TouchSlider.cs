using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityAction _onPointerDownEvent;
    public UnityAction<float> _onPointerDragEvent;
    public UnityAction _onPointerUpEvent;

    private Slider _uiSlider;

    private void Awake()
    {
        _uiSlider = GetComponent<Slider>();
        _uiSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //���콺 Ŭ��(�ٿ�) �̺�Ʈ �ݹ�
        //���콺 �����͸� Ŭ���� �� �巡�� �ݹ鵵 �Բ� ����
        _onPointerDownEvent?.Invoke();
        _onPointerDragEvent?.Invoke(_uiSlider.value);
    }
    public void OnSliderValueChanged(float value)
    {
        //�巡�� �̺�Ʈ �ݹ�
        _onPointerDragEvent?.Invoke(value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //���콺 Ŭ��(��) �̺�Ʈ �ݹ�
        _onPointerUpEvent?.Invoke();
        //���콺�� ���� �����̴��� ����� ���ƿ�
        _uiSlider.value = 0.0f;
    }

    private void OnDestroy()
    {
        _uiSlider.onValueChanged.RemoveAllListeners();
    }
}
