using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameHandler : MonoBehaviour
{
    [SerializeField] private UIManager m_UIManagerController;

    // Start is called before the first frame update
    void Start()
    {
        m_UIManagerController  =  UIManager._instance;
    }
    
    void OnCollisionEnter()
    {
        if (!UIManager.IsButtonCollided)
        {
            UIManager.IsButtonCollided = true;
            Debug.Log("TopTen Button Collision...");
            Application.Quit();
        } 
    }
}
