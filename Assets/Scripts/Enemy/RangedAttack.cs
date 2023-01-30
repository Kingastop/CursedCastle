using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Enemy
{
    public class RangedAttack : MonoBehaviour
    {
        [SerializeField] GameObject knife;
        [SerializeField] Transform spawnPoint;
        [SerializeField] EnemyStateMachine enemy;

        private float speed = 0.015f;
        private Vector3 transition;
        private float direction;
        private float radius = 0.11f;

        private CircleCollider2D circleCollider;

        private GameObject projectile;

        private float fireTime;
        private float existingTime = 2;

       

        private void FixedUpdate()
        {
            if (projectile != null)
            {
                projectile.transform.position += transition;

                if (circleCollider.IsTouchingLayers())
                {
                    Destroy(projectile.GetComponent<CircleCollider2D>());
                    circleCollider = null;
                    
                    Collider2D[] hits = Physics2D.OverlapCircleAll(projectile.transform.position, radius, LayerMask.GetMask("Player", "Enemy", "Obstacle"));

                    foreach (Collider2D hit in hits)
                    {
                        if (hit.GetComponent<PlayerStateMachine>() != null)
                        {
                            hit.GetComponent<PlayerStateMachine>().SetDeathState(true);
                        }

                        if (hit.GetComponent<EnemyStateMachine>() != null)
                        {
                            hit.GetComponent<EnemyStateMachine>().SetDeathState(true);
                        }

                        if (hit.GetComponent<ObstacleLogic>() != null)
                        {
                            hit.GetComponent<ObstacleLogic>().SetDestroyed(true);
                        }
                    }

                    Destroy(projectile.gameObject);
                }



                if (Time.time > fireTime + existingTime)
                    Destroy(projectile.gameObject);
            }


        }


        public void OnAttacked()
        {
            fireTime = Time.time;
            projectile = Instantiate(knife);
            projectile.gameObject.transform.SetParent(null, false);
            projectile.transform.position = spawnPoint.position;

            direction = enemy.transform.rotation == Quaternion.Euler(0, 0, 0) ? 1 : -1;
            circleCollider = projectile.GetComponent<CircleCollider2D>();
            transition.Set(speed * direction, 0, 0);
            fireTime = Time.time;
        }
    }
}
