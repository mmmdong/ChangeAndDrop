using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ballExplosion;

    ParticleSystem.MainModule _ballExMainModule;

    public static FXManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _ballExMainModule = _ballExplosion.main;
    }

    public void PlayBallExplosionFX(Vector3 pos, Color color)
    {
        _ballExMainModule.startColor = new ParticleSystem.MinMaxGradient(color);
        _ballExplosion.transform.position = pos;
        _ballExplosion.Play();
    }
}
