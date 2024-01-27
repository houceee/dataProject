using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScore;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int  best = 0;
    private string bestText;


    private bool m_GameOver = false;

    string playerName = MenuUI.Instance.playerName;     // Access the player's name from MenuUI

    void Awake()
    {
        LoadColor();
    }
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);


        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        ScoreText.text = $"{playerName}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            SaveBestScore();
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;

        // Update the ScoreText with playerName and score
        ScoreText.text = $"{playerName}: Score : {m_Points}";
        if (m_Points > best)
        {
            NewBestScore();
        }
    }
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    public void NewBestScore()
    {
        best = m_Points;
        bestText = $"{playerName}: Score : {m_Points}";
        bestScore.text = bestText;
    }
    [System.Serializable]
    public class SaveBest
    {
        public int lastBest;
        public string lastText;
    }

    public void SaveBestScore()
    {
        SaveBest data = new SaveBest();
        data.lastBest = best;
        data.lastText = bestText;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadColor()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveBest data = JsonUtility.FromJson<SaveBest>(json);

            best = data.lastBest;
            bestText = data.lastText;
            bestScore.text = bestText;
        }
    }
}
