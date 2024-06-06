using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
    public GameObject battleButton;
    public GameObject runAwayButton;
    public int weaponCount;
    public int knightRank;
    public knightController knight;
    public knightController lastCollided = null;
    public int keyCount;
    void Start()
    {

        weaponCount = 0;
        keyCount = 0;
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
           
            Destroy(collision.gameObject);
            StartCoroutine(showMessage("You collected a weapon!", 2));
            weaponCount++;
        }
        if(collision.gameObject.tag == "knight")
        {
            knight = collision.gameObject.GetComponent<knightController>();
            lastCollided = knight;
            Debug.Log(lastCollided != null ? "not null" : "null");
            knightRank = knight.knightRank;
            message.text = "You encountered a knight! You may choose to either battle the knight to collect a key or to run away";
            messagePanel.SetActive(true);
            textBox.enabled = true;
            battleButton.SetActive(true);
            runAwayButton.SetActive(true);


        }
        

    }
    private IEnumerator showMessage(string msg, float delay)
    {
        message.text = msg;
        yield return new WaitForSeconds(delay);
        message.text = " ";
    }


}


