using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEffectCtrl : MonoBehaviour
{
    public AudioClip GunFireSound;
    private AudioSource AudioSource;
    public float Volume = 0.5f;

    public ParticleSystem MuzzleFlash;

    // Start is called before the first frame update
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
