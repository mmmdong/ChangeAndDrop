using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBoxController : MonoBehaviour
{
    [SerializeField] private GameObject _goalBox;
    [SerializeField] private Collider[] _colliders;
    private bool _getRb;
    private List<BallController> _ballList = new List<BallController>();
    private int _count;
    private Rigidbody boxRig;

    private void OnTriggerEnter(Collider other)
    {
        _count++;
        var ball = other.GetComponent<BallController>();
        _ballList.Add(ball);
        var boxPos = _goalBox.transform.position;

        if (_count % 2 == 0 && !_getRb)
        {
            _goalBox.transform.position = new Vector3(boxPos.x, boxPos.y - 0.2f, boxPos.z);
        }


        if (_count >= 70 && !_getRb)
        {
            boxRig = _goalBox.AddComponent<Rigidbody>();
            boxRig.constraints = RigidbodyConstraints.FreezeRotation;
            GameManager.instance._isGaming.Value = false;
            _getRb = true;
            StartCoroutine(CoExplosion(_ballList));
        }
    }

    private IEnumerator CoExplosion(List<BallController> ball)
    {
        yield return new WaitForSeconds(5.0f);

        boxRig.constraints = RigidbodyConstraints.FreezeAll;

        for(var i = 0; i < ball.Count; i++)
        {
            ball[i].ExplosionBalls();
        }
    }
}
