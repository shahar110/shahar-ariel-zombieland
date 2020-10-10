using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopTenHandler : MonoBehaviour
{
    [SerializeField] private UIManager m_UIManagerController;
    [SerializeField] private HighscoreManager m_HighScoreController;

    private void Awake()
    {
        
        m_HighScoreController = GameObject.GetComponent<HighscoreManager>();
    }

    void Start()
    {
        // m_HighScoreController = GetComponent<HighscoreManager>();
        // m_HighScoreController = GameObject.GetComponent<HighscoreManager>();
        // m_HighScoreController = GameObject.Find("HighscoreCanvas").GetComponent<HighscoreManager>();
        // m_HighScoreController =   new HighscoreManager();
        m_UIManagerController = UIManager._instance;
        m_HighScoreController = HighscoreManager._instance;
        // m_HighScoreController = HighscoreManager.
    }
    void OnCollisionEnter()
    {
        if (!UIManager.IsButtonCollided)
        {
            // m_UIManagerController.IsButtonCollided = true;
            UIManager.IsButtonCollided = true;
            // m_UIManagerController._highscoreManager.transform.gameObject.SetActive(true);
            m_HighScoreController.transform.gameObject.SetActive(true);
            Debug.Log("TopTen Button Collision...");
        } 
    }
}