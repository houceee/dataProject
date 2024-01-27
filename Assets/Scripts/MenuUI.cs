using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MenuUI : MonoBehaviour
{
    public static MenuUI Instance;
    public string playerName;
    public TMP_InputField nameInputField;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void StartButton()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            playerName = "UnKnown";
        }
        else
        {
            playerName = nameInputField.text;
        }

        SceneManager.LoadScene(1);
    }

    public void ResetBestScore()
    {
        // Specify the path to the JSON file
        string path = Application.persistentDataPath + "/savefile.json";

        // Check if the file exists
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Best score reset and JSON file deleted.");
        }
        else
        {
            Debug.Log("No saved best score found.");
        }
    }
}
