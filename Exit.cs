using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene("Start");
        Debug.Log("데이터 저장");
    }
}
