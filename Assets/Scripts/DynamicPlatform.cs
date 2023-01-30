using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatform : MonoBehaviour
{
    [SerializeField] BoxCollider2D collider;
    [SerializeField] BoxCollider2D upperCollider;
    [SerializeField] BoxCollider2D lowerCollider;

    private ContactFilter2D filter2D = new ContactFilter2D();

    private Collider2D[] hitResults;

    private void Start()
    {
        filter2D.SetLayerMask(LayerMask.GetMask("Obstacle", "Player", "Enemy"));
        hitResults = new Collider2D[2];
    }
    private void Update()
    {
        if (upperCollider.OverlapCollider(filter2D, hitResults) != 0 && lowerCollider.OverlapCollider(filter2D, hitResults) != 0)
        {
            collider.isTrigger = true;
        }
        else
        {
            if (upperCollider.OverlapCollider(filter2D, hitResults) != 0)
            {
                collider.isTrigger = false;
            }
            else if (lowerCollider.OverlapCollider(filter2D, hitResults) != 0)
            {
                collider.isTrigger = true;
            }
            else
            {
                collider.isTrigger = false;
            }
            
        }
        Array.Clear(hitResults, 0, hitResults.Length);
    }


    /*private Collider2D[] lowerHit;
    private Collider2D[] upperHit;

    private bool low, high;

    void Start()
    {
        lowerHit = new Collider2D[3];
        upperHit = new Collider2D[3];
    }

    private int Hit(Collider2D[] whichHit) =>
        Physics2D.OverlapBoxNonAlloc(new Vector2(collider.transform.position.x, (whichHit == lowerHit) ? (collider.transform.position.y - collider.bounds.size.y * 5f) : (collider.transform.position.y + collider.bounds.size.y * 5f)), new Vector2(collider.bounds.size.x, collider.bounds.size.y * 3), 0, whichHit, LayerMask.GetMask("Player", "Enemy", "Obstacle"));

    private void HitResult()
    {

        for (int i = 0; i < Hit(lowerHit); i++)
        {
            if (lowerHit[i] != null)
            {

                low = true;
            }
            else
            {

                low = false;
            }

        }

        Array.Clear(lowerHit, 0, lowerHit.Length);


        for (int i = 0; i < Hit(upperHit); i++)
        {
            if (upperHit[i] != null)
            {

                high = true;
            }
            else
            {

                high = false;
            }
        }

        Array.Clear(upperHit, 0, upperHit.Length);


        Debug.Log(this.gameObject.name + " high = " + high + ", low = " + low);
        if (high && low)
        {
            collider.isTrigger = true;
        }
        else if (high)
        {
            collider.isTrigger = false;
        }
        else
        {
            collider.isTrigger = true;
        }
    }

    void Update()
    {
        if (lowerHit != null || upperHit != null)
        {
            HitResult();
        }
    }*/



}
