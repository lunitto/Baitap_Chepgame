using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public Rigidbody2D rb;
    private float speedKunai = 10f;
    // Start is called before the first frame update
    void Start() 
    {
        OnInit();
    } 

    public void OnInit()
    {
        rb.velocity = transform.right * speedKunai;
        Invoke(nameof(OnDespawn), 4f);
    }
    
    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(30f);
            OnDespawn();
        }
    }

}
