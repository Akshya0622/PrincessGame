using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class battle : MonoBehaviour
{

    public TMP_Text msg;
    public Camera battleCam;
    public Camera mainCam;
    public float messageDisplayTime = 2.0f; 
    String[] possibleAttacks = { "You used your sword!", "You punched the knight in the face!", "You threw a dagger at the knight" };
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
            StartCoroutine(ShowMessage("Oh no", messageDisplayTime));
        }
        else
        {
            int index = Random.Range(0, possibleAttacks.Length);
            msg.text = possibleAttacks[index];
            attackCount++;
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
