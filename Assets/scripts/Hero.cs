using System.Collections;
using UnityEngine;

public delegate void DeadEventHandler ();

public class Hero : Character {
    private static Hero instance;

    public event DeadEventHandler Dead;

    [SerializeField]
    private Transform[] groundPoints;

    private bool immortal;

    [SerializeField]
    private float immortalTime;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    public Joystick joystick;

    float axis;

    //Velocidade do personagem
    Vector2 speed;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    public Rigidbody2D Body { get; set; }

    public bool Slide { get; set; }
    public bool Jump { get; set; }

    public static Hero Instance {
        get {

            if (instance == null) {
                instance = GameObject.FindObjectOfType<Hero> ();
            }
            return instance;
        }

    }

    public override bool IsDead {
        get {
            if (health <= 0)
                OnDead ();
            return health <= 0;
        }
    }

    // Use this for initialization
    public override void Start () {
        //Inicializamos o Componente Animator para podermos trabalhar com os parametros que criamos.
        base.Start ();

        Body = GetComponent<Rigidbody2D> ();
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    // Executado em sincronismo com a fisica do jogo.
    void FixedUpdate () {
        if (!TakingDamage && !IsDead) {

            float horizontal = Input.GetAxis ("Horizontal");

            OnGround = IsGrounded ();

            HandleMovements (horizontal);

            Flip (horizontal);

            HandleLayers ();

        }

    }

    void HandleMovements (float horizontal) {
        if (Body.velocity.y < 0)
            MyAnimator.SetBool ("land", true);

        if (!Attack && !Slide && (OnGround || airControl)) {
            Body.velocity = new Vector2 (horizontal * maxSpeed, Body.velocity.y);
        }
        if (Jump && Body.velocity.y == 0) {
            Body.AddForce (new Vector2 (0, jumpForce));
        }

        MyAnimator.SetFloat ("speed", Mathf.Abs (horizontal));
    }

    private void Flip (float horizontal) {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
            ChangeDirection ();
        }

    }

    void Update () {
        if (!TakingDamage && !IsDead) {
            if (transform.position.y <= -14f) {
                Death ();
            }
            HandleInput ();
        }
    }

    private void HandleInput () {
        /* float vertical = joystick.Vertical;
        if(vertical >= .5f)
        {
            jump = true;
        }

        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                attacking = true;
            }
        }
        if(vertical <= -.5f)
        {
            sliding = true;
        } */

        if (Input.GetButtonDown ("Jump")) {
            MyAnimator.SetTrigger ("jump");
        }

        if (Input.GetButtonDown ("Fire1")) {
            MyAnimator.SetTrigger ("attack");
        }

        if (Input.GetButtonDown ("Fire2")) {
            MyAnimator.SetTrigger ("throw");
        }

        if (Input.GetButtonDown ("Fire3")) {
            MyAnimator.SetTrigger ("slide");
        }

    }

    void LateUpdate () { }

    private bool IsGrounded () {
        if (Body.velocity.y <= 0) {
            foreach (Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject != gameObject) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers () {
        if (!OnGround) {
            MyAnimator.SetLayerWeight (1, 1);
        } else {
            MyAnimator.SetLayerWeight (1, 0);
        }
    }

    private IEnumerator IndicateImmortal () {
        while (immortal) {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds (.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds (.1f);
        }
    }

    public void OnDead () {
        if (Dead != null)
            Dead ();
    }

    public override IEnumerator TakeDamage () {
        if (!immortal) {

            health -= 10;

            if (!IsDead) {
                MyAnimator.SetTrigger ("damage");
                immortal = true;
                StartCoroutine (IndicateImmortal ());
                yield return new WaitForSeconds (immortalTime);
                immortal = false;
            } else {
                MyAnimator.SetLayerWeight (1, 0);
                MyAnimator.SetTrigger ("die");
            }
        }
    }

    public override void Death () {
        Body.velocity = Vector2.zero;
        MyAnimator.SetTrigger ("idle");
        health = 30;
        transform.position = startPos;
    }
}