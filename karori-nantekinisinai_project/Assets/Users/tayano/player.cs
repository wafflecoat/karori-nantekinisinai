
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player: MonoBehaviour
{
    private bool jumpNow;
    public float jumpPwer;
    public Rigidbody rb;
    public Vector3 moving, latestPos;
    public float speed;
    public int force_variable;
    private float player_force = 1;
    [SerializeField] private float power = 1;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = 5;
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player2"))
        {
            Vector3 myPos = transform.position;
            Vector3 otherPos = other.transform.position;
            myPos.y = 0;
            otherPos.y = 0;

            Vector3 force = myPos - otherPos * power * player_force;
            rb.AddForce(force,ForceMode.Impulse);
        }
        if (jumpNow == true)
        {
            if (other.gameObject.CompareTag("ground"))
            {
                jumpNow = false;
            }
        }
    }

    void Update()
    {
        MovementControll();
        Movement();
        jump();
    }

    void FixedUpdate()
    {

        RotateToMovingDirection();
    }
    void MovementControll()
    {
        //斜め移動と縦横の移動を同じ速度にするためにVector3をNormalize()する。
        moving = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moving.Normalize();
        moving = moving * speed;
        moving.y = rb.velocity.y;
    }

    public void RotateToMovingDirection()
    {
        Vector3 differenceDis = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(latestPos.x, 0, latestPos.z);
        latestPos = transform.position;
        //移動してなくても回転してしまうので、一定の距離以上移動したら回転させる
        if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(differenceDis);
            rot = Quaternion.Slerp(rb.transform.rotation, rot, 0.1f);
            this.transform.rotation = rot;
            //アニメーションを追加する場合
            //animator.SetBool("run", true);
        }
        else
        {
            //animator.SetBool("run", false);
        }
    }

    void Movement()
    {
        rb.velocity = moving;
    }
   
    void jump()
    {
        if (jumpNow == true) return;
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpPwer, ForceMode.Impulse);
            jumpNow = true;
        }
    }

    int player_weight()
    {
        if(force_variable >= 2 && force_variable < 4)
        {
            player_force = 1.5f;
        }
        else { if(force_variable >= 5)
            {
                player_force = 3.0f;
            }
        }


        return 0;
    }
    
}
