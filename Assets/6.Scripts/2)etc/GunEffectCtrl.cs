using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GunEffectCtrl : MonoBehaviour
{
    public AudioClip GunFireSound;
    private AudioSource AudioSource;
    public float Volume = 0.5f;

    public ParticleSystem MuzzleFlash;

    void Start()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Fire()
    {
        AudioSource.PlayOneShot(GunFireSound, Volume);
        MuzzleFlash.Play();
    }
}
