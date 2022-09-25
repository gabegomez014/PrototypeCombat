using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    [Header("Rotation speed")]
    public float speed_rot;
    [Header("Movement speed during jump")]
    public float speed_move;
    public bool isJump;
    Quaternion rot;
    bool isRun;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Rotate()
    {
        if (Input.GetKey(KeyCode.D))
        {            
            Move();            
            rot = Quaternion.LookRotation(Vector3.right);
        }

        
        else if (Input.GetKey(KeyCode.A))
        {            
            Move();
            rot = Quaternion.LookRotation(Vector3.left);
        }

        else if (Input.GetKey(KeyCode.W)) {
            Move();
            rot = Quaternion.LookRotation(Vector3.forward);
        }

        else if (Input.GetKey(KeyCode.S)) {
            Move();
            rot = Quaternion.LookRotation(Vector3.back);
        }

        else
        {            
            anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed_rot * Time.deltaTime);

    }

    void Move()
    {
        if (isJump)
        {            
            transform.position += transform.forward * speed_move * Time.deltaTime;            
            anim.SetBool("Run", false);
                anim.SetBool("Walk", false);

        }
        else
        {            
            anim.SetBool("Run", true);
                anim.SetBool("Walk", Input.GetKey(KeyCode.LeftShift));
        }
    }
}
