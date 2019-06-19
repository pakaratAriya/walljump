using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHelper : MonoBehaviour
{
    private AudioSource audioS;
    public static SoundHelper sh;
    public AudioClip clickedSfx;
    public AudioClip deathSfx;
    public AudioClip jumpSfx;
    public AudioClip mineExplosionSfx;
    public AudioClip postJumpSfx;
    public AudioClip gemSfx;

    private void Awake()
    {
        if (sh == null)
        {
            audioS = GetComponent<AudioSource>();
            sh = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public static void PlayClickSfx()
    {
        if(sh.clickedSfx)
            sh.audioS.PlayOneShot(sh.clickedSfx);
    }

    public static void PlayDeathSfx()
    {
        if (sh.deathSfx)
            sh.audioS.PlayOneShot(sh.deathSfx);
    }

    public static void PlayJumpSfx()
    {
        if (sh.jumpSfx)
            sh.audioS.PlayOneShot(sh.jumpSfx);
    }

    public static void PlayMineExplosionSfx()
    {
        if (sh.mineExplosionSfx)
            sh.audioS.PlayOneShot(sh.mineExplosionSfx);
    }

    public static void PlayPostJumpSfx()
    {
        if (sh.postJumpSfx)
            sh.audioS.PlayOneShot(sh.postJumpSfx);
    }

    public static void PlayGemSfx()
    {
        if (sh.gemSfx)
            sh.audioS.PlayOneShot(sh.gemSfx);
    }
}
