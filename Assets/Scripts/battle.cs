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
    String[] possibleAttacks = {"You used your sword!", "You punched the knight in the face!", "You threw a dagger at the knight"};
    int attackCount = 0;
  
    public void attack()
    {
        
        int index = Random.Range(0, possibleAttacks.Length);
        msg.text = possibleAttacks[index];
        attackCount++;

    }
}
