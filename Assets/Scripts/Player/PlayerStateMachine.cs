using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using Enemy;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] GameObject gameOverMenu;
        [SerializeField] GameObject inputUi;

        [SerializeField] private Transform attackPoint;
        private float attackRadius = 0.08f;

        [SerializeField] private LayerMask m_WhatIsGround;
        [SerializeField] private Transform m_GroundCheck;
        [SerializeField] private Transform m_CeilingCheck;

        private Rigidbody2D rbody;
        public float jumpForce = 100f;
        private float jumpHeight = 1f;

        private bool m_Grounded;
        private float k_GroundedRadius = .1f;

        public event Action AttackTriggered;
        public event Action AnimationTriggered;
        private bool standing;
        private bool slideEnded;
        private bool attacking;

        private Quaternion Left, Right;

        public UnityEvent OnLandEvent;

        public bool disabledInput = false;

        private bool dead;

        void Awake()
        {
            Left.eulerAngles = Vector3.zero;
            Right.eulerAngles = new Vector3(0, 180, 0);
            rbody = GetComponent<Rigidbody2D>();

            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rbody.gravityScale));
            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();

            AttackTriggered += Attack;
            AnimationTriggered += EndOfAnimation;
        }


        private void FixedUpdate()
        {
            bool wasGrounded = m_Grounded;
            m_Grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    if (!wasGrounded)
                    {
                        OnLandEvent.Invoke();
                    }
                }
            }
        }

        public void SetAttackState()
        {
            if (!attacking)
            {
                attacking = true;
                animator.SetInteger("AttackNum", UnityEngine.Random.Range(0, 2));
                animator.SetBool("IsAttack", true);
            }
        }

        public void SetMove(float direction)
        {
            if (!attacking)
            {
                if (direction == 0)
                    SetDefaultState();
                else
                    SetRunState(direction);
            }
        }

        public void SetRunState(float value)
        {
            if(standing)
                animator.SetFloat("MoveX", value);


            if (!slideEnded)
            {
                Vector3 pos = transform.position;
                if (value > 0)
                {

                    pos.x += value * Time.deltaTime;

                    transform.position = pos;
                    gameObject.transform.rotation = Left;
                }
                else if (value < 0)
                {

                    pos.x += value * Time.deltaTime;

                    transform.position = pos;
                    gameObject.transform.rotation = Right;
                }
            }
        }

        public void SetJumpState(bool jump)
        {
            if (m_Grounded && jump)
            {
                standing = false;
                animator.SetBool("IsJump", true);
                m_Grounded = false;
                rbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
        }

        public void OnLanding()
        {
            animator.SetBool("IsJump", false);
            SetDefaultState();
        }

        public void SetCrouchState(float value)
        {
            animator.SetBool("IsCrouch", true);
            standing = false;
        }

        public void SetStandingUpState()
        {
            animator.SetBool("IsCrouch", false);
            animator.SetFloat("MoveX", 0);
            standing = true;
            slideEnded = false;
        }

        private void Attack()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, LayerMask.GetMask("Enemy", "Obstacle"));

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<EnemyStateMachine>() != null)
                {
                    colliders[i].GetComponent<EnemyStateMachine>().SetDeathState(true);
                }
                if(colliders[i].GetComponent<ObstacleLogic>() != null)
                {
                    colliders[i].GetComponent<ObstacleLogic>().SetDestroyed(true);
                }
            }
        }
        
        private void EndOfAnimation()
        {
            if (attacking)
            {
                AttackEnds(attacking);
                attacking = false;
                animator.SetBool("IsAttack", false);
            }
            if (!standing && m_Grounded)
            {
                slideEnded = true;
                animator.SetFloat("MoveX", 0);
            }
            if (SetDeathState(dead))
            {
                GameManager.instance.SetRecord();
                inputUi.SetActive(false);
                gameOverMenu.SetActive(true);
                inputUi.GetComponent<MenuButton>().returnToMenuBtn.gameObject.transform.SetParent(gameOverMenu.transform, false);
                DataPersistanceManager.Instance.SaveGame();
                //Menu + dramatic music
            }
        }

        public void SetDefaultState()
        {
            standing = true;
        }


        public void TriggeredAnimation() =>
            AnimationTriggered?.Invoke();

        public void TriggeredAttack() =>
            AttackTriggered?.Invoke();

        private void OnDestroy()
        {
            AttackTriggered -= Attack;
            AnimationTriggered -= EndOfAnimation;
        }

        public bool SetDeathState(bool dead = false)
        {
            if (dead)
            {
                animator.SetBool("IsDead", true);
                disabledInput = true;
                this.dead = dead;
            }
            return dead;
        }

        public bool AttackEnds(bool a = false)
        {
            
            return a;
        }

    }

}
