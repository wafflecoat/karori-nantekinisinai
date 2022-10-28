
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = 5;
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player2"))
        {
            Vector3 force = transform.position - other.transform.position;
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
        //�΂߈ړ��Əc���̈ړ��𓯂����x�ɂ��邽�߂�Vector3��Normalize()����B
        moving = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moving.Normalize();
        moving = moving * speed;
        moving.y = rb.velocity.y;
    }

    public void RotateToMovingDirection()
    {
        Vector3 differenceDis = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(latestPos.x, 0, latestPos.z);
        latestPos = transform.position;
        //�ړ����ĂȂ��Ă���]���Ă��܂��̂ŁA���̋����ȏ�ړ��������]������
        if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(differenceDis);
            rot = Quaternion.Slerp(rb.transform.rotation, rot, 0.1f);
            this.transform.rotation = rot;
            //�A�j���[�V������ǉ�����ꍇ
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
    
}
