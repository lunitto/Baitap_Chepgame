using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float hp;
    private string currentAnimName;

    public bool IsDeath => hp <= 0; // khi nao chet tra ve hp <= 0

    private void start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
    }

    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnHit( float damage )
    {
        if ( !IsDeath )
        {
            hp -= damage;
            if ( IsDeath )
            {
                OnDeath();
            }
        }
    }

    
}
