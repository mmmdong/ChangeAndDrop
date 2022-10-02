using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public int _colorIdx;
    private Transform _spawnArea;
    [SerializeField] private TextMesh _textMesh;
    private void OnTriggerExit(Collider other)
    {
        var ball = other.GetComponent<BallController>();
        _spawnArea = ball.gameObject.transform;
        if(ball._ballIdx != _colorIdx)
        {
            ball.DestroyBall();
            return;
        }
        if (ball._isCopied.Value == false)
        {
            var multiPly = int.Parse(_textMesh.text[1].ToString());
            for (var i = 0; i < multiPly - 1; i++)
            {
                BallObjPool.instance.Spawn(_spawnArea.position);
            }
        }
        ball._isCopied.Value = true;
    }

}
