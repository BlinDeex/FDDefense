using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretLogic : MonoBehaviour
{
    public float range = 2f;

    public float damage = 60f;

    public float speed = 5f;

    public GameObject projectilePrefab;

    [SerializeField]
    float energy;

    public float shootCost;

    public CircleCollider2D rangeCollider;

    public Rigidbody2D rb;

    readonly Queue<(GameObject, IEnemyInterface, int)> enemyQueue = new();

    readonly string Tag = "BasicTurretProjectile";

    private void Awake()
    {
        rangeCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IEnemyInterface enemyInterface)) // if object is enemy and will not die to travelling
                                                                          // projectiles, enqueue into targets
        {
            if(enemyInterface.FutureHealth > 0)
            {
                enemyQueue.Enqueue((collision.gameObject, enemyInterface, enemyInterface.DeathCount));
            }
        }
    }

    private void FixedUpdate()
    {
        if (energy <= shootCost) energy++;

        if (energy >= shootCost) Shoot();
    }

    void Shoot()
    {
            (GameObject, IEnemyInterface, int)? target = FindTarget();

            if (target == null) return; // no target found

            while(target.Value.Item2.FutureHealth > 0 && energy > shootCost) // as long as turret has enough energy
                                                                             // and enemy doesnt have enough projectiles
                                                                             // flying at him
            {
                energy -= shootCost;

                GameObject projectile = DynamicObjectPooler.Instance.ProvideObject(
                    (DynamicObjectPooler.ObjectType.Projectile, Tag), projectilePrefab); // get projectile from pool


                BasicProjectile BP = projectile.GetComponent<BasicProjectile>(); // get its class and set appropriate values
                BP.damage = damage;
                BP.speed = speed;
                BP.enemy = (target.Value.Item1, target.Value.Item2);
                projectile.transform.position = transform.position;

                projectile.SetActive(true); // enable projectile and set enemy FutureHealth to what it will be when it
                                            // reaches target
                if(projectile.activeSelf) target.Value.Item2.FutureHealth -= damage;
            }
    }

    (GameObject, IEnemyInterface, int)? FindTarget()
    {
        (GameObject, IEnemyInterface, int)? target = null;

        while(!target.HasValue && enemyQueue.Count != 0) // as long as target is null and there are potential targets in queue
        {
            target = enemyQueue.Dequeue();
            if(target.Value.Item2.FutureHealth > 0 
                && target.Value.Item2.DeathCount == target.Value.Item3) // if target will not die to travelling projectiles
                                                                        // and it has not died already before turret got to it in queue
            {
                return ((GameObject, IEnemyInterface, int)) target;
            }
        }
        return null; // no target found
    }
}
