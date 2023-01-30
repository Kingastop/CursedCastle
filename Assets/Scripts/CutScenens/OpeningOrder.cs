using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningOrder : MonoBehaviour
{
    [SerializeField] GameObject KnightGirl, EvilWizard, Player;
    [SerializeField] Animator gAnimator, wAnimator, pAnimator;

    [SerializeField] MenuButton mBtn;

    [SerializeField] Transform[] pTransforms, gTransforms, wTransforms;

    private Vector3 pPosition = new Vector3(0.003f, 0, 0), gPosition = new Vector3(0.003f, 0, 0), wPosition = new Vector3(0.003f, 0, 0);
    private bool pMoving, gMoving;
    private bool wAppear;

    private float cutsceneSpeed = 0.0006f;

    private float timer = 2;
    private float lastTime = 0;
    private bool standing;
    private bool taking;
    private bool runningW;
    private bool end;

    private void Start()
    {
        EvilWizard.transform.position = wTransforms[0].position;
        EvilWizard.SetActive(false);
        pMoving = true;
        gMoving = true;

        if (Application.platform == RuntimePlatform.Android)
        {
            cutsceneSpeed *= 100;
        }
        else
        {
            
        }
        pPosition = new Vector3(cutsceneSpeed, 0, 0);
        gPosition = new Vector3(cutsceneSpeed, 0, 0);
        wPosition = new Vector3(cutsceneSpeed, 0, 0);
    }

    private void Update()
    {
        if (MoveTo(Player, pTransforms[0], pAnimator, pPosition))
        {

        }
        else
        {
            Player.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            Player.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            pMoving = false;
        }

        if (MoveTo(KnightGirl, gTransforms[0], gAnimator, gPosition))
        {
            standing = true;
        }
        else
        {
            gMoving = false;
            KnightGirl.transform.rotation = Quaternion.Euler(0, 180, 0);
            gAnimator.SetFloat("MoveX", 0);
            KnightGirl.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        }

        if (!pMoving && !gMoving)
        {
            if (standing)
            {
                lastTime = Time.time;
                standing = false;
                EvilWizard.SetActive(true);
            }

            if (Time.time > lastTime + timer && !runningW)
            {

                wAppear = true;
                gPosition.Set(0, cutsceneSpeed * 0.3f, 0);
            }
        }

        if (wAppear)
        {
            if (FlyingTo(KnightGirl, gTransforms[1], gAnimator, gPosition))
            {

            }
            else
            {

                wAnimator.SetBool("IsAttacking", true);
                if (EvilWizard.GetComponent<WizardAttackState>().attacked && wAppear)
                {
                    wAppear = false;
                    taking = true;
                }
            }
        }

        if (taking)
        {

            KnightGirl.SetActive(false);
            EvilWizard.SetActive(false);

            runningW = true;
            taking = false;
            standing = false;
            end = true;

        }

        if (runningW)
        {

            if (MoveTo(Player, pTransforms[1], pAnimator, pPosition))
            {

            }
            else
            {
                
                if (end)
                {
                    mBtn.NextScene(1);
                    end = false;
                }


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

    private bool FlyingTo(GameObject person, Transform transform, Animator animator, Vector3 direction)
    {
        gAnimator.SetBool("IsFalling", true);
        if (person.transform.position.y < transform.position.y)
        {

            person.transform.position += direction;
        }

        return person.transform.position.y < transform.position.y;
    }


}
