using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {
    private Enemy enemy;

    private float timer;

    private float duration = 5f;

    public void Execute () {
        Debug.Log ("patrolling");
        Patrol ();

        enemy.Move ();

        if (enemy.Target != null && enemy.InThrowRange)
            enemy.ChangeState (new RangedState ());
    }

    public void Enter (Enemy enemy) {
        duration = UnityEngine.Random.Range (1, 10);
        this.enemy = enemy;
    }
    public void Exit () {

    }

    public void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "item")
            enemy.Target = Hero.Instance.gameObject;
    }

    private void Patrol () {
        enemy.MyAnimator.SetFloat ("speed", 0);

        timer += Time.deltaTime;

        if (timer >= duration) {
            enemy.ChangeState (new IdleState ());
        }
    }

}