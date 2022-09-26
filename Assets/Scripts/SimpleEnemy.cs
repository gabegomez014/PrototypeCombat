using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public float rotationSpeed;
    public float movementSpeed;
    public float within_range;

    private Transform playerChar;

    void Start() {
        playerChar = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(playerChar.position, transform.position);
        //check if it is within the range you set
        if(dist >= within_range){
            //move to target(player) 
            Vector3 enemyPos = transform.position;
            Vector3 playerPos = playerChar.transform.position;
            playerPos.y = enemyPos.y;
            transform.position = Vector3.MoveTowards(transform.position, playerPos, movementSpeed * Time.deltaTime);      
        }
    }
}
