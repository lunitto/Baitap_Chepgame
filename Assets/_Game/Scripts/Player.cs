using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 50;
    [SerializeField] private float jumpFore = 350;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;
    private float horizontal;
    
    private int coin = 0;
    private Vector3 savePoint;

    // Start is called before the first frame update
    void Start()
    {
        SavePoint();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDeath)
        {
            return;
        }
        isGrounded = CheckGrounded();

        // -1 => 0 => 1
        horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            
            //throw 
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
                 
        }

        //check falling
        if (!isGrounded && rb.velocity.y < 0)
            {
                isJumping = false;
                ChangeAnim("fall");
            }

        //moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            // horizontal > 0 tra ve 0, neu khong tra ve 180
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0.1f ? 0 : 180, 0));

            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }

    }

    public override void OnInit()
    {
        base.OnInit();
        isDeath = false;
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }


    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }

        //return hit.collider != null;

    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void ResetAttack()
    {
        ChangeAnim("ilde"); // idle bị loi
        isAttack = false;
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpFore * Vector2.up);
    }

    

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin ++;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "DeathZone")
        {
            isDeath = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1.1f);
        }
    }

}
