using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    [SerializeField] public  HighscoreManager _highscoreManager;
    public static bool IsButtonCollided { get; set; } = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

        IsButtonCollided = false;
        // _highscoreManager = GetComponent<HighscoreManager>();
        _highscoreManager.transform.gameObject.SetActive(false);
    }

    void Start()
    {
        // IsButtonCollided = false;
    }

    public static void EnableUI()
    {
        IsButtonCollided = false;
    }

    public void TestMessage()
    {
        Debug.Log("Message form UI Manager...");
    }

    // Start is called before the first frame update
    // public void InvokeExitScreen()
    // {
    //     if (OnClick_ExitScreen != null)
    //     {
    //         OnClick_ExitScreen.Invoke();
    //     }
    // }
    //
    // public void InvokeHowToPlayScreen(bool i_Display)
    // {
    //     if (OnClick_HowToPlayScreen != null)
    //     {
    //         OnClick_HowToPlayScreen.Invoke(i_Display);
    //     }
    // }
}