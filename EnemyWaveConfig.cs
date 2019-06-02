using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy WaveConfig")]
public class EnemyWaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float moveSpeed;
    [SerializeField] int numberOfEnemies;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    /*crear una lista "wayPoints" y agregar todas las posiciones en el pathprefab a ella
    para ser entregada*/  
    public List<Transform> GetWayPoints()
    {
        var wayPoints = new List<Transform>();
        foreach(Transform child in pathPrefab.transform)
        {
            wayPoints.Add(child);
        }
        return wayPoints;
    }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetMoveSpeed() { return moveSpeed; }
    public int GetNumberOfEnemies() { return numberOfEnemies; }
}
