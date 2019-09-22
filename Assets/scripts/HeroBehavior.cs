using UnityEngine;
using System.Collections;

public class HeroBehavior : MonoBehaviour
{
    Animator animator;
    AnimationState anim;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private GameObject sword;

    public Joystick joystick;

    float axis;

    //Verifica se o personagem esta' olhando para o lado direito.
    bool ladoDireito = true;

    //Velocidade do personagem
    Vector2 velocidade;

    bool attacking = false;
    bool throwing = false;


    bool sliding = false;

    private bool isGrounded;

    private bool jump;

    private bool moving;

    private bool jumpAttack;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;


    //Velocidade ma'xima que o personagem pode correr.
    public float MaxVelocidade = 10;

    // Use this for initialization
    void Start()
    {
        //Inicializamos o Componente Animator para podermos trabalhar com os parametros que criamos.
        animator = GetComponent<Animator>();
        anim = GetComponent<AnimationState>();
    }

    // Executado em sincronismo com a fisica do jogo.
    void FixedUpdate()
    {
        HandleMovements();

        HandleAttacks();

        HandleLayers();

        ResetValues();

        /* if(Input.GetButtonDown("Fire1"))
        {
            PlayAttackAnimation();
        }

        attacking = false;
 */
    }

    void HandleMovements()
    {
        // Seta a varia'vel axis para o valor recebido quando o jogador preciona algum direcional.
        // O Input.GetAxis deve ser usado porque suporta as setas do teclado, controles e joysticks.
        axis = Input.GetAxisRaw("Horizontal");
        //axis = joystick.Horizontal;

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        isGrounded = IsGrounded();

        //Viramos o personagem de acordo com o valor da varia'vel axis
        if (axis > 0 && !ladoDireito)
            Flip();
        else if (axis < 0 && ladoDireito)
            Flip();

        if (sliding)
            MaxVelocidade += 5;
        //Setamos a variavel velocidade.
        if (!animator.GetBool("slide") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && (isGrounded || airControl))
        {
            velocidade = new Vector2(axis * MaxVelocidade, body.velocity.y);

            //Alteramos o parametro Velocidade que colocamos no animator para a velocidade do personagem.
            //Como nao importa se e' positivo ou negativo, usamos a funcao Mathf.Abs para pegar o valor absoluto (sem sinal).
            animator.SetFloat("Velocidade", Mathf.Abs(axis));

            //Por ultimo alteramos a velocidade do personagem para gerar o movimento
            body.velocity = velocidade;

            moving = true;
        }


        if (isGrounded && jump)
        {
            isGrounded = false;
            body.AddForce(new Vector2(0, jumpForce));
            animator.SetTrigger("jump");

        }

        if (sliding && !animator.GetCurrentAnimatorStateInfo(0).IsName("sliding"))
        {
            animator.SetBool("slide", true);
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("sliding"))
        {
            animator.SetBool("slide", false);
        }
    }

    /* void Attack()
    {
        attacking = true;
    } */

    //Viramos o personagem para o lado que o jogador esta' apertando.
    void Flip()
    {
        ladoDireito = !ladoDireito;

        Vector2 novoScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

        transform.localScale = novoScale;
    }

    void Update()
    {
        HandleInput();
    }


    private void HandleAttacks()
    {
        if (attacking && isGrounded)
        {
            animator.SetTrigger("attack");
        }

        if (throwing && isGrounded)
        {
            animator.SetTrigger("throw");
            ThrowSword(0);
        }

        if (jumpAttack && !isGrounded && !animator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            animator.SetBool("jumpAttack", true);
        }
        if (!jumpAttack && !animator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            animator.SetBool("jumpAttack", false);
        }
    }

    private void HandleInput()
    {
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

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }



        if (Input.GetButtonDown("Fire1"))
        {
            attacking = true;
            jumpAttack = true;

        }

        if (Input.GetButtonDown("Fire2"))
        {
            throwing = true;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            sliding = true;
        }

    }

    void LateUpdate()
    {
    }

    void ResetValues()
    {
        attacking = false;
        sliding = false;
        jump = false;
        MaxVelocidade = 10;
        moving = false;
        jumpAttack = false;
        throwing = false;
    }

    private bool IsGrounded()
    {
        if (GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    public void ThrowSword(int value)
    {
        if (ladoDireito)
        {
            GameObject swordThrowed = (GameObject)Instantiate(sword, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            swordThrowed.GetComponent<Sword>().Init(Vector2.right);
            swordThrowed.GetComponent<Sword>().Initialize(Vector2.right);
        }
        else
        {
            GameObject swordThrowed = (GameObject)Instantiate(sword, transform.position, Quaternion.Euler(new Vector3(0, -180, 0)));
            swordThrowed.GetComponent<Sword>().Init(Vector2.left);
            swordThrowed.GetComponent<Sword>().Initialize(Vector2.left);
        }
    }



}