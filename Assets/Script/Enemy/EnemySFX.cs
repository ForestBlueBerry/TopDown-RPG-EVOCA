using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class EnemySFX : MonoBehaviour
{
    private AudioClip walk_Clip;
    private AudioClip run_Clip;

    private AudioClip death_Clip;
    private AudioClip hurt_Clip;

    public AudioSource move_Source;
    public AudioSource attack_Source;
    public AudioSource hurt_Source;

    private int lastframe;
   
    public void AttackLaunchClip(AudioClip clip)
    {
        attack_Source.PlayOneShot(clip);
    }

    public void AttackImpactClip(AudioClip clip)
    {
        if (!CheckLastFrame()) return;
        attack_Source.PlayOneShot(clip);
    }

    public void WalkClip(bool isWalk)
    {
        if (isWalk)
        {
            if (move_Source.clip != walk_Clip || !move_Source.isPlaying)
            {
                move_Source.clip = walk_Clip;
                move_Source.loop = true;
                move_Source.Play();
            }
          
        }
        else if (move_Source.clip == walk_Clip)
        {
            move_Source.Stop();
        }
    }

    public void RunClip(bool isRun)
    {
        if (isRun)
        {
            if (move_Source.clip != run_Clip || !move_Source.isPlaying)
            {
                move_Source.clip = run_Clip;
                move_Source.loop = true;
                move_Source.Play();
            }
      
        }
        else if (move_Source.clip == run_Clip)
        {
            move_Source.Stop();
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
        if (Time.frameCount == lastframe) return false;
        lastframe = Time.frameCount;
        return true;
    }

    public void SetupSFXController(EnemySO enemySO)
    {
        walk_Clip = enemySO.setSFXEnemy.walk_Clip;
        run_Clip = enemySO.setSFXEnemy.run_Clip;
        death_Clip = enemySO.setSFXEnemy.death_Clip;
        hurt_Clip = enemySO.setSFXEnemy.hurt_Clip;
    }
}
