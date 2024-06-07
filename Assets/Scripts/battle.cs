using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class battle : MonoBehaviour
{

    public TMP_Text msg;
    public Camera battleCam;
    public Camera mainCam;
    public float msgDispTime = 2.0f; 
    String[] possibleAttacks = {"You used your sword!", "You punched the knight in the face!", "You threw a dagger at the knight"};
    String[] possibleKnightAttacks = { "The knight threw rocks at you", "The knight head butted you with his helmet on!", "The knight used his sword!", "" };
    public int attackCount = 0;
    public princessController pc;
    public GameObject princessImage;
    public GameObject knightImage;
    public int health = 3;
    public Image health1;
    public Image health2;
    public Image health3;
    public int weapons;
    public knightController lastknight;
    public int lastKnightRank;
    public int keyCount = 0;
    void Start()
    {
        pc = FindObjectOfType<princessController>();
        

         
    }

    public void StartBattle()
    {
        weapons = pc.weaponCount;
        lastknight = pc.lastCollided;
        lastKnightRank = lastknight.knightRank;
    }
    

    public void Attack()
    {
        
        if(weapons == 0 && lastKnightRank > 0) 
        {
            Debug.Log("last knight rank " + lastKnightRank);
            StartCoroutine(showMessage("You lost. Find more weapons and try again :(", msgDispTime));
            health--;
            if(health == 2)
            {
                health1.enabled = false;
            }
            if(health == 1)
            {
                health2.enabled = false;
            }

        }
        else if(attackCount == lastKnightRank)
        {
            Debug.Log("last knight rank " + lastKnightRank);
            StartCoroutine(knightDed("You defeated the knight and earned a key! You are now one step closer to saving the prince."));
            keyCount++;
        }
        else
        {
            Debug.Log("last knight rank " + lastKnightRank);
            int index = Random.Range(0, possibleAttacks.Length);
            StartCoroutine(showBattlemsg(possibleAttacks[index], msgDispTime));
            attackCount++;
            weapons--;

          
          

        }
    }

    private IEnumerator showMessage(string message, float delay)
    {
        msg.text = message;
        yield return new WaitForSeconds(delay);
        battleCam.enabled = false;
        mainCam.enabled = true;
        msg.text = " ";
    }
    private IEnumerator showBattlemsg(string message,float delay)
    {
        msg.text = message;
        StartCoroutine(flash(knightImage));
        yield return new WaitForSeconds(delay);
        int index2 = Random.Range(0, possibleKnightAttacks.Length);
        msg.text = possibleKnightAttacks[index2];
        StartCoroutine(flash(princessImage));
        yield return new WaitForSeconds(delay);
      
    }
    private IEnumerator flash(GameObject character)
    {
        character.SetActive(false);
        yield return new WaitForSeconds(.5f);
        character.SetActive(true);
    }
    private IEnumerator knightDed(string message)
    {
       
        knightImage.SetActive(false);
        yield return new WaitForSeconds(.5f);
        knightImage.SetActive(true);
        yield return new WaitForSeconds(.3f);
        knightImage.SetActive(false);
        msg.text = message;
        yield return new WaitForSeconds(2.0f);
        battleCam.enabled = false;
        mainCam.enabled = true;
        msg.text = " ";
        lastknight.gameObject.SetActive(false);
    }
  
}
