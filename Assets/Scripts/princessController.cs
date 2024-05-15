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
    Rigidbody2D rb;
    public GameObject button;

    void Start()
    {

       
        DontDestroyOnLoad(gameObject);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("moveX", horizontal);
        animator.SetFloat("moveY", vertical);
        animator.SetBool("isMoving", Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0);
    }
    

    void FixedUpdate()
    {
        camera = FindFirstObjectByType<CinemachineVirtualCamera>();
        if (camera != null)
        {
            camera.Follow = transform;

        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized * speed;
        rb.velocity = movement;
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
            button.SetActive(true);
        }
    }
}


