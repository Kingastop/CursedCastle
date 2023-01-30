using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class WizardAttackState : MonoBehaviour
{
    [SerializeField] Animator animator;

    public event Action TriggerAttack;

    public bool attacked;

    private void Start()
    {
        TriggerAttack += EndOfAnimation;
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void EndOfAnimation()
    {
        attacked = true;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        animator.SetBool("IsAttacking", false);
    }

    private void AttackTriggered() =>
        TriggerAttack?.Invoke();

    private void OnDestroy()
    {
        TriggerAttack -= EndOfAnimation;
    }
}
