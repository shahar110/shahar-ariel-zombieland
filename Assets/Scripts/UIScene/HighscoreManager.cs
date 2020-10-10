using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField] private Transform m_scoreItemContainer;
    [SerializeField] private Transform m_scoreItemTemplate;
    public static HighscoreManager _instance;
    private List<int> m_ScoreList;
    private List<Transform> m_ScoreItemsTransformList;
    
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
        
        m_scoreItemTemplate.gameObject.SetActive(false);
        m_ScoreItemsTransformList = new List<Transform>();
        
        // ------- Clear list - deactivated code -------
        // m_ScoreList = new List<int>() { 111, 222, 333};
        // HighscoreTable temporaryTable = new HighscoreTable() {highscoreList = m_ScoreList};
        // string json = JsonUtility.ToJson(temporaryTable);
        // PlayerPrefs.SetString("HighscoreTable", json);
        // PlayerPrefs.Save();
        // AddHighScoreEntry(555);
        // AddHighScoreEntry(666);
        // AddHighScoreEntry(777);
        // AddHighScoreEntry(888);
        
        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        HighscoreTable highscoreTable = JsonUtility.FromJson<HighscoreTable>(jsonString);
        Debug.Log(highscoreTable.highscoreList.ToString());
        
        highscoreTable.highscoreList.Sort((num1, num2) => num2 - num1);
        foreach (int score in highscoreTable.highscoreList)
        {
            CreateScoreItem(score, m_scoreItemContainer, m_ScoreItemsTransformList);
        }
    }

    public void CreateScoreItem(int i_Score, Transform i_Container, List<Transform> i_TransformList)
    {
        float templateHeight = 20f;
        Vector3 scoreItemPosition = m_scoreItemTemplate.position;
        
        Transform scoreTransform = Instantiate(m_scoreItemTemplate, i_Container);
        RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
        // scoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
        scoreRectTransform.anchoredPosition = new Vector3(scoreItemPosition.x, scoreItemPosition.y * -i_TransformList.Count*5, scoreItemPosition.z);
        scoreTransform.gameObject.SetActive(true);

        scoreTransform.Find("posText").GetComponent<Text>().text = getScorePositionText(i_TransformList.Count + 1);
        int currentScore = i_Score;
        scoreTransform.Find("scoreText").GetComponent<Text>().text = currentScore.ToString();
        
        i_TransformList.Add(scoreTransform);
    }

    private string getScorePositionText(int i_Rank)
    {
        string rankString;
        switch (i_Rank)
        {
            case 1: 
                rankString = "1ST";
                break;
            case 2: 
                rankString = "2ST";
                break;
            case 3: 
                rankString = "3ST";
                break;
            default:
                // rankString = string.Format("{0]TH", i_Rank);
                rankString = i_Rank + "TH";
                break;
        }

        return rankString;
    }

    public class HighscoreTable
    {
        public List<int> highscoreList;
    }
}
