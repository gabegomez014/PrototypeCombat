using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public float rotationSpeed;
    public float acceleration;
    public float maxSpeed;
    public float within_range;
    public float pushBackForce;
    public float stunTime;
    public int maxHealth;
    public GameObject deathVFX;

    private Rigidbody _rb;

    private Transform _playerChar;

    private int _currentHealth;
    private bool _stunned;

    private float _currentStunTime;

    void Start() {
        _playerChar = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        _rb = GetComponent<Rigidbody>();
        _currentHealth = maxHealth;
        _currentStunTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stunned) {
            if (_playerChar != null)
            {
                var lookPos = _playerChar.position - transform.position;
                lookPos.y = 0;
                var rotationAngle = Quaternion.LookRotation(lookPos); // we get the angle has to be rotated
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * rotationSpeed); // we rotate the rotationAngle 
            }

            float dist = Vector3.Distance(_playerChar.position, transform.position);
            //check if it is within the range you set
            if(dist >= within_range && _rb.velocity.magnitude < maxSpeed){    
                _rb.AddForce(transform.forward * acceleration);
            }
        } else if (_currentStunTime > 0) {
            _currentStunTime -= Time.deltaTime;
        } else if (_currentStunTime <= 0) {
            _stunned = false;
        }

        if (_currentHealth == 0 || this.transform.position.y < -1) {
            Death();
        }

        _rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    private void Death() {
        GameObject fx = Instantiate(deathVFX, this.transform.position, Quaternion.identity);
        Destroy(fx, 3);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.tag == "Weapon")
        {
            Hit(collision);
        }
    }

    private void Hit(Collision weaponCollider) {
        if (!_stunned) {
            _stunned = true;
            _currentStunTime = stunTime;
            // StartCoroutine(StunLock());
            _rb.velocity = new Vector3(0f, 0f, 0f);
            _rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            _rb.AddForce(-transform.forward * pushBackForce, ForceMode.VelocityChange);
        }
        ContactPoint contact = weaponCollider.GetContact(0);
        WeaponController weapon = weaponCollider.collider.transform.parent.GetComponent<WeaponController>();
        GameObject hitVFX = Instantiate(weapon.GetHitVFX(), contact.point, Quaternion.identity);
        Destroy(hitVFX, 3);

        _currentHealth -= weapon.GetDamage();
    }
}
