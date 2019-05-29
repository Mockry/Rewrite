using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string firstLevel;
   
   //Basic script for the title screen

    void Update()
    {
        //loads Level1 one when you left click
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene (firstLevel);
    }
}
