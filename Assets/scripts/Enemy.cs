using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    public GameObject Target { get; set; }
    private IEnemyState currentState;

    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private float throwRange;

    [SerializeField]
    public Transform leftEdge;

    [SerializeField]
    public Transform rightEdge;
    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            else
                return false;
        }
    }
    public bool InThrowRange
    {
        get
        {
            if (Target != null)
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            else
                return false;
        }
    }

    public override bool IsDead => health <= 0;

    public override void Start()
    {
        base.Start();
        Hero.Instance.Dead += new DeadEventHandler(RemoveTarget);
        this.ChangeState(new IdleState());
        Debug.Log("START CALLED");
    }
    void Awake()
    {
        base.Start();
        Hero.Instance.Dead += new DeadEventHandler(RemoveTarget);
        this.ChangeState(new IdleState());
        Debug.Log("AWAKE CALLED");
    }

    void Update()
    {
        OnGround = true;
        if (!IsDead)
        {
            if (!TakingDamage)
            {

                currentState.Execute();
            }
            LookAtTarget();

        }

    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {

        if (rightEdge == null)
        {
            MyAnimator.SetFloat("speed", 1);

            transform.Translate(GetDirection() * (maxSpeed * Time.deltaTime));
        }
        else if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
        {
            MyAnimator.SetFloat("speed", 1);

            transform.Translate(GetDirection() * (maxSpeed * Time.deltaTime));

        }
        else if (currentState is PatrolState)
            ChangeDirection();

    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter2D(other);
    }

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
                ChangeDirection();

        }
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;

        if (!IsDead)
            MyAnimator.SetTrigger("damage");
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }

    public override void Death()
    {
        MyAnimator.ResetTrigger("die");
        MyAnimator.SetTrigger("idle");
        health = 30;
        transform.position = startPos;
    }
}