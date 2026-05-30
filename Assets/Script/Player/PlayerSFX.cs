using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    private AudioClip walk_Clip;
    private AudioClip run_Clip;

    private AudioClip death_Clip;
    private AudioClip hurt_Clip;

    public AudioSource move_Source;
    public AudioSource attack_Source;
    public AudioSource hurt_Source;

    private int lastframe;

    private Coroutine fadecor;

    public void AttackLaunchClip(AudioClip clip)
    {
        attack_Source.PlayOneShot(clip,1f);
    }
    public void AttackImpactClip(AudioClip clip)
    {
        if (!CheckLastFrame()) return;
        attack_Source.PlayOneShot(clip,0.9f);
    }

    public void WalkClip(bool isWalk)
    {
        if (isWalk)
        {
            if (move_Source.clip != walk_Clip || !move_Source.isPlaying  )
            {
                move_Source.clip = walk_Clip;
                move_Source.loop = true;
                move_Source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                move_Source.Play();
            }
           FadeSFX(move_Source, 0.6f, 0.5f);
        }
        else if (move_Source.clip == walk_Clip)
        {
                FadeSFX(move_Source, 0.0f, 0.08f);
        }
    }
    public void RunClip(bool isRun)
    {
        if (isRun)
        {
            if (move_Source.clip != run_Clip || !move_Source.isPlaying )
            {
                move_Source.clip = run_Clip;
                move_Source.loop = true;
                move_Source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                move_Source.Play();
            }
            FadeSFX(move_Source, 0.5f, 0.5f);
        }
        else if (move_Source.clip == run_Clip)
        {
                FadeSFX(move_Source, 0.0f, 0.08f);
        }
    }
    public void DeathClip()
    {
        hurt_Source.Stop();
        move_Source.Stop();
        hurt_Source.PlayOneShot(death_Clip);
      
    }
    public void HurtClip()
    {
        hurt_Source.PlayOneShot(hurt_Clip);
    }
    private bool CheckLastFrame()
    {
        if(Time.frameCount == lastframe) return false;
        lastframe = Time.frameCount;
        return true;
    }

    public void SetupSFXController(PlayerSO playerSo)
    {
        walk_Clip = playerSo.setSFX.walk_Clip;
        run_Clip =playerSo.setSFX.run_Clip;
        death_Clip =playerSo.setSFX.death_Clip;
        hurt_Clip = playerSo.setSFX.hurt_Clip;
}
    public void FadeSFX(AudioSource audio, float target, float during)
    {
        if (Mathf.Approximately(audio.volume, target) && target > 0) return;
        if (fadecor != null) StopCoroutine(fadecor);
        fadecor = StartCoroutine(FadeCorutine(audio, target, during));
    }

    IEnumerator FadeCorutine(AudioSource audio, float target, float during)
    {
        float currenttime = 0;

        float volume = audio.volume;
        
        while (currenttime < during)
        {
            currenttime += Time.deltaTime;
            audio.volume = Mathf.Lerp(volume, target, currenttime / during);
            yield return null;
        }
        if (target <= 0.1f ) {
            audio.Stop();
            move_Source.clip = null;
        }
    }
}
