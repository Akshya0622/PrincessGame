using UnityEngine;

public class knightController : MonoBehaviour
{
    public float speed = 2f;
    public float walkDuration = 1.5f; 
    public float turnDuration = 2f;
    private bool walkingRight;
    private bool walkingUp;
    private float timer = 0f; 
    private Rigidbody2D rb;
    public int pickDir;
    private Animator animator;
    public bool canMove = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pickDir = Random.Range(0, 10);
        if(pickDir < 5)
        {
            walkingRight = true;
        }
        else
        {
            walkingUp = true;
        }

    }

    void Update()
    {

        if (canMove)
        {
            timer += Time.deltaTime;


            if (timer <= walkDuration)

            {

                if (pickDir < 5)
                {
                    if (walkingRight == true)
                    {
                        rb.velocity = Vector2.right * speed;
                        updateAnimation(1f, 0f, true);
                    }
                    else
                    {
                        rb.velocity = Vector2.left * speed;
                        updateAnimation(-1f, 0f, true);
                    }
                }
                else
                {
                    if (walkingUp == true)
                    {
                        rb.velocity = Vector2.up * speed;
                        updateAnimation(0f, 1f, true);
                    }
                    else
                    {
                        rb.velocity = Vector2.down * speed;
                        updateAnimation(0f, -1f, true);
                    }
                }

            }

            else if (timer <= walkDuration + turnDuration)
            {
                // stop movingggg
                rb.velocity = Vector2.zero;
                updateAnimation(0f, 0f, false);
            }
            //change direc and reset timer
            else
            {
                if (pickDir < 5)
                {
                    walkingRight = !walkingRight;
                }
                else
                {
                    walkingUp = !walkingUp;
                }

                timer = 0f;

            }
        }   
    }
    void updateAnimation(float moveX, float moveY, bool isMoving)
    {
        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);
        animator.SetBool("isMoving", isMoving);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "princess")
        {
            canMove = false;
            Debug.Log("what");
        }
    }
}
