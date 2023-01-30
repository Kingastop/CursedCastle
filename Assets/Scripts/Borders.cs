using System;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Enemy;

public class Borders : MonoBehaviour
{
    [SerializeField] BoxCollider2D collider;

    Collider2D[] hit;

    void Start()
    {
        hit = new Collider2D[10];
    }

    private void Update()
    {
        if (hit != null)
        {
            HitResult();
        }
    }

    private int Hit() =>
        Physics2D.OverlapBoxNonAlloc(collider.transform.position, collider.bounds.size, 0, hit, LayerMask.GetMask("Player", "Enemy", "Obstacle"));

    private void HitResult()
    {
        for (int i = 0; i < Hit(); i++)
        {
            if (hit[i].GetComponent<PlayerStateMachine>() != null)
            {
                hit[i].GetComponent<PlayerStateMachine>().SetDeathState(true);
            }

            if (hit[i].GetComponent<EnemyStateMachine>() != null)
            {
                hit[i].GetComponent<EnemyStateMachine>().SetDeathState(true);
            }

            if (hit[i].GetComponent<ObstacleLogic>() != null)
            {
                hit[i].GetComponent<ObstacleLogic>().SetDestroyed(true);
            }
        }

        Array.Clear(hit, 0, hit.Length);
    }

}
