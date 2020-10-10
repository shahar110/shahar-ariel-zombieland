using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score = 0;
    public static int health = 10;
    public static int curlevel = 1;
    private static bool checkForLevel = false;
    private int numOfLevels = 10;
    public static int[] levelScoring;

    public GameObject[] enemyPrefabs;

    // UI
    [SerializeField]
    public Text scoreText;
    [SerializeField]
    public Text healthText;
    [SerializeField]
    public Text levelText;
    
    public static void initialize()
    {
        score = 0;
        health = 10;
        curlevel = 1;
        checkForLevel = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // score = 0;
        // health = 10;
        // curlevel = 1;
        // checkForLevel = false;
        // numOfLevels = 10;
        onGUI();

        levelScoring = new int[numOfLevels];
        for (int i = 0; i < numOfLevels; i++)
        {
            levelScoring[i] = 4 * (i + 1);
        }
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
        for (int i = 0; i < numOfLevels - 1; i++)
        {
            if (score >= levelScoring[i] && curlevel == i + 1)
            {
                curlevel++;
                EnemySpawner.levelUp();
                Debug.Log("reached level " + curlevel);
            }
        }
        levelText.text = "Level: " + (curlevel).ToString();
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
        Debug.Log("------------------------ GameOver() ------------------------");
        CheckAndAddToHighscore(score);
        // Open UI
        UIManager.EnableUI();
        SceneManager.LoadScene("MainMenu");
    }
    
    private void CheckAndAddToHighscore(int i_Score) 
    { 
        // Load Highscore table from UserPref
        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        HighscoreManager.HighscoreTable currentHighscoreTable = JsonUtility.FromJson<HighscoreManager.HighscoreTable>(jsonString);
        
        // check if it's on top-10
        if (currentHighscoreTable.highscoreList.Count > 9)
        {
            if (currentHighscoreTable.highscoreList.Exists(num => i_Score > num))
            {
                // remove lower score
                int itemToDelete = currentHighscoreTable.highscoreList.Find(num => i_Score > num);
                currentHighscoreTable.highscoreList.Remove(itemToDelete);

                // Add new score entry to the table
                currentHighscoreTable.highscoreList.Add(i_Score);
            }
            else
            {
                return;
            }
        }
        else
        {
            currentHighscoreTable.highscoreList.Add(i_Score);
        }
        
        // Save updated talbe
        string json = JsonUtility.ToJson(currentHighscoreTable);
        PlayerPrefs.SetString("HighscoreTable", json);
        PlayerPrefs.Save();
    }

    private void onGUI()
    {
        scoreText.text = "Socre: " + score.ToString();
        healthText.text = "Health: " + health.ToString();
    }
}