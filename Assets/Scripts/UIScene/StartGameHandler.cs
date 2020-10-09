using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameHandler : MonoBehaviour
{
    [SerializeField] private Text m_CounterText;
    [SerializeField] private UIManager m_UIManagerController;

    void Start()
    {
        m_UIManagerController  =  UIManager._instance;
    }
    // Start is called before the first frame update
    public void StartGame()
    {
        m_CounterText.gameObject.SetActive(true);
        StartCoroutine(startGameCountDown(3));
    }
    
    public IEnumerator startGameCountDown(int i_timerLeft)
    {
        m_CounterText.text = "" + i_timerLeft;
        yield return new WaitForSeconds(1f);
        if (i_timerLeft > 0)
        {
            StartCoroutine(startGameCountDown(i_timerLeft - 1));
        }
        else
        {
            m_CounterText.gameObject.SetActive(false);
            // SceneManager.LoadScene("ShooterScene");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("--------------Entered Start Button Collider----------------");
        // Debug.Log("--------------UI Manager: " + m_UIManagerController.m_IsButtonCollided + "----------------");
        // if (!m_UIManagerController.m_IsButtonCollided)
        // {
            // m_UIManagerController.m_IsButtonCollided = true;
            Debug.Log("Start Button Collision...");
            StartGame();
        // }
    }
}