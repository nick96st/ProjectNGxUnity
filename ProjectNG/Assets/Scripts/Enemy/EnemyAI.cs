using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public LayerMask detectionLayer;

    private Transform transform;
    private NavMeshAgent nav;
    private Collider[] hitColliders;

    private float checkRate;
    private float nextCheck;
    private float detectionRadius = 10f;

	void Start ()
    {
        transform = GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        checkRate = Random.Range(0.8f, 1.2f);
	}
	
	void Update ()
    {
        CheckIfPlayerInRange();
	}

    void CheckIfPlayerInRange()
    {
        if (Time.time > nextCheck && nav.enabled)
        {
            nextCheck = Time.time + checkRate;

            hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
            
            if(hitColliders.Length > 0)
            {
                nav.SetDestination(hitColliders[0].GetComponent<Transform>().position);
            }
        }
    }
}
