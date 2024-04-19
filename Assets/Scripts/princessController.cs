using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class princessController : MonoBehaviour
{
    float horizontal;
    float vertical;
    float speed = 3f;
    Rigidbody2D rigidbody2d;
   

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
        
    }
 

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position); // move player
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if(collision.gameObject.tag == "dungeonEntrance")
        {
            SceneManager.LoadScene("Dungeon");
            Debug.Log("enter");
        }
    }
}
