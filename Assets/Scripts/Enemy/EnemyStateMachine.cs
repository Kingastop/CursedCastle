using UnityEngine;
using UnityEngine.Events;
using System;
using Player;
using UnityEngine.AI;
using System.Collections.Generic;

namespace Enemy
{
    public class EnemyStateMachine : MonoBehaviour
    {
        [SerializeField] Animator animator;

        [SerializeField] private Transform checkPoint;
        [SerializeField] NavMeshAgent agent;

        [SerializeField] EnemyReward reward; 
        private float checkRadius = 1;
        private Collider2D[] hits;
        private int searchedCounts;
        private Vector3 target;

        private bool dead;

        private IEnumerable<Collider2D> Hits => hits;
        private Collider2D FirstHit => hits[0];

        [SerializeField] Transform attackPoint;
        private float attackRadius = 0.04f;
        private Quaternion Left, Right;

        private event Action AnimationTriggered;
        private event Action AttackTriggered;

        public bool attacked;

        private float deathtimer = 5;

        private float attackCooldown = 3.5f;
        private float lastAttackTime;

        private bool rangedEnemy, movingEnemy, simpleEnemy;
        private RangedAttack rangeAttack;
        private MovingEnemy moving;

        private int rewardPoints;

        void Start()
        {
            Left.eulerAngles = Vector3.zero;
            Right.eulerAngles = new Vector3(0, 180, 0);

            AnimationTriggered += EndOfAnimation;
            AttackTriggered += Attack;

            if (GetComponent<RangedAttack>() != null)
            {
                rangedEnemy = true;
                rangeAttack = GetComponent<RangedAttack>();
                rewardPoints = 50;
            }
            else if(GetComponent<MovingEnemy>() != null)
            {
                movingEnemy = true;
                moving = GetComponent<MovingEnemy>();
                rewardPoints = 100;
            }
            else
            {
                attackRadius = 0.2f;
                simpleEnemy = true;
                rewardPoints = 25;
            }

            hits = new Collider2D[2];
        }


        void Update()
        {
            if (reward != null)
            {
                if (IsFound())
                {
                    //agent.SetDestination(FirstHit.transform.position);
                    if (FirstHit.transform.position.x >= gameObject.transform.position.x)
                    {
                        gameObject.transform.rotation = Left;
                    }
                    else
                    {
                        gameObject.transform.rotation = Right;
                    }
                }

                if (Time.time > lastAttackTime + attackCooldown)
                {
                    SetAttackState();
                }
            }

        }

        private bool IsFound()
        {
            searchedCounts = Physics2D.OverlapCircleNonAlloc(checkPoint.position, checkRadius, hits, LayerMask.GetMask("Player"));

            return searchedCounts > 0;
        }

        private void SetAttackState()
        {
            attacked = true;
            animator.SetBool("IsAttack", true);
            lastAttackTime = Time.time;
            
        }


        private void Attack()
        {
            if (rangedEnemy)
            {
                rangeAttack.OnAttacked();
            }
            else
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, LayerMask.GetMask("Player", "Obstacle"));


                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].GetComponent<PlayerStateMachine>() != null)
                    {
                        colliders[i].GetComponent<PlayerStateMachine>().SetDeathState(true);
                    }
                    if (colliders[i].GetComponent<ObstacleLogic>() != null)
                    {
                        colliders[i].GetComponent<ObstacleLogic>().SetDestroyed(true);
                    }
                }
            }
        }

        private void EndOfAnimation()
        {
            if (SetDeathState(dead))
            {
                Destroy(gameObject);
            }
            if (attacked)
            {
                animator.SetBool("IsAttack", false);
                attacked = false;
            }
        }

        public bool SetDeathState(bool dead = false)
        {
            if (dead)
            {
                animator.SetBool("IsDead", true);
                this.dead = dead;
            }
            return dead;
        }

        private void TriggerAnimation() =>
            AnimationTriggered?.Invoke();

        private void TriggerAttack() =>
            AttackTriggered?.Invoke();

        private void OnDestroy()
        {
            AnimationTriggered -= EndOfAnimation;
            if(reward != null)
                GameManager.instance.Points(rewardPoints);
            AttackTriggered -= Attack;
        }

    }
}