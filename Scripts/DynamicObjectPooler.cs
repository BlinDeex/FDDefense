using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectPooler : MonoBehaviour
{

    public enum ObjectType
    {
        Enemy = 1,
        Projectile = 2,
        Effect = 3
    }

    public static DynamicObjectPooler Instance;

    public Dictionary<string, Queue<GameObject>> EnemyPools = new();

    public Dictionary<string, Queue<GameObject>> ProjectilePools = new();

    public Dictionary<string, Queue<GameObject>> EffectPools = new();

    public Queue<(string, GameObject)> EnemyCooldownQueue = new(); // temporary queue, prevents enemies to be reused instantly

    public int EnemyCooldownQueueTimer = 0;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject ProvideObject((ObjectType, string tag) identification, GameObject prefab)
    {
        return identification.Item1 switch
        {
            ObjectType.Enemy => ProvideEnemy(identification.tag, prefab),
            ObjectType.Projectile => ProvideProjectile(identification.tag, prefab),
            ObjectType.Effect => ProvideEffect(identification.tag, prefab),
            _ => null,
        };
    }

    public void ReturnObject((ObjectType, string tag) identification, GameObject obj)
    {
        switch(identification.Item1)
        {
            case ObjectType.Enemy:
                EnemyCooldownQueue.Enqueue((identification.tag, obj));
                break;
            case ObjectType.Projectile:
                ProjectilePools[identification.tag].Enqueue(obj);
                break;
            case ObjectType.Effect:
                EffectPools[identification.tag].Enqueue(obj);
                break;
            case 0:
                break;
        };
    }

    private void FixedUpdate()
    {
        if (EnemyCooldownQueue.Count != 0) EnemyCooldownQueueTimer++;
        

        if (EnemyCooldownQueueTimer > 4)
        {
            while (EnemyCooldownQueue.Count != 0)
            {
                (string, GameObject) enemy = EnemyCooldownQueue.Dequeue();
                EnemyPools[enemy.Item1].Enqueue(enemy.Item2);
            }
        }
    }



    GameObject ProvideEnemy(string tag, GameObject prefab)
    {
        if (EnemyPools.ContainsKey(tag))
        {
            if(EnemyPools[tag].Count > 0)
            {
                return EnemyPools[tag].Dequeue();
            }
            else
            {
                return Instantiate(prefab);
            }
        }
        else
        {
            EnemyPools.Add(tag, new());
            return Instantiate(prefab);
        }
    }

    GameObject ProvideProjectile(string tag, GameObject prefab)
    {
        if (ProjectilePools.ContainsKey(tag))
        {
            if (ProjectilePools[tag].Count > 0)
            {
                return ProjectilePools[tag].Dequeue();
            }
            else
            {
                return Instantiate(prefab);
            }
        }
        else
        {
            ProjectilePools.Add(tag, new());
            return Instantiate(prefab);
        }
    }

    GameObject ProvideEffect(string tag, GameObject prefab)
    {
        if (EffectPools.ContainsKey(tag))
        {
            if (EffectPools[tag].Count > 0)
            {
                return EffectPools[tag].Dequeue();
            }
            else
            {
                return Instantiate(prefab);
            }
        }
        else
        {
            EffectPools.Add(tag, new());
            return Instantiate(prefab);
        }
    }
}
