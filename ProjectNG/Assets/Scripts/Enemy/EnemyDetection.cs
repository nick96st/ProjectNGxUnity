using UnityEngine;
using System.Collections;

public class EnemyDetection : MonoBehaviour
{
    private EnemyMaster enemyMaster;
    private Transform myTransform;
    public Transform head;
    public LayerMask playerLayer;
    public LayerMask sightLayer;
    private float checkRate;
    private float nextCheck;
    [SerializeField]
    private float detectRadius; // 80
    private RaycastHit hit;

    void OnEnable()
    {
        SetInitialReferences();
        enemyMaster.EventEnemyDie += DisableThis;
    }

    void OnDisable()
    {
        enemyMaster.EventEnemyDie -= DisableThis;   
    }
	
	void Update ()
    {
        CarryOutDetection();
	}

    void SetInitialReferences()
    {
        enemyMaster = GetComponent<EnemyMaster>();

        if(head == null)
            head = myTransform;

        checkRate = Random.Range(0.8f, 1.2f);
    }

    void CarryOutDetection()
    {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;

            Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);

            if (colliders.Length > 0)
            {
                foreach (Collider potentialTargetCollider in colliders)
                {
                    if (potentialTargetCollider.CompareTag("player") && CanPotentialTargetBeSeen(potentialTargetCollider.transform))
                        // stop
                        break;
                }
            }
            else { enemyMaster.CallEventEnemyLostTarget(); }
        }
    }

    bool CanPotentialTargetBeSeen(Transform potentialTarget)
    {
        if(Physics.Raycast(head.position, potentialTarget.position, sightLayer))
        {
            if(hit.transform == potentialTarget)
            {
                enemyMaster.CallEventEnemySetNavTarget(potentialTarget);
                return true;
            }
            else
            {
                enemyMaster.CallEventEnemyLostTarget();
                return false;
            }
        }
        else
        {
            enemyMaster.CallEventEnemyLostTarget();
            return false;
        }
    }

    void DisableThis()
    {
        this.enabled = false;
    }
}
