using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class battleSceneChange : MonoBehaviour
{
    public Camera battleCam;
    public Camera mainCamera;
    public GameObject purpleBox;
    public GameObject runButton;
    public GameObject battleButton;
    public TMP_Text message;
    public GameObject menu;
    public princessController p;
    public Image key1;
    public Image key2;
    public Image key3;
    public battle battle;
    public GameObject dungeon;

    public void ChangeScene()
    {
        battle.StartBattle();
        battleCam.enabled = true;
        mainCamera.enabled = false;
        purpleBox.SetActive(false);

    }
    public void runAway()
    {
        p.lastCollided.canMove = true;

        message.text = " ";
        runButton.SetActive(false);
        battleButton.SetActive(false);

    }
    public void loadGame()
    {
        menu.SetActive(false);
    }
    public void Update()
    {
        if (battle.keyCount == 1)
        {
            key1.enabled = true;
        }
        if (battle.keyCount == 2)
        {
            key2.enabled = true;
        }
        if (battle.keyCount == 3)
        {
            key3.enabled = true;
            message.text = "You collected all three keys! Now you can unlock the dungeon, battle the dragon, and save your prince!";
            dungeon.SetActive(true);
        }

    }
}