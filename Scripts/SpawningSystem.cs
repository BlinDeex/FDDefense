using UnityEngine;

public class SpawningSystem : MonoBehaviour
{
    public float energy = 0;
    public float spawnWeight = 1;

    public GameObject basicSquare;
    public GameObject spawnPoint;
    readonly string basicEnemy = "BasicEnemy";
    void FixedUpdate()
    {
        SpawnSequence();
    }

    public virtual void SpawnSequence()
    {
        energy++;
        while (energy >= spawnWeight)
        {
            GameObject obj =
                DynamicObjectPooler.Instance.ProvideObject((DynamicObjectPooler.ObjectType.Enemy, basicEnemy), basicSquare);

            Vector2 spawnLocation = new(
                spawnPoint.transform.position.x + Random.Range(-2f, 2f),
                spawnPoint.transform.position.y + Random.Range(-1, 1));
            obj.transform.position = spawnLocation;
            obj.GetComponent<IEnemyInterface>().ReloadStats(100, 20, 50, 1);
            obj.SetActive(true);
            

            energy -= spawnWeight;
        }
    }
}
