using UnityEngine;
using System.Collections;

// Non instantiatable - only inheritable!!! Fully abstract
public abstract class EnemyController : MonoBehaviour
{
    // stats
    public string name;
    public float health;
    public float damage;
    
    //public float walkMagnitude;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float fovRadius;
    [SerializeField]
    protected float wpRange;
    [SerializeField]
    protected float detectionRadius;

    protected int currentWP = 0;

    [SerializeField]
    protected Transform head;
    [SerializeField]
    protected Transform playerTransform;
    protected Transform myTransform;
    protected NavMeshAgent myNav;
    protected Animator anim;
    public LayerMask detectionLayer;
    public GameObject[] waypoints;
    protected Collider[] hitColliders;

    protected float checkRate;
    protected float nextCheck;

    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float rotationSpeed;
    [SerializeField]
    protected float brakeSpeed;
    [SerializeField]
    protected int obstaclePriorityLevel;

    protected Vector3 direction;
    protected float angle;
    protected bool isInRange = false;
    protected bool isDead = false;

    protected enum EnemyStates
    {
        PATROL,
        WANDER,
        CHASE,
        ATTACK,
        DIE,
        DISAPPEAR
    };

    public void GetHit(float damageValue)
    {
        damageValue = (this.health - damageValue < 0) ? this.health : damageValue;
        this.health -= damageValue;
    }

    protected abstract void Chase(Vector3 direction, float angle);
    protected abstract void Patrol(Vector3 direction);
    protected abstract void Attack();
    protected abstract void Die();
}
