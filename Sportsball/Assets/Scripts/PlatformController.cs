using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    [HideInInspector]
    public bool facingRight = false;
    [HideInInspector]
    public bool jump = true;
    [HideInInspector]
    public float moveForce = 365f;
    [HideInInspector]
    public float maxSpeed = 5f;
    [HideInInspector]
    public float jumpForce = 1200f;

    public Transform groundCheck;

    public PhysicsMaterial2D bouncy;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    public Transform ball;

    float throwStrength = 30.5f;

    float length = 0;

    bool ballHeld = false;

    void Awake() {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && grounded) {
            jump = true;
        }
    }

    void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(h));

        if (h * rb2d.velocity.x < maxSpeed) {
            rb2d.AddForce(Vector2.right * h * moveForce);
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }

        if (h > 0 && facingRight) {
            Flip();
        }
        else if (h < 0 && !facingRight) {
            Flip();
        }

        if (jump) {
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        float distanceToBall = Vector2.Distance(ball.position, transform.position);
        //print(distanceToBall);

        if (ballHeld == false && distanceToBall < 2.5f) {
            if (Input.GetKeyDown(KeyCode.E)) {
                
                ballHeld = true;
            }
        }

        if (ballHeld) {

            //ball.GetComponent<Rigidbody2D>().gravityScale = 0f;

            //ball.GetComponent<Rigidbody2D>().isKinematic = true;

            if (!facingRight) {
                ball.position = new Vector2(transform.position.x + 0.7f, transform.position.y);
            } else if (facingRight) {
                ball.position = new Vector2(transform.position.x - 0.7f, transform.position.y);
            }

            length += Time.deltaTime;

            if (length > 0.5f && Input.GetKeyDown(KeyCode.E)) {

                ball.GetComponent<Rigidbody2D>().mass = 1f;
                //ball.GetComponent<Rigidbody2D>().isKinematic = false;

                length = 0;
                ballHeld = false;

                if (facingRight) {
                    ball.GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * throwStrength);
                }
                else if (!facingRight) {
                    ball.GetComponent<Rigidbody2D>().AddForce(Vector2.left * h * throwStrength);
                }
            }
        }
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
