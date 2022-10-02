using UniRx;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObjPool : MonoBehaviour
{
    //싱글턴 선언
    public static BallObjPool instance;

    private Queue<BallController> _ballsQueue = new Queue<BallController>();
    [SerializeField] private int _ballQueueCapacity = 20;
    [SerializeField] private bool _autoQueueGrow = true;

    [SerializeField] private GameObject _ballPref;

    private Vector3 _defaultSpawnPos;

    [SerializeField] Transform _cam;

    public BoolReactiveProperty _isBlue = new BoolReactiveProperty(true);

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _defaultSpawnPos = transform.position;
        //Pow(x,y) -> x의 y제곱

        InitializeBallsQueue();
        _isBlue.TakeUntilDestroy(this).Subscribe(x =>
        {
            var balls= GetComponentsInChildren<BallController>();
            var leng = balls.Length;
            for(var i = 0; i < leng; i++)
            {
                if (x == false)
                    balls[i].BallColor(1);
                else
                    balls[i].BallColor(0);
            }
        });
    }
    private void Start()
    {
        for(var i = 0; i < 5; i++)
        {
            Spawn(_defaultSpawnPos);
        }
    }

    private void InitializeBallsQueue()
    {
        for (var i = 0; i < _ballQueueCapacity; i++)
        {
            AddBallToQueue();
        }
    }

    private void AddBallToQueue()
    {
        var ball = Instantiate(_ballPref, _defaultSpawnPos, Quaternion.identity, transform).GetComponent<BallController>();

        ball.gameObject.SetActive(false);
        _ballsQueue.Enqueue(ball);
    }

    public BallController Spawn(Vector3 pos)
    {
        //풀에 남은 공이 없으면 공를 생성한다
        if(_ballsQueue.Count == 0)
        {
            if (_autoQueueGrow)
            {
                _ballQueueCapacity++;
                AddBallToQueue();
            }
            else
            {
                Debug.LogError("풀에서 사용할 수 있는 공들이 없읍니다.");
                return null;
            }
        }
     

        var ball = _ballsQueue.Dequeue();
        ball.transform.position = pos;
        ball.gameObject.SetActive(true);
        ball._isCopied.Value = true;
        if (_isBlue.Value)
            ball.BallColor(0);
        else
            ball.BallColor(1);
        return ball;
    }

    public void DestroyBall(BallController ball)
    {
        //공이 파괴되는 것이 아닌 풀로 돌려보냄
        ball.gameObject.SetActive(false);
        _ballsQueue.Enqueue(ball);
    }

   
}
