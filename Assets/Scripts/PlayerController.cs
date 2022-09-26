using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    [Header("Rotation speed")]
    public float speed_rot;
    [Header("Movement speed during jump")]
    public float speed_move;
    [Header("Time available for combo")]
    public int term;

    public bool isJump;
    Quaternion rot;
    bool isRun;

    int clickCount;
    float timer;
    bool isTimer;

    Vector3 startingMousePos;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (!isJump) {
            Jump();
            Attack();
        }
    }

    void Rotate()
    {
        if (Input.GetMouseButtonDown(1)) {
            startingMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1)) {
            Vector3 currentMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            Vector3 targetDir = currentMousePos - startingMousePos;

            Vector3 currentEuler = transform.rotation.eulerAngles;

            currentEuler.y = currentEuler.y - ( targetDir.x * speed_rot );

            rot = Quaternion.Euler(currentEuler);
        }

        if (Input.GetKey(KeyCode.D)) {
            Vector3 currentEuler = transform.rotation.eulerAngles;

            currentEuler.y = currentEuler.y + ( speed_rot / 2 );

            rot = Quaternion.Euler(currentEuler);
        }

        if (Input.GetKey(KeyCode.A)) {
            Vector3 currentEuler = transform.rotation.eulerAngles;

            currentEuler.y = currentEuler.y - ( speed_rot / 2);

            rot = Quaternion.Euler(currentEuler);
        }
        
        // if (Input.GetKey(KeyCode.D))
        // {            
        //     Move();            
        //     rot = Quaternion.LookRotation(Vector3.right);
        // }

        
        // else if (Input.GetKey(KeyCode.A))
        // {            
        //     Move();
        //     rot = Quaternion.LookRotation(Vector3.left);
        // }

        // else if (Input.GetKey(KeyCode.W)) {
        //     Move();
        //     rot = Quaternion.LookRotation(Vector3.forward);
        // }

        // else if (Input.GetKey(KeyCode.S)) {
        //     Move();
        //     rot = Quaternion.LookRotation(Vector3.back);
        // }

        // else
        // {            
        //     anim.SetBool("Run", false);
        //         anim.SetBool("Walk", false);
        // }

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed_rot * Time.deltaTime);

    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W)) {
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

        else if (Input.GetKey(KeyCode.S)) {
            if (isJump)
            {            
                transform.position -= transform.forward * ( speed_move / 2 ) * Time.deltaTime;            
                anim.SetBool("WalkBack", false);
                anim.SetBool("Walk", false);

            }
            else
            {            
                anim.SetBool("WalkBack", true);
            }
        }

        else {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
            anim.SetBool("WalkBack", false);
        }
    }

    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {            
            anim.SetBool("Block", false);
            anim.SetBool("Crouch", false);
            anim.SetTrigger("Jump");

            isJump = true;
        }
    }

    void JumpEnd()
    {
        isJump = false;
    }

    void Attack()
    {
        
        if (isTimer)
        {
            timer += Time.deltaTime;
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            switch (clickCount)
            {
                
                case 0:
                    
                    anim.SetTrigger("Attack1");
                    
                    isTimer = true;
                    
                    clickCount++;
                    break;

                
                case 1:
                    
                    if (timer <= term)
                    {                        
                        anim.SetTrigger("Attack2");
                        
                        clickCount++;
                    }

                    
                    else
                    {                        
                        anim.SetTrigger("Attack1");
                        
                        clickCount = 1;
                    }

                    
                    timer = 0;
                    break;

                
                case 2:
                    
                    if (timer <= term)
                    {                        
                        anim.SetTrigger("Attack3");
                        
                        clickCount = 0;
                        
                        isTimer = false;
                    }

                    
                    else
                    {                        
                        anim.SetTrigger("Attack1");
                        
                        clickCount = 1;
                    }
                
                    timer = 0;
                    break;
            }
        }
    }
}
