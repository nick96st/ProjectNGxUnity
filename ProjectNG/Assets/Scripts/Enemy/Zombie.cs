using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Zombie : EnemyController
{
    PlayerHealth player;
    EnemyStates state;
    [SerializeField]
    Text placeholder;
    bool enemyClicked = false;

    void OnMouseDown()
    {
        enemyClicked = !enemyClicked;
    }

    void UpdateEnemyGUI()
    {
        placeholder.text = name + " / HP: " + health;
    }

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        myNav = GetComponent<NavMeshAgent>();

        // Gain Access to player script...
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Start()
    {
        // constant nav agent initials
        myNav.stoppingDistance = brakeSpeed;
        myNav.avoidancePriority = obstaclePriorityLevel;
        myNav.acceleration = 1.0f; // a normal acceleration so we just care about the speed value now

        // initial behaviour state
        state = EnemyStates.PATROL;
    }

    void Update()
    {
        if (myNav)
        {
            // Set speed values...
            myNav.speed = speed;
            myNav.angularSpeed = rotationSpeed;
        }

        // Set enemy look direction
        direction = playerTransform.position - myTransform.position;
        angle = Vector3.Angle(direction, head.up);

        // Enemy <-> Player Collision Detection
        if (Time.time > nextCheck && myNav.enabled)
        {
            nextCheck = Time.time + checkRate;
            hitColliders = Physics.OverlapSphere(myTransform.position, detectionRadius, detectionLayer);

            if ((hitColliders.Length > 0 && angle < fovRadius))
            {
                isInRange = true;
                state = EnemyStates.CHASE;
            }
            else if (isInRange && direction.magnitude < attackRange)
            {
                state = EnemyStates.ATTACK;
            }
            else state = EnemyStates.PATROL;

            if (health <= 0 && state != EnemyStates.DISAPPEAR)
            {
                health = 0;
                state = EnemyStates.DIE;
            }
        }

        // Show GUI on Click
        if (enemyClicked) UpdateEnemyGUI();
    }

    void FixedUpdate()
    {
        // PATROL
        if (state == EnemyStates.PATROL && waypoints.Length > 0)
        {
            Patrol(direction);
        }

        // CHASE
        if (state == EnemyStates.CHASE)
        {
            speed = 2.5f;
            rotationSpeed = 10.0f;
            Chase(direction, angle);
        }
        else
        {
            speed = 0.75f;
            rotationSpeed = 1.0f;
        }

        // ATTACK IS SET IN CHASE

        // DIE
        if (state == EnemyStates.DIE)
        {
            Die();
        }

        // DISAPPEAR
        if (state == EnemyStates.DISAPPEAR)
        {
            //Disappear();
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                // Avoid anim reload and Disable collider
                anim.SetBool("isDead", false);
                GetComponent<CapsuleCollider>().enabled = false;
                // Delete Object
                Invoke("Disappear", 8.0f);
            }
        }
    }

    protected override void Chase(Vector3 direction, float angle)
    {
        state = EnemyStates.CHASE;

        direction.y = 0;
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

        if (direction.magnitude > attackRange)
        {
            float betweenRange = attackRange - 0.5f;
            Vector3 playerPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z - betweenRange);
            myNav.SetDestination(playerPosition);
            myNav.Resume();

            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
        }
        // IF IN ATTACK RANGE - SET TO ATTACK
        else Attack();
    }

    protected override void Patrol(Vector3 direction)
    {
        state = EnemyStates.PATROL;

        anim.SetBool("isIdle", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", true);

        if (Vector3.Distance(waypoints[currentWP].GetComponent<Transform>().position, myTransform.position) < wpRange)
            currentWP = Random.Range(0, waypoints.Length);

        direction = waypoints[currentWP].GetComponent<Transform>().position - myTransform.position;

        myTransform.rotation = Quaternion.Slerp(
            myTransform.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.deltaTime
        );

        myNav.SetDestination(waypoints[currentWP].GetComponent<Transform>().position);
    }

    protected override void Attack()
    {
        /* SLIDING BUG FIX */
        myNav.velocity = Vector3.zero;
        myNav.Stop();

        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", true);

        // Do Attack
        player.GetHit(this.damage);
    }

    protected override void Die()
    {
        /* SLIDING BUG FIX */
        myNav.velocity = Vector3.zero;
        myNav.Stop();

        // NORMAL DEATH
        anim.SetBool("isDead", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        state = EnemyStates.DISAPPEAR;
    }

    void Disappear()
    {
        Destroy(this.gameObject);
    }
}