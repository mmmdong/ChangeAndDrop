using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpd;
    [SerializeField] private float _pushForce;
    [SerializeField] private float _boxMaxPosX;    //박스의 최대 행동범위
    [Space]
    [SerializeField] private TouchSlider _touchSlider;
    [SerializeField] private GameObject _roof;

    public int _color;

    private bool _isPointerDown;
    private bool _isGameStart;
    private Vector3 _ballPos;

    private void Start()
    {

        //이벤트 리스너에 액션 등록
        _touchSlider._onPointerDownEvent += OnPointerDown;
        _touchSlider._onPointerDragEvent += OnPointerDrag;
        _touchSlider._onPointerUpEvent += OnPointerUp;
    }

    private void Update()
    {
        if (_isPointerDown)
            transform.position = Vector3.Lerp(transform.position, _ballPos, _moveSpd * Time.deltaTime);
    }

    private void OnDestroy()
    {
        //액션 할당 해제
        _touchSlider._onPointerDownEvent -= OnPointerDown;
        _touchSlider._onPointerDragEvent -= OnPointerDrag;
        _touchSlider._onPointerUpEvent -= OnPointerUp;
    }

    private void OnPointerDown()
    {
        _isPointerDown = true;
        if (_isGameStart)
        {
            if (BallObjPool.instance._isBlue.Value)
                BallObjPool.instance._isBlue.Value = false;
            else
                BallObjPool.instance._isBlue.Value = true;
        }
    }
    private void OnPointerDrag(float xMoveValue)
    {
        if (_isPointerDown && !_isGameStart)
        {
            _ballPos = transform.position;
            _ballPos.x = xMoveValue * _boxMaxPosX;
        }
    }

    private void OnPointerUp()
    {
        if (_isPointerDown && !_isGameStart)
        {
            _isPointerDown = false;
            _isGameStart = true;
        }
        StartCoroutine(CoRotateBox());
    }

    private IEnumerator CoRotateBox()
    {
        while (transform.rotation.x < 0.9999f)
        {
            transform.Rotate(Vector3.right, Time.deltaTime * 200f);
            yield return null;
        }
        _isGameStart = true;
        yield return new WaitForSeconds(0.1f);
        _roof.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameManager.instance._isGaming.Value = true;
    }
}
