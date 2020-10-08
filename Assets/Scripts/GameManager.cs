using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static int score = 0;
    private static int health = 12;
    private static int curlevel = 1;
    private static bool checkForLevel = false;
    public static int[] levelScoring = { 20, 40, 60 };

    public GameObject[] enemyPrefabs;

    // UI
    [SerializeField]
    public Text scoreText;
    [SerializeField]
    public Text healthText;
    [SerializeField]
    public Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        onGUI();
    }

    // Update is called once per frame
    void Update()
    {
        onGUI();
        Debug.Log("<< health =" + health);
        if (health <= 0)
        {
            gameOver();
        }

        if (checkForLevel)
            levelUp();
    }

    public void levelUp()
    {
        if (score >= levelScoring[0] && curlevel == 1)
        {
            curlevel++;
            Debug.Log("reached level 2");
        }
        else if (score >= levelScoring[1] && curlevel == 2)
        {
            curlevel++;
            Debug.Log("reached level 3");
        }
        else if (score >= levelScoring[2] && curlevel == 3)
        {
            curlevel++;
            Debug.Log("reached level 4");
        }
        levelText.text = "Level: " + (curlevel).ToString();
        EnemyManager.incDamage(1);
        checkForLevel = false;
    }

    public static void addPoints(int pointVal)
    {
        score += pointVal;
        checkForLevel = true;
    }

    public static void decHealth(int healthDelta)
    {
        health -= healthDelta;
    }

    private void gameOver()
    {
        Debug.Log("<< GameOver()");
        // Open UI
        // Reload scene
        //SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    private void onGUI()
    {
        scoreText.text = "Socre: " + score.ToString();
        healthText.text = "Health: " + health.ToString();
    }
}


