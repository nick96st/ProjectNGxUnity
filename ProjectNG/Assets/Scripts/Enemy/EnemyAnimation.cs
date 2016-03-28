using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{
    private EnemyMaster enemyMaster;
    private Animator myAnimator;

	void OnEnable()
    {
        SetInitialReferences();
        enemyMaster.EventEnemyDie += DisableAnimator;
        enemyMaster.EventEnemyWalking += SetAnimationWalk;
        enemyMaster.EventEnemyReachedNavTarget += SetAnimationIdle;
        enemyMaster.EventEnemyAttack += SetAnimationAttack;
        enemyMaster.EventEnemyDeductHealth += SetAnimationStruck;
    }

    void OnDisable()
    {
        enemyMaster.EventEnemyDie -= DisableAnimator;
        enemyMaster.EventEnemyWalking -= SetAnimationWalk;
        enemyMaster.EventEnemyReachedNavTarget -= SetAnimationIdle;
        enemyMaster.EventEnemyAttack -= SetAnimationAttack;
        enemyMaster.EventEnemyDeductHealth -= SetAnimationStruck;
    }

    void SetInitialReferences()
    {
        enemyMaster = GetComponent<EnemyMaster>();

        if (GetComponent<Animator>() != null)
            myAnimator = GetComponent<Animator>();
    }

    void SetAnimationIdle()
    {
        if (myAnimator != null && myAnimator.enabled)
        {
            myAnimator.SetBool("isChasing", false);
        }
    }

    void SetAnimationWalk()
    {
        if(myAnimator != null && myAnimator.enabled)
        {
            myAnimator.SetBool("isChasing", true);
        }
    }

    void SetAnimationAttack()
    {
        if (myAnimator != null && myAnimator.enabled)
        {
            myAnimator.SetTrigger("Attack");
        }
    }

    void SetAnimationStruck(float dmg)
    {
        if (myAnimator != null && myAnimator.enabled)
        {
            myAnimator.SetTrigger("Struck");
        }
    }

    void DisableAnimator()
    {
        if(myAnimator != null)
        {
            myAnimator.enabled = false;
        }
    }
}
