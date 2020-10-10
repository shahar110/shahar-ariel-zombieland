using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHighscoreHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_HighScoreController;
    [SerializeField] private UIManager m_UIManagerController;

    void Start()
    {
        m_UIManagerController = UIManager._instance;
    }
    void OnCollisionEnter()
    {
        m_HighScoreController.transform.gameObject.SetActive(false);
        // m_UIManagerController.IsButtonCollided = false;
        UIManager.IsButtonCollided = false;
        Debug.Log("Exiting top ten..");
    }
}
