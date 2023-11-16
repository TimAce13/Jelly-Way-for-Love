using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI levelText;

    private int _currentLevel;
    void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            _currentLevel = PlayerPrefs.GetInt("Level");

        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
            _currentLevel = PlayerPrefs.GetInt("Level");
        }

        levelText.text = "Уровень " + _currentLevel;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


}
