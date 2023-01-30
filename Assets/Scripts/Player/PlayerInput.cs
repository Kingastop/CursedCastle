
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerStateMachine stateMachine;

        [SerializeField] private ButtonInput leftBtn, rightBtn, jumpBtn, crouchBtn;

        private Rigidbody2D rbody;

        private bool jumpdown = false;
        private float jtiming;
        private float jForce;
        private float jumpCooldown = 1;
        private float lastTimeJumped = 0;
        private bool crouching;

        private float baseSpeed = 0.0f;
        private float speedMultiplier = 1.1f;

        private bool mLeftPlayerMoving = false;
        private bool mRightPlayerMoving = false;
        private int mDirection = 1;

        private int mAttacking = 0;
        private int mJumpdown = 0;
        private int mCrouching = 0;

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (!stateMachine.disabledInput)
            {
                leftBtn.Checked();
                rightBtn.Checked();
                jumpBtn.Checked();
                crouchBtn.Checked();
                



                if (IsAttacking())
                {
                    stateMachine.SetAttackState();
                    MIntAttacking();
                }

                 
                if (IsJumping())
                {
                    if(!jumpdown)
                        stateMachine.SetJumpState(true);
                    jumpdown = true;
                    
                    jtiming = 0;
                    
                }


                if (jumpdown)
                {
                    jtiming += Time.deltaTime;
                    jForce = Mathf.Sqrt(jtiming * -2 * (Physics2D.gravity.y * rbody.gravityScale));
                }

                if (IsJumpReleased())
                {

                    /*if (jForce < stateMachine.jumpForce)
                    {
                        rbody.AddForce(new Vector2(0, -jForce), ForceMode2D.Impulse);
                    }*/
                    jumpdown = false;
                    lastTimeJumped = Time.time;
                    MFall();
                }

                if (IsCrouching())
                {
                    crouching = true;
                }

                if (crouching)
                {
                    stateMachine.SetCrouchState(mDirection == 0 ? (SimpleInputDirection()) : (baseSpeed));
                }
                else
                {
                    stateMachine.SetStandingUpState();
                }


                if (IsCrouchRelease())
                {
                    stateMachine.SetMove(mDirection == 0 ? (SimpleInputDirection()) : (baseSpeed));

                    crouching = false;
                    MIntCrouching();
                }

                if (mDirection != 0)
                {
                    if (baseSpeed >= 1 || baseSpeed <= -1)
                    {
                        baseSpeed = mDirection;
                    }
                    else
                    {
                        baseSpeed *= speedMultiplier;
                    }

                    stateMachine.SetMove(baseSpeed);
                }
                else
                {
                    stateMachine.SetMove(SimpleInputDirection());
                    baseSpeed = 0.0f;
                }

            }
        }

        private bool IsAttacking() =>
            Input.GetKey(GameManager.instance.attack) || MIntAttacking(mAttacking) > 0;

        public float SimpleInputDirection()
        {
            return Input.GetAxis("Horizontal");
        }

        private bool IsJumping() =>
            Input.GetKeyDown(GameManager.instance.jump) || MIntJumped(mJumpdown) > 0;

        private bool IsJumpReleased() =>
            Input.GetKeyUp(GameManager.instance.jump) || MIntJumped(mJumpdown) == 0;

        private bool IsCrouching() =>
            Input.GetKeyDown(GameManager.instance.crouch) || MIntCrouching(mCrouching) > 0;

        private bool IsCrouchRelease() =>
            Input.GetKeyUp(GameManager.instance.crouch) || MIntCrouching(mCrouching) ==0;


        //_____________________________________________________Mobile_Functions_________________________________________________________
        public void MRightInputDirection()
        {
            mRightPlayerMoving = true;
            baseSpeed += 0.0075f;
            mDirection = 1;
        }

        public void MLeftInputDirection()
        {
            mLeftPlayerMoving = true;
            baseSpeed -= 0.0075f;
            mDirection = -1;
        }

        public void MRightStopped()
        {
            if (!mLeftPlayerMoving)
            {
                mLeftPlayerMoving = false;
                mRightPlayerMoving = false;
                mDirection = 0;
                baseSpeed *= mDirection;
            }
        }

        public void MLeftStopped()
        {
            if (!mRightPlayerMoving)
            {
                mRightPlayerMoving = false;
                mLeftPlayerMoving = false;
                mDirection = 0;
                baseSpeed *= mDirection;
            }

        }

        public void MJumped()
        {
            
            MIntJumped(1);
        }

        private int MIntJumped(int j = -1) =>
            mJumpdown = j;

        public void MFall()
        {
            if(mJumpdown > 0)
                MIntJumped(0);
        }

        public void MCrouching()
        {
            MIntCrouching(1);
        }

        private int MIntCrouching(int c = -1) =>
            mCrouching = c;

        public void MStanding()
        {
            if(mCrouching > 0)
                MIntCrouching(0);
        }

        public void MAttack()
        {
            MIntAttacking(1);
        }

        private int MIntAttacking(int a = 0) =>
            mAttacking = a;

    }
}