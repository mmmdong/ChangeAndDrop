using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenFloorController : MonoBehaviour
{
    private int _count;
    private bool _isOver;


    [SerializeField] TextMesh _textMesh;
    [SerializeField] GameObject _parent;

    private void OnTriggerEnter(Collider other)
    {
        var ball = other.GetComponent<BallController>();
        var targetCount = int.Parse(_textMesh.text);
        if (ball._isCopied.Value == false && !_isOver)
        {
            _count++;
            ball._isCopied.Value = true;
        }

        if (_count >= targetCount)
            _isOver = true;

        if (_isOver)
            StartCoroutine(CoBreakBlock());
    }

    private IEnumerator CoBreakBlock()
    {
        yield return new WaitForSeconds(0.5f);

        _parent.gameObject.SetActive(false);
        FXManager.instance.PlayBallExplosionFX(_parent.transform.position, Color.white);
        GameManager.instance._camFloorQueue.Dequeue().SetActive(false);
    }
}
