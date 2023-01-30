using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Enemy;
using UnityEngine.UI;

public class EndingOrder : MonoBehaviour
{
    [SerializeField] GameObject KnightGirl, Player, Goblin1, Mushroom, Goblin2;
    [SerializeField] Animator gAnimator, pAnimator, gobAnim1, mushAnim, gobAnim2;

    [SerializeField] MenuButton mBtn;

    [SerializeField] Transform pTransform;
    [SerializeField] Transform enemyLeft, enemyRight;

    private GameObject enemy, nextEnemy;
    private Transform enemyTransform;
    private Animator enemyAnim;

    private PlayerStateMachine playerState;

    private Vector3 pPosition = new Vector3(0.0006f, 0, 0);
    private bool pMoving;
    private bool enemyAppear = false;

    private float cutsceneSpeed = 0.0006f;

    private int iter = 0;
    private float timer = 1;
    private float lastTime = 0;

    private bool dead = false;
    private bool end = false;

    [SerializeField] Canvas canvas;
    private float alpha = 0;
    private bool ending;
    private bool endimage;
    Image shade;

    private void Start()
    {

        KnightGirl.SetActive(false);
        Goblin1.SetActive(false);
        Mushroom.SetActive(false);
        Goblin2.SetActive(false);

        enemy = Goblin1;
        enemyTransform = enemyRight;
        enemyAnim = gobAnim1;

        pMoving = true;
        playerState = Player.GetComponent<PlayerStateMachine>();

        if (Application.platform == RuntimePlatform.Android)
        {
            cutsceneSpeed *= 100;
        }
        else
        {

        }
        pPosition = new Vector3(cutsceneSpeed, 0, 0);
    }

    private void Update()
    {
        if (!end)
        {
            if (MoveTo(Player, pTransform, pAnimator, pPosition))
            {

            }
            else
            {
                Player.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                Player.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                if (pMoving)
                    lastTime = Time.time;
                pMoving = false;

            }

            if (Time.time > lastTime + timer)
            {

                if (!pMoving && !enemyAppear)
                {
                    Appearing(enemy, enemyTransform);
                }

            }

            if (!pMoving && enemyAppear)
            {
                if (enemyTransform == enemyRight)
                {
                    Player.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    Player.transform.rotation = Quaternion.Euler(0, 0, 0);
                }


                if (!dead)
                {
                    playerState.SetAttackState();
                    Disappear();
                    if (iter < 3)
                        enemy.GetComponent<EnemyStateMachine>().SetDeathState(true);
                    else
                    {
                        enemyAnim.SetBool("IsDead", true);
                        end = true;
                        pMoving = true;
                    }
                }

            }

            if (enemyAppear)
            {
                if (dead)
                {
                    NextIteration();
                }
                lastTime = Time.time + 1;
            }
        }
        else
        {
            if (Time.time > lastTime + timer)
            {
                if (MoveBack(Player, enemyLeft, pAnimator, -pPosition))
                {

                }
                else
                {
                    Player.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                    Player.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    if (pMoving)
                        lastTime = Time.time;
                    pMoving = false;

                }

                if (Time.time > lastTime + timer)
                {

                    if (!pMoving && !endimage)
                    {
                        pAnimator.SetFloat("MoveX", 0);
                        pAnimator.SetBool("IsCrouch", true);

                        if (!ending)
                        {
                            lastTime = Time.time;
                            canvas.gameObject.SetActive(true);
                            shade = canvas.GetComponent<Image>();
                            ending = true;
                        }

                    }

                }
            }
        }


        if (ending)
        {
            if (Time.time > lastTime + timer)
            {

                if (alpha > 1)
                {
                    lastTime = Time.time;
                    timer = 5;
                    ending = false;
                    endimage = true;
                }
                else
                {
                    alpha += cutsceneSpeed * 2.5f;
                    shade.color = new Color(1, 1, 1, alpha);
                }


            }



        }

        if (endimage)
        {
            if (Time.time > lastTime + timer)
            {
                if(alpha > 1)
                    mBtn.NextScene(1);
                alpha = 0;
            }
        }


    }

    private bool MoveTo(GameObject person, Transform transform, Animator animator, Vector3 direction)
    {

        if (person.transform.position.x < transform.position.x)
        {
            person.transform.position += direction;
            animator.SetFloat("MoveX", 1);
        }
        else
        {
            animator.SetFloat("MoveX", 0);
        }
        return person.transform.position.x < transform.position.x;
    }

    private bool MoveBack(GameObject person, Transform transform, Animator animator, Vector3 direction)
    {
        if (person.transform.position.x > transform.position.x)
        {
            person.transform.position += direction;
            animator.SetFloat("MoveX", 1);
        }
        else
        {
            animator.SetFloat("MoveX", 0);
        }
        return person.transform.position.x > transform.position.x;
    }

    private void Appearing(GameObject person, Transform perstransform)
    {
        person.transform.position = perstransform.position;
        if (enemyLeft == perstransform)
        {
            person.transform.rotation = Quaternion.Euler(0, 0, 0);
            enemyTransform = enemyRight;
        }

        if (enemyRight == perstransform)
        {
            person.transform.rotation = Quaternion.Euler(0, 180, 0);
            enemyTransform = enemyLeft;
        }

        person.SetActive(true);
        enemyAppear = true;
    }

    private void Disappear()
    {
        dead = true;
    }

    private void NextIteration()
    {
        dead = false;
        enemyAppear = false;

        switch (iter)
        {
            case 0:
                enemy = Mushroom;
                enemyAnim = mushAnim;
                iter++;
                break;

            case 1:
                enemy = Goblin2;
                enemyAnim = gobAnim2;
                iter++;
                break;

            case 2:
                enemy = KnightGirl;
                enemyAnim = gAnimator;
                iter++;
                break;
        }

    }
}
