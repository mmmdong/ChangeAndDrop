using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using static UnityEngine.ParticleSystem;

public class BallController : MonoBehaviour
{
    public Material[] _ballMatArr;
    public MeshRenderer _meshRender;
    public Rigidbody _ballRb;
    public BoolReactiveProperty _isCopied = new BoolReactiveProperty(false);
    public int _ballIdx;

    private Vector3 collidedPoint;

    [SerializeField] private ParticleSystem _particleSystem;

    MainModule _particleMain;

    private void Awake()
    {
        _ballRb = GetComponent<Rigidbody>();
        _isCopied.TakeUntilDestroy(this).Subscribe(x =>
        {
            if (x == true)
            {
                StartCoroutine(CoChangeStat());
            }
        });
    }

    private IEnumerator CoChangeStat()
    {
        yield return new WaitForSeconds(2.5f);


        _isCopied.Value = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idx">0 = blue, 1 = orange</param>
    public void BallColor(int idx)
    {
        _meshRender.material = _ballMatArr[idx];
        _particleMain = _particleSystem.main;
        _particleMain.startColor = new ParticleSystem.MinMaxGradient(_meshRender.material.color);
        _ballIdx = idx;
    }

    public void DestroyBall()
    {
        BallObjPool.instance.DestroyBall(this);
    }
    public void ExplosionBalls()
    {
        Collider[] surroundedCubes = Physics.OverlapSphere(collidedPoint, 2f);
        float explosionForce = 500f;
        float explosionRadius = 1.5f;

        for (var i = 0; i < surroundedCubes.Length; i++)
        {
            if (surroundedCubes[i].attachedRigidbody != null)
                surroundedCubes[i].attachedRigidbody.AddExplosionForce(explosionForce, collidedPoint, explosionRadius);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collidedPoint = collision.contacts[0].point;
    }
}
