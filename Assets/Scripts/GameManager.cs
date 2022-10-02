using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject[] _camFloor;
    [SerializeField] private Rigidbody _camRig;
    [SerializeField] Vector3 _camOriPos;

    public Queue<GameObject> _camFloorQueue = new Queue<GameObject>();
    public BoolReactiveProperty _isGaming = new BoolReactiveProperty(false);

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _isGaming.TakeUntilDestroy(this).Subscribe(x =>
        {
            _camRig.useGravity = x;
        });
        for(var i = 0; i < _camFloor.Length; i++)
        {
            _camFloorQueue.Enqueue(_camFloor[i]);
        }
        _camOriPos = _camRig.gameObject.transform.position;
    }
}
