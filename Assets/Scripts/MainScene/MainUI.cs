using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    [SerializeField] private TMPro.TextMeshProUGUI jellyCoinsText;

    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject mainUI;

    private Saves _save;

    void Start()
    {
        _save = new Saves();
        levelText.text = "Уровень " + _save.GetCurrentLevel();
        UpdateJellyCoinsText();
    }

    public void StartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void ShopButtonClicked()
    {
        shopUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public void UpdateJellyCoinsText()
    {
        jellyCoinsText.text = _save.GetJellyCoins().ToString();
    }
}
