using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    private IState currentState;

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    } 
   
    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    public void ChangeState(IState newState)
    {
        if (currentState == null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving()
    {

    }

    public void StopMoving()
    {

    }

    public void Attack()
    {

    }

    public bool IsAttackInRange()
    {
        return false;
    }
}
