using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class battleSceneChange : MonoBehaviour
{
    public Camera battleCam;
    public Camera mainCamera;
    public GameObject purpleBox;

    public void ChangeScene()
    {
        battleCam.enabled = true;
        mainCamera.enabled = false;
        purpleBox.SetActive(false);
        
    }

}
