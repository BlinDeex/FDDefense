using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public (GameObject, IEnemyInterface) enemy;
    public float damage;
    public float speed;

    readonly string Tag = "BasicTurretProjectile";

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, enemy.Item1.transform.position, speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, enemy.Item1.transform.position) < 0.1f)
        {
            TakeDamage(enemy);
        }
        
        if(!enemy.Item1.activeSelf) // target is not active anymore return projectile, TODO: keep last projectile velocity
                                    // and scan if it collides with anything else before getting out of bounds
        {
            Debug.Log("Lost Target");
            gameObject.SetActive(false);
            DynamicObjectPooler.Instance.ReturnObject((DynamicObjectPooler.ObjectType.Projectile, Tag), gameObject);
        }
    }

    void TakeDamage((GameObject, IEnemyInterface) enemy) // projectile reached its target, damage it and return to pool
    {
        enemy.Item2.TakeBasicDamage(damage, -1);
        gameObject.SetActive(false);
        DynamicObjectPooler.Instance.ProjectilePools[Tag].Enqueue(gameObject);
    }
}
