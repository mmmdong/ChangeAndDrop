using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var ball = collision.gameObject.GetComponent<BallController>();
        BallObjPool.instance.DestroyBall(ball);
    }
}
