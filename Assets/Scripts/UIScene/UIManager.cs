using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public delegate void HowToPlayScreenDelegate(bool display);
    public event HowToPlayScreenDelegate OnClick_HowToPlayScreen;
    
    public delegate void ExitScreenDelegate();
    public event ExitScreenDelegate OnClick_ExitScreen;

    public bool m_IsButtonCollided = false;

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