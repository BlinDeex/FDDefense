using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyInterface
{
    float CurrentHealth { get; set; }
    float FutureHealth { get; set; }

    string PoolTag { get; set; }

    int DeathCount { get; set; }

    SpriteRenderer SR { get; set; }

    int Money { get; set; }

    float Damage { get; set; }

    float Speed { get; set; }

    void ReloadStats(float health, float damage, int moneyBounty, float speed);


    void TakeBasicDamage(float damage, int damageType);
}
