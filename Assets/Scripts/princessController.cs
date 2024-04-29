using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class princessController : MonoBehaviour
{
    float horizontal;
    float vertical;
    float speed = 3f;
    public GameObject messagePanel;
    public TMP_Text message;
    public Image textBox;
    CinemachineVirtualCamera camera;
    private Animator animator;
    public bool isMoving;
    private Vector2 input;


    void Start()
    {

       
        DontDestroyOnLoad(gameObject);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isMoving)
        {

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    void FixedUpdate()
    {
        camera = FindFirstObjectByType<CinemachineVirtualCamera>();
        if (camera != null)
        {
            camera.Follow = transform;

        }
        
        

    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag == "dungeonEntrance")
        {
            SceneManager.LoadScene("Dungeon");
            Debug.Log("enter");
        }
        if (collision.gameObject.tag == "weapon")
        {

        }
        if(collision.gameObject.tag == "knight")
        {
            message.text = "You encountered a knight!";
            messagePanel.SetActive(true);
            textBox.enabled = true;
        }
    }
}