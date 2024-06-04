using System.Collections;
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
    public int knightRank;
    public mapGenerator map;
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
        int randRow = Random.Range(0, map.sizeY);
        int randCol = Random.Range(0, map.sizeX);
        rb.position = new Vector3(randCol, map.sizeY - randRow, 0) + map.centerize;

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
                    if (walkingRight)
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
                    if (walkingUp)
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
                // stop moving
                rb.velocity = Vector2.zero;
                updateAnimation(0f, 0f, false);
            }
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
        else
        {
            
            rb.velocity = Vector2.zero;
            updateAnimation(0f, 0f, false);
        }
    }

    void updateAnimation(float moveX, float moveY, bool isMoving)
    {
        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);
        animator.SetBool("isMoving", isMoving);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("princess"))
        {
            canMove = false;
            StopAllCoroutines(); 
            StartCoroutine(moveAgain());
        }
    }

    private IEnumerator moveAgain()
    {
        yield return new WaitForSeconds(3f);
        canMove = true;
    }

}
