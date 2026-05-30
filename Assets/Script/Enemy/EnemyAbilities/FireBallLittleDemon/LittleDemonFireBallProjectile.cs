using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class LittleDemonFireBallProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float sqrRange;
    private float speed;
    private float damage;
    private Vector2 startpos;
    private bool isDead = false;
    private int id;
    private AudioClip impactClip;
    private AudioClip loopClip;
    public AudioSource audioSource;

    public CircleCollider2D circleCollider;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private bool isFrozen;
    void Update()
    {
        FrozenSFXTimeScale();
        if (!isDead && (startpos -(Vector2)transform.position).sqrMagnitude>sqrRange)
        {
            isDead = true;
           StartCoroutine(DestroyAfterSFX());
        }
    }

    public void Launch(Vector2 dir,float speed,float range,float damage,int id,AudioClip impactclip, AudioClip loopclip)
    {
        //audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        this.impactClip = impactclip;
        this.loopClip = loopclip;

        startpos = transform.position;
       
        this.sqrRange = range*range;
        this.speed = speed;
        this.damage = damage;
        this.id = id;
        LoopClip();
        rb.linearVelocity = dir* this.speed;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.GetInstanceID() == id) return;

        if (!collision.CompareTag("Projectile"))
        {
           
            collision.GetComponent<IDamageable>()?.TakeDamage(this.damage);
            rb.linearVelocity = Vector2.zero;
            isDead = true;
            StartCoroutine(DestroyAfterSFX());
        }

    }

    public void LoopClip()
    {
        audioSource.clip = loopClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ImpactClip()
    {
        if (audioSource == null) return;
        audioSource.Stop();
        audioSource.PlayOneShot(impactClip);
    }

    IEnumerator DestroyAfterSFX()
    {
        
        float timedalay = 0.1f;
        ImpactClip();
        circleCollider.enabled = false;

        yield return new WaitForSeconds(timedalay);

        spriteRenderer.enabled = false;
        float remainingtimeSfx = impactClip.length - timedalay;
        if (animator != null) animator.enabled = false;

        if (remainingtimeSfx > 0) yield return new WaitForSeconds(remainingtimeSfx);
       
        Destroy(gameObject);
    }
    public void DestroyAfterSF1X()
    {
       circleCollider.enabled = false;
       spriteRenderer.enabled = false;
       if(animator != null) animator.enabled = false;
       Destroy(gameObject, impactClip.length);
    }
    private void FrozenSFXTimeScale()
    {
        if (Time.timeScale == 0f)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                isFrozen = true;
            }
        }
        else if (isFrozen)
        {
            audioSource.UnPause();
            isFrozen = false;
        }
    }
}
