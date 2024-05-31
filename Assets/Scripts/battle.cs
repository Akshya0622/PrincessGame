using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class battle : MonoBehaviour
{

    public TMP_Text msg;
    public Camera battleCam;
    public Camera mainCam;
    public float messageDisplayTime = 2.0f; 
    String[] possibleAttacks = {"You used your sword!", "You punched the knight in the face!", "You threw a dagger at the knight"};
    String[] possibleKnightAttacks = { "The knight threw rocks at you", "The knight head butted you with his helmet on!", "The knight used his sword!", "" };
    public int attackCount = 0;
    public princessController pc;
    void Start()
    {
        pc = FindObjectOfType<princessController>();
    }

    public void Attack()
    {
        if (attackCount == pc.weaponCount)
        {
            StartCoroutine(ShowMessage("You lost. Find more weapons and try again :(", messageDisplayTime));
        }
        else
        {
            int index = Random.Range(0, possibleAttacks.Length);
            msg.text = possibleAttacks[index];
            attackCount++;
            int index2 = Random.Range(0, possibleKnightAttacks.Length);
            msg.text = possibleKnightAttacks[index2];

        }
    }

    private IEnumerator ShowMessage(string message, float delay)
    {
        msg.text = message;
        yield return new WaitForSeconds(delay);
        battleCam.enabled = false;
        mainCam.enabled = true;
    }
}
