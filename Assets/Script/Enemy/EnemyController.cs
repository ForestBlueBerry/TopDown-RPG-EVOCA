using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IKnockbackResistance, IHasLeague, IHasMagnitusm
{
    [Header("Animator hash for optimize!!!")]
    private readonly int hashX = Animator.StringToHash("X");
    private readonly int hashY = Animator.StringToHash("Y");
    private readonly int hashIsRun = Animator.StringToHash("isRun");
    private readonly int hashIsMove = Animator.StringToHash("isMove");


    private float thinkingBeforeWanderer;
    private float speedMove;
    private float speedRun;
    private float knockbackTimer;
    private float detectionRadius;
    private float StunAfterAttack;
    private float stoppingDistance;

    private float timeMagnetism;

    private Vector2 direct;
    private Vector2 lastdirAnimator = Vector2.zero;
    private RaycastHit2D ray;
    public NavMeshAgent agent;
    private Collider2D[] arrayZone = new Collider2D[10];
    private ContactFilter2D contactFilter = new ContactFilter2D();

    private Vector3 targetpos = Vector3.zero;
    private Vector3 closesPointVictim = Vector3.zero;


    public EnemyName enemyName;

    [HideInInspector] public Transform victim = null;
    [HideInInspector] public float distanceTargetPosSqr;
    [HideInInspector] public float distanceClosesPointVictimSqr;
    [HideInInspector] public EnemySO enemySO;
    [HideInInspector] public bool canSeeForAttack = false;
    [HideInInspector] public Animator animator;
    public LayerMask generalLayer;
    public LayerMask obstacleLayer;
    [HideInInspector] public CapsuleCollider2D colider;
    [HideInInspector] public Rigidbody2D rb;

    private bool changePathMateObstacle = false;
    private Vector3 currentDetourPoint = Vector3.zero;
    private bool agentVelocity005 = false;
    private float cooldownChangePath = 0f;
    private WaitForSeconds detectionDelay = new WaitForSeconds(0.2f);
    private float detectionRadiusSqr;

    private Vector3 lastSampledVictimPos;
    private float distanceNearMate;
    private float distanceNearMateSteps;

    public Bounds bounds;

    public bool isBoss ;

    private EnemySFX enemySFX;

    private bool isRunSFX;
    private bool lastisRunSFX;

    private bool isWalkSFX;
    private bool lastisWalkSFX;

    private bool frozenSFXscale;
    void Start()
    {
        enemySFX = GetComponent<EnemySFX>();
        SetupSO();
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(0, 99);
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        thinkingBeforeWanderer = 3f;
        distanceClosesPointVictimSqr = detectionRadius;//When game at the starting Then is value have zero distance;Enemy will attacking instantly  


        contactFilter.useLayerMask = true;
        contactFilter.SetLayerMask(generalLayer);
        StartCoroutine(DetectionRoutine());
        detectionRadiusSqr = detectionRadius * detectionRadius;

        distanceNearMate = stoppingDistance > 2f ? (stoppingDistance * stoppingDistance) : 6.25f; // respect for teammates HF
        distanceNearMateSteps = Mathf.Clamp(stoppingDistance, 2.0f, 3.0f);
    }
    void Timers()
    {
        if (!agent.enabled && timeMagnetism > 0)
        {
            timeMagnetism -= Time.deltaTime;
         
            if (timeMagnetism <= 0)
            {
                rb.linearVelocity = Vector2.zero;
                agent.enabled = true;
            }
        }

        if (cooldownChangePath > 0f) cooldownChangePath -= Time.deltaTime;
            if (StunAfterAttack > 0) StunAfterAttack -= Time.deltaTime;
        if (knockbackTimer > 0) {
            knockbackTimer -= Time.deltaTime;
            isRunSFX = false;
            isWalkSFX = false;
        } 
            if (victim != null)
            {
                distanceTargetPosSqr = (targetpos - transform.position).sqrMagnitude;
                distanceClosesPointVictimSqr = (closesPointVictim - transform.position).sqrMagnitude;
            }
            else
            {   
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) thinkingBeforeWanderer -= Time.deltaTime;
            }
            //SFX
        if (Time.timeScale == 0f) {
            if (enemySFX.move_Source.isPlaying) {
                enemySFX.move_Source.Pause();
                frozenSFXscale= true;
            }
            return;
        }

        if (frozenSFXscale) {
            frozenSFXscale = false;
            enemySFX.move_Source.UnPause();
                };
    }

    void Update()
    {
        Timers();
        if (!agent.enabled) return;
        
        agentVelocity005 = agent.velocity.sqrMagnitude > 0.05f;


        if (knockbackTimer > 0) return;
        if (timeMagnetism > 0) {
            SetAnimator(direct, hashIsMove, agentVelocity005);
            return;
        }

       
        if ( agent.isActiveAndEnabled && agent.isOnNavMesh && agent.isStopped)
        {
            agent.isStopped = false;
            rb.linearVelocity = Vector2.zero;
        }

        if (victim != null)
        {
            isWalkSFX = false;
            if (distanceTargetPosSqr <= detectionRadiusSqr && StunAfterAttack <= 0)
            {
                //direct = (victim.position - transform.position).normalized;
                direct = (agent.steeringTarget - transform.position).normalized;  //test

                isRunSFX = agentVelocity005; //tyt SFX

                if (changePathMateObstacle && distanceClosesPointVictimSqr <= (stoppingDistance + 2.5f) * (stoppingDistance + 2.5f))
                {
                    
                    float distToDetourSqr = (currentDetourPoint - transform.position).sqrMagnitude;
                    if (currentDetourPoint != Vector3.zero && distToDetourSqr < 1.0f)//bugs
                    {
                        agent.ResetPath();
                        currentDetourPoint = Vector3.zero;
                    }

                    if (currentDetourPoint == Vector3.zero)
                    {
                        currentDetourPoint = ChangePathMateeObstacle();
                        if (currentDetourPoint != Vector3.zero) agent.SetDestination(currentDetourPoint);
                    }

                    agent.speed = speedMove;
                    SetAnimator(direct, hashIsRun, agentVelocity005);
                }
                else if (distanceClosesPointVictimSqr <= stoppingDistance * stoppingDistance)
                {
                    direct = (victim.position - transform.position).normalized;  //test
                    currentDetourPoint = Vector3.zero;
                    if (agent.hasPath) agent.ResetPath();
                    SetAnimator(direct, hashIsRun, false);
                }
                else if (distanceClosesPointVictimSqr > (stoppingDistance + 0.1f) * (stoppingDistance + 0.1f) || agent.hasPath)
                {
                    direct = (victim.position - transform.position).normalized; //test
                    if (!agent.pathPending && agent.isOnNavMesh)
                    {
                        agent.speed = speedRun;
                        agent.SetDestination(targetpos);
                    }
                    RunAnimationsToChase();
                }
            }
            else
            {
             
                if (agent.hasPath) agent.ResetPath();
                SetAnimator(direct, hashIsRun, false);
            }
        }
        else
        {
            isRunSFX = false;//SFX
            isWalkSFX = false; //SFX

            if (agentVelocity005 && agent.hasPath) {
                direct = (agent.steeringTarget - transform.position).normalized;
                isWalkSFX = true; //SFX
            }

            if (!agent.hasPath && agent.isOnNavMesh) isWanderer();

            if (agent.hasPath && agent.velocity.sqrMagnitude < 1f && agent.remainingDistance < stoppingDistance + 2f) // was bug
            {
                agent.ResetPath();
            }

            SetAnimator(direct, hashIsMove, agentVelocity005);
        }

        if (isRunSFX != lastisRunSFX ) {  //SFX
            enemySFX.RunClip(isRunSFX);
            lastisRunSFX = isRunSFX;
        }

        if (isWalkSFX != lastisWalkSFX)
        { 
            enemySFX.WalkClip(isWalkSFX);
            lastisWalkSFX = isWalkSFX;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void SetAnimator(Vector2 direct, int stateAnimHash, bool isMove)
    {
        animator.SetBool(stateAnimHash, isMove);
        if (direct != Vector2.zero)
        {
            animator.SetFloat(hashX, direct.x);
            animator.SetFloat(hashY, direct.y);
            lastdirAnimator = direct;
        }
        else
        {

            animator.SetFloat(hashX, lastdirAnimator.x);
            animator.SetFloat(hashY, lastdirAnimator.y);
        }

    }

    public void StopBeforeAttack(float attackTimer)
    {
        this.StunAfterAttack = attackTimer;
    }

    private void SetupSO()
    {
        enemyName = enemySO.enemyName;
        colider = GetComponent<CapsuleCollider2D>();
        colider.offset = enemySO.colliderOffset;
        colider.direction = enemySO.capsuleDirection;
        colider.size = enemySO.colliderSize;

        rb = GetComponent<Rigidbody2D>();


        animator = GetComponent<Animator>();

        speedMove = enemySO.speedMove;
        speedRun = enemySO.speedRun;
        detectionRadius = enemySO.detectionRadius;
        stoppingDistance = enemySO.stoppingDistance;

        isBoss = enemySO.boss;


        if (enemySO.animator != null)
        {
            animator.runtimeAnimatorController = enemySO.animator;
        }
        enemySFX.SetupSFXController(enemySO);
    }
    void IKnockbackResistance.ApplyKnockback(Vector2 forceDirection, float strength, float knockbackTimer)
    {
        this.knockbackTimer = knockbackTimer;
        if (agent.isOnNavMesh) { 
            agent.ResetPath();
            agent.isStopped = true;
        }
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(forceDirection * strength, ForceMode2D.Impulse);
    }

    IEnumerator DetectionRoutine()
    {
        while (true)
        {
            Collider2D _victim = null;
            Vector3 _closesPoint = Vector3.zero;
            canSeeForAttack = false;
            float mindist = float.MaxValue;


            int count = Physics2D.OverlapCircle(transform.position, detectionRadius, contactFilter, arrayZone);

            for (int i = 0; i < count; i++)
            {

                if (arrayZone[i].gameObject == this.gameObject) continue;

                if (arrayZone[i].TryGetComponent(out IHasLeague league))
                {
                    if (league.GetEnemyName() == EnemyName.none || league.GetEnemyName() == enemyName) continue;

                }

                Vector2 d = (arrayZone[i].transform.position - transform.position);
                float dSqr = d.sqrMagnitude;

                if (mindist > dSqr)
                {
                    mindist = dSqr;
                    _victim = arrayZone[i];
                }
            }
            if (_victim != null)
            {
                _closesPoint = _victim.ClosestPoint(transform.position);
                victim = _victim.transform;


                if ((lastSampledVictimPos - _closesPoint).sqrMagnitude > 2.25f)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(_closesPoint, out hit, 2.0f, NavMesh.AllAreas))
                    {
                        closesPointVictim = _closesPoint;
                        targetpos = hit.position;
                    }
                }


                Vector2 dirRay = _closesPoint - transform.position;
                float distRay = dirRay.magnitude;
                ray = Physics2D.Raycast(transform.position, dirRay, distRay, obstacleLayer);

                if (ray.collider != null || timeMagnetism > 0)
                {

                    canSeeForAttack = false;
                    changePathMateObstacle = false;
                }
                else
                {
                    canSeeForAttack = true;
                    changePathMateObstacle = false;

                    for (int j = 0; j < count; j++) //Detect Teammates
                    {
                        if (arrayZone[j].gameObject == this.gameObject || arrayZone[j] == _victim) continue;

                        if (arrayZone[j].TryGetComponent(out IHasLeague l) && l.GetEnemyName() == enemyName)
                        {

                            Vector2 dirToTarget = (_closesPoint - transform.position).normalized;
                            Vector2 dirToAlly = (arrayZone[j].transform.position - transform.position).normalized;


                            if (Vector2.Dot(dirToTarget, dirToAlly) > 0.88f) //11*2 0.87f    0.95f
                            {

                                float sqrDistToAlly = (arrayZone[j].transform.position - transform.position).sqrMagnitude;
                                if (sqrDistToAlly < distanceNearMate) //2.5f distance to near teammate 6.25f
                                {
                                    canSeeForAttack = false;
                                    changePathMateObstacle = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (victim != null)
                {
                    thinkingBeforeWanderer = 5f;
                    distanceClosesPointVictimSqr = detectionRadiusSqr;
                }

                victim = null;
                canSeeForAttack = false;
                changePathMateObstacle = false;
            }
            yield return detectionDelay;
        }
    }
    bool RandomWandererPoint(Bounds bounds, out Vector3 result)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 10f;// 10f - range to wanderer
            float randomPointInCircleX = transform.position.x + randomOffset.x;
            float randomPointInCircleY = transform.position.y + randomOffset.y;

            float nearAllowPointX = Mathf.Clamp(randomPointInCircleX, bounds.min.x, bounds.max.x);
            float nearAllowPointY = Mathf.Clamp(randomPointInCircleY, bounds.min.y, bounds.max.y);

            Vector3 randompos = new(nearAllowPointX, nearAllowPointY);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randompos, out hit, 2.0f, NavMesh.AllAreas))
            {
                if ((hit.position - transform.position).sqrMagnitude > 8f)
                {
                    result = hit.position;

                    return true;
                }
            }
        }
        result = transform.position;
        return false;
    }
    private void isWanderer() 
    {
        animator.SetBool("isRun", false);
        if (agent.remainingDistance > agent.stoppingDistance) return;

        if (thinkingBeforeWanderer > 0f) return;
       
        thinkingBeforeWanderer = 5f;

        if (RandomWandererPoint(bounds,out targetpos))
        {
            agent.speed = speedMove;
        };
        if (agent.isActiveAndEnabled && agent.isOnNavMesh && !agent.pathPending) agent.SetDestination(targetpos);

    }
    public EnemyName GetEnemyName()
    {
        return enemyName;
    }

    private Vector3 ChangePathMateeObstacle()
    {
        if (cooldownChangePath > 0) return transform.position;
        Vector3 dirFromPlayer = (transform.position - closesPointVictim).normalized;
        float randomAngle = Random.Range(-20f, 20f); // randomPos
        Quaternion spread = Quaternion.Euler(0, 0, randomAngle); // randomPos
        var randomPerpend = new Vector3(-dirFromPlayer.y, dirFromPlayer.x, 0f);
        Vector3 chaoticPerpend = spread * randomPerpend; // randomPos
        float realDistVictim = Mathf.Sqrt(distanceClosesPointVictimSqr);
        float side = (Random.value > 0.5f ? 1f : -1f);
        float maxDisttoVictim = Mathf.Max(realDistVictim, stoppingDistance);

        Vector3 readyPoint = closesPointVictim + ((Vector3)dirFromPlayer * maxDisttoVictim) + (chaoticPerpend * side * (distanceNearMateSteps));//step for avoid team1.5f
        NavMeshHit hideo;

        if (NavMesh.SamplePosition(readyPoint, out hideo, 2.0f, NavMesh.AllAreas))
        {
            cooldownChangePath = 2f;
            return hideo.position;
        }
        return transform.position;
    }

    private void RunAnimationsToChase()
    {
        if (distanceClosesPointVictimSqr > (stoppingDistance + 1f) * (stoppingDistance + 1f))
        {
            animator.SetBool(hashIsMove, false);
            SetAnimator(direct, hashIsRun, agentVelocity005);
        }
    }

    public void onMagnitusm(Vector2 point,float timeMagnetism,float strenght)
    {
        if (this.timeMagnetism <= 0f) {
            this.timeMagnetism = timeMagnetism;
            rb.linearVelocity = Vector2.zero;
       
        }

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.isStopped = true;
            agent.enabled = false;
        }
      
        rb.linearVelocity = (point - (Vector2)transform.position).normalized * strenght;
        

    }
}
