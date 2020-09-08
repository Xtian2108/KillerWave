using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SOActorModel actorModel;
    [SerializeField] private float spawnRate;
    [SerializeField] [Range(0, 10)] private int quantity;
    private GameObject enemies;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        enemies = GameObject.Find("_Enemies");
        StartCoroutine(FireEnemy(quantity, spawnRate));
    }

    private IEnumerator FireEnemy(int quantity, float spawnRate)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemyUnit = CreateEnemy();
            enemyUnit.gameObject.transform.SetParent(this.transform);
            enemyUnit.transform.position = transform.position;
            yield return new WaitForSeconds(spawnRate);
        }
        yield return null;
    }

    private GameObject CreateEnemy()
    {
        GameObject enemy = GameObject.Instantiate(actorModel.actor);
        enemy.GetComponent<IActorTemplate>().ActorStats(actorModel);
        enemy.name = actorModel.actorName.ToString();
        return enemy;
    }
}
