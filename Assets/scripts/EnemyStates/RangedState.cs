using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState {
    private Enemy enemy;

    private float throwTimer;
    private float throwCoolDown = 2;
    private bool canThrow = true;

    public void Execute () {
        if (enemy.InMeleeRange)
            enemy.ChangeState (new MeleeState ());
        ThrowItem ();
        if (enemy.Target != null)
            enemy.Move ();
        else
            enemy.ChangeState (new IdleState ());
    }
    public void Enter (Enemy enemy) {
        this.enemy = enemy;
    }
    public void Exit () {

    }

    public void OnTriggerEnter2D (Collider2D other) {

    }

    public void ThrowItem () {
        throwTimer += Time.deltaTime;
        if (throwTimer >= throwCoolDown) {
            canThrow = true;
            throwTimer = 0;
        }

        if (canThrow) {
            canThrow = false;
            enemy.MyAnimator.SetTrigger ("throw");
        }
    }
}