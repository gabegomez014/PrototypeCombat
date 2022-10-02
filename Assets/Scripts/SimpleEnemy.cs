using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public float rotationSpeed;
    public float movementSpeed;
    public float within_range;
    public float pushBackForce;

    private Rigidbody _rb;

    private Transform playerChar;

    void Start() {
        playerChar = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerChar != null)
        {
            var lookPos = playerChar.position - transform.position;
            lookPos.y = 0;
            var rotationAngle = Quaternion.LookRotation(lookPos); // we get the angle has to be rotated
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * rotationSpeed); // we rotate the rotationAngle 
        }

        float dist = Vector3.Distance(playerChar.position, transform.position);
        //check if it is within the range you set
        if(dist >= within_range){
            //move to target(player) 
            //Vector3 enemyPos = transform.position;
            //Vector3 playerPos = playerChar.transform.position;
            //playerPos.y = enemyPos.y;
            //transform.position = Vector3.MoveTowards(transform.position, playerPos, movementSpeed * Time.deltaTime);      
            _rb.AddForce(transform.forward * movementSpeed);
        }

        _rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.transform.tag == "Weapon")
        {
            ContactPoint contact = collision.GetContact(0);
            WeaponController weapon = collision.collider.transform.parent.GetComponent<WeaponController>();
            GameObject hitVFX = Instantiate(weapon.GetHitVFX(), contact.point, Quaternion.identity);
            Destroy(hitVFX, 3);
            _rb.velocity = new Vector3(0f, 0f, 0f);
            _rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            _rb.AddForce(-transform.forward * pushBackForce, ForceMode.VelocityChange);
        }
    }
}
