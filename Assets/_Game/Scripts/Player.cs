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

    [SerializeField] private Kunai kunaiFrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isAttack = false;
    //private bool isDeath = false;
    private float horizontal;
    private int jumpCount = 0;
    private int coin = 0;
    private Vector3 savePoint;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            return;
        }
        isGrounded = CheckGrounded();

        // -1 => 0 => 1
        //horizontal = Input.GetAxisRaw("Horizontal");
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
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
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
            
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            // horizontal > 0 tra ve 0, neu khong tra ve 180
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0.1f ? 0 : 180, 0));

            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
            jumpCount = 0;
        }

    }

    public override void OnInit()
    {
        base.OnInit();
        //isDeath = false;
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();
        SavePoint();
        UIManager.instance.SetCoin(coin);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
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

    public void Attack()
    {
        if (isJumping)
        {
            ChangeAnim("jumpAttack");
            isAttack = true;
            Invoke(nameof(ResetAttack), 0.5f);
            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f);
        }
        if (!isJumping)
        {
            ChangeAnim("attack");
            isAttack = true;
            Invoke(nameof(ResetAttack), 0.5f);
            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f);
        }
    }

    public void Throw()
    {
        if (!isJumping)
        {
            ChangeAnim("throw");
            isAttack = true;
            Invoke(nameof(ResetAttack), 0.5f);

            Instantiate(kunaiFrefab, throwPoint.position, throwPoint.rotation);
        }
        if (isJumping)
        {
            ChangeAnim("jumpThrow");
            isAttack = true;
            Invoke(nameof(ResetAttack), 0.5f);

            Instantiate(kunaiFrefab, throwPoint.position, throwPoint.rotation);
        }
            
        //ChangeAnim("jumpThrow");
        //isAttack = true;
        //Invoke(nameof(ResetAttack), 0.5f);

        //Instantiate(kunaiFrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle"); // idle bị loi
        isAttack = false;
    }

    public void Jump()
    {
        if (jumpCount < 2)
        {
            isJumping = true;
            ChangeAnim("jump");
            rb.AddForce(jumpFore * Vector2.up);
            jumpCount++;
        }
            
        
    }

    

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void SetMove(float horizontal)
    {
        Debug.Log("SetMove");
        this.horizontal = horizontal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin ++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "DeathZone")
        {
            //isDeath = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1.1f);
        }
    }

}
