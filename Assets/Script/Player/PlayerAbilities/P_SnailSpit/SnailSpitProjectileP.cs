using UnityEngine;
using UnityEngine.Audio;

public class SnailSpitProjectileP : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public float range;
    
    [HideInInspector] public Vector2 mousePos;


    private bool isDead = false;
    private Animator animatorProjectile ;
    private Vector2 startPos;
    private Rigidbody2D rb;

    [HideInInspector] public AudioClip loopclip;
    [HideInInspector] public AudioClip impactclip;
    private AudioSource audioSource;
    private bool isFrozen;
    void Update()
    {
        FrozenSFXTimeScale();
        if (((Vector2)transform.position - startPos).sqrMagnitude > range * range) Explode();
    }
    public void Launch(Vector2 mousePos, float speed, float range,float damage)
    {
        animatorProjectile = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = loopclip;
        audioSource.Play();

        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        this.mousePos = mousePos;
        this.speed = speed;
        this.range = range;
        this.damage = damage;
        rb.linearVelocity = this.mousePos * this.speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (!isDead && collision.CompareTag("Enemy") && !collision.CompareTag("Player") && !collision.CompareTag("Projectile") )
        {
            if (collision.TryGetComponent(out EnemyHealth health)) health.SetLastAttacker(this.gameObject);
            collision.GetComponent<IDamageable>()?.TakeDamage(damage);
            //if (collision.TryGetComponent(out IKnockbackResistance d)) d.ApplyKnockback(mousePos, 55, 0.5f);
            Explode();
        }
        if(collision.CompareTag("Obstacle")) Explode();

    }
    public void Explode()
    {
        if (isDead) return;

        isDead = true;
        audioSource.Stop();
        audioSource.PlayOneShot(impactclip);
        animatorProjectile.SetTrigger("Explode");
        rb.linearVelocity = Vector2.zero;

        float animLength = animatorProjectile.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animLength+0.1f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
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

