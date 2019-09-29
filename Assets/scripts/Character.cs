using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public Animator MyAnimator { get; private set; }

    [SerializeField]
    private EdgeCollider2D attackCollider;

    [SerializeField]
    private List<string> damageSources = new List<string> ();

    [SerializeField]
    protected int health;

    public abstract bool IsDead { get; }

    [SerializeField]
    protected Transform itemPos;

    [SerializeField]
    protected float maxSpeed = 10;

    protected bool facingRight = true;

    [SerializeField]
    protected GameObject itemThrowed;

    protected Vector2 startPos;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public bool Throw { get; set; }

    public bool OnGround { get; set; }
    public EdgeCollider2D AttackCollider { get => attackCollider; }

    // Start is called before the first frame update
    public virtual void Start () {
        facingRight = true;
        startPos = transform.position;
        MyAnimator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update () {

    }
    public abstract IEnumerator TakeDamage ();
    public abstract void Death ();

    public void ChangeDirection () {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        //transform.localScale = new Vector3(transform.position.x * -1, 1, 1);
    }

    public void ThrowAttack (int value) {
        if (!OnGround && value == 1 || OnGround && value == 0) {
            if (facingRight) {
                GameObject swordThrowed = (GameObject) Instantiate (itemThrowed, itemPos.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
                swordThrowed.GetComponent<Sword> ().Init (Vector2.right);
            } else {
                GameObject swordThrowed = (GameObject) Instantiate (itemThrowed, itemPos.position, Quaternion.Euler (new Vector3 (0, -180, 0)));
                swordThrowed.GetComponent<Sword> ().Init (Vector2.left);
            }
        }
    }

    public virtual void OnTriggerEnter2D (Collider2D other) {
        if (damageSources.Contains (other.tag))
            StartCoroutine (TakeDamage ());
    }

    public void MeleeAttack () {
        AttackCollider.enabled = true;

        AttackCollider.transform.position = new Vector3 (AttackCollider.transform.position.x + 0.01f, AttackCollider.transform.position.y, AttackCollider.transform.position.z);

    }

}