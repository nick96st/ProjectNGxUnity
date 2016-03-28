using UnityEngine;
using System.Collections;

public class DerivedEnemy : BaseEnemyClass
{
    private Transform eTransform;
    private Rigidbody eRigidbody;
    public Transform head;
    private Animator anim;

    public Transform pTransform;
    public Rigidbody pRigidbody;

    public float walkMagnitude;
    public float attackMagnitude;
    public float fovRadius;

    private string state;

    public GameObject[] waypoints;

    private int currentWP = 0;
    public float rotationSpeed;
    public float speed;
    public float wpRange;

    enum AnimStates
    {
        IDLE,
        WALK,
        RUN,
        ATTACK,
        DIE
    };

    void Start()
    {
        eTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();

        state = "patrol";
    }

    void Update()
    {
        // Set enemy look direction
        Vector3 direction = pTransform.position - eTransform.position;
        float angle = Vector3.Angle(direction, head.up);

        if (state == "patrol" && waypoints.Length > 0)
        {
            Patrol(direction);
        }

        if (Vector3.Distance(pTransform.position, eTransform.position) < walkMagnitude && (angle < fovRadius || state == "chasing"))
        {
            Chase(direction, angle);
        }
        else
        {
            // Ensure walking is set to true
            anim.SetBool("isWalking", true);
            // Discontinue attacking
            anim.SetBool("isAttacking", false);
            // Finally, setting the state to patrolling
            state = "patrol";
        }
    }

    private void Chase(Vector3 direction, float angle)
    {
        // Set chasing state
        state = "chasing";

        // Set y not to move with player's y... position bugs
        direction.y = 0;

        // Set enemy rotation to player's position
        eTransform.rotation = Quaternion.Slerp(
            eTransform.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.deltaTime
        );

        if (direction.magnitude > attackMagnitude)
        {
            eTransform.Translate(0, 0, speed * Time.deltaTime);

            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
        else if (direction.magnitude < attackMagnitude && direction.magnitude < walkMagnitude)
        {
            anim.SetBool("isAttacking", true);
            anim.SetBool("isWalking", false);

            Attack();
        }
    }

    private void Patrol(Vector3 direction)
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", true);
        // If enemy is in the custom added range of the waypoint area,
        // because there are bugs with accuracy cuz of the enemy speed it gets exactly on the waypoint position.
        if (Vector3.Distance(waypoints[currentWP].GetComponent<Transform>().position, eTransform.position) < wpRange)
        {
            currentWP = Random.Range(0, waypoints.Length);

            /* if want Non-Random */
            // currentWP++;
            // currentWP = (currentWP >= waypoints.Length) ? 0 : currentWP;
        }

        // rotate towards waypoint
        direction = waypoints[currentWP].GetComponent<Transform>().position - eTransform.position;

        // Just in case...
        direction.y = 0;

        // Set enemy rotation towards current waypoint's positon
        eTransform.rotation = Quaternion.Slerp(
            eTransform.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.deltaTime
        );

        // speed gets dropped by 30% when patroling
        float slowPerc = speed * 0.30f;
        eTransform.Translate(0, 0, (speed - slowPerc) * Time.deltaTime);
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked!");
    }

    private void Attack()
    {
        // Do Attack
        Debug.Log("Attacking...");
    }

    private void GetHit()
    {

    }
}
