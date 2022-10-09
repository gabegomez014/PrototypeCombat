using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy;
    public int maxEnemies = 5;
    public float spawnCoolDown = 2;

    [SerializeField]
    private Transform _enemyHolder;
    private float _currentCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentCoolDown <= 0 && _enemyHolder.childCount < maxEnemies) {
            Transform randomSpawnPoint = this.transform.GetChild(Random.Range(0,3));
            Instantiate(enemy, randomSpawnPoint.position, Quaternion.identity, _enemyHolder);
            _currentCoolDown = spawnCoolDown;
        } else {
            _currentCoolDown -= Time.deltaTime;
        }
        
    }
}
