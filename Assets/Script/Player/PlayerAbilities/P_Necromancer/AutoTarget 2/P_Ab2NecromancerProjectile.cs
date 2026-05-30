using UnityEngine;
using UnityEngine.Audio;


public class P_Ab2NecromancerProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    public float damage;
    public float range;
    public float speed;
    private Vector2 startpos;
    private SpriteRenderer sprite;
    float rangeDetect; // When using the start Targer auto
    LayerMask layerMask;
    Transform target = null;
    bool isDead = false;
    int id;
    Vector2 dirTarget = Vector2.zero;
    private AudioSource audioSource;
    public AudioClip clipLoop;
    public AudioClip clipImpact;
    private bool isFrozen;

    private CircleCollider2D circleCollider;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        FrozenSFXTimeScale();
        sprite.flipX = (dirTarget.x< 0);

        float sqrDist = ((Vector2)transform.position - startpos).sqrMagnitude;

        if (sqrDist > rangeDetect * rangeDetect && target == null)
        {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, layerMask);
                float minDist = float.MaxValue;
                Transform transformak = null;
                for (int i = 0; i < hits.Length; i++)
                {
                    float dist = ((Vector2)hits[i].transform.position - (Vector2)transform.position).sqrMagnitude;
                    if (dist < minDist)
                    {
                        minDist = dist;
                    if (hits[i].gameObject.GetInstanceID() == id) continue;
                    transformak = hits[i].transform;
                }
                }

               if(transformak != null) target = transformak;
        }
        if (target != null) {

            dirTarget = (target.position - transform.position).normalized;
            rb.linearVelocity = dirTarget*speed;

        }
        if (sqrDist > range * range) DestroyAfterSFX();

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Projectile")) return;
        if (collision.CompareTag("Obstacle")) { rb.linearVelocity = Vector2.zero; DestroyAfterSFX(); };
        if (!isDead && collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyHealth health)) health.SetLastAttacker(this.gameObject);
            DestroyAfterSFX();
            collision.GetComponent<IDamageable>()?.TakeDamage(damage);
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void Launch(Vector2 direct, float damage, float range, float speed, LayerMask layer,int id) { 
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clipLoop;
        audioSource.Play();
        this.dirTarget =direct;
        startpos = transform.position;
        this.damage = damage;
        this.range = range;
        this.speed = speed;
        layerMask = layer;
        rangeDetect = range / 2;
        this.id = id;
       rb.linearVelocity = direct* speed;
    }


    public void DestroyAfterSFX()
    {
        if (!isDead)
        {
            isDead = true;
            audioSource.Stop();
            audioSource.PlayOneShot(clipImpact);
            circleCollider.enabled = false;
            sprite.enabled = false;
            animator.enabled = false;
            Destroy(this.gameObject, clipImpact.length);
        }
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
