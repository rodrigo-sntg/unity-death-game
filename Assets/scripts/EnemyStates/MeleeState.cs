﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState {

    private Enemy enemy;
    private float attackTimer;
    private float attackCoolDown = 2;
    private bool canAttack = true;
    public void Execute () {
        Attack ();

        if (enemy.InThrowRange && !enemy.InMeleeRange)
            enemy.ChangeState (new RangedState ());
        else if (enemy.Target == null)
            enemy.ChangeState (new IdleState ());
    }
    public void Enter (Enemy enemy) {
        this.enemy = enemy;
    }
    public void Exit () {

    }

    public void OnTriggerEnter2D (Collider2D other) {

    }

    public void Attack () {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCoolDown) {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack) {
            canAttack = false;
            enemy.MyAnimator.SetTrigger ("attack");
        }
    }
}