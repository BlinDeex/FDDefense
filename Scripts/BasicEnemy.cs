using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemyInterface
{
    public float CurrentHealth { get; set; }
    
    [field: SerializeField]
    public float FutureHealth { get; set; } // ENEMY STAYS ALIVE WITH FUTUREHEALTH IN NEGATIVES SHOULD BE IMPOSSIBLE
    public int Money { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }
    public string PoolTag { get; set; }
    public int DeathCount { get; set; }
    public SpriteRenderer SR { get; set; }

    Vector3 Destination;

    new readonly string tag = "BasicEnemy";

    public void ReloadStats(float health, float damage, int moneyBounty, float speed) // set stats upon respawning
    {
        CurrentHealth = health;
        FutureHealth = health; 
        Damage = damage;
        Speed = speed;
        SR.color = Color.white;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, Destination) < 0.5f) ReturnObject(); // reached finish, return object
        if(FutureHealth < 0)
        {
            SR.color = Color.red;
        }
    }

    private void Awake()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
        DeathCount = 0;
        PoolTag = tag;
        Destination = new Vector3(0 + Random.Range(-2, 2), -3, 1);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
    }

    void ReturnObject()
    {
        DeathCount++; // enemy has died once more
        gameObject.SetActive(false);
        DynamicObjectPooler.Instance.ReturnObject((DynamicObjectPooler.ObjectType.Enemy, tag), gameObject);
    }

    public void TakeBasicDamage(float damage, int damageType)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            ReturnObject();
        }
    }
}
