using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Shop : MonoBehaviour
{
    private SerializableKeyValue _currentObject;

    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject chooseButton;
    [SerializeField] private GameObject choosenButton;

    [SerializeField] private GameObject priceObject;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private List<SerializableKeyValue> playerObjects = new List<SerializableKeyValue>();

    [SerializeField] private Saves save;

    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject mainUI;

    [SerializeField] private GameObject[] _eventParicles;

    private int amountOfPlayerObjects;

    private void Start()
    {
        LoadPlayerObjectsFromSave();

        amountOfPlayerObjects = playerObjects.Count;

        _currentObject = playerObjects.FirstOrDefault(entry => entry.state == 2);
        _currentObject.PlayerObject.SetActive(true);

        CheckSates();
    }
    private void LoadPlayerObjectsFromSave()
    {
        var dictionaryFromSave = save.GetPlayerObjects();

        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].state = dictionaryFromSave[i];
        }
    }

    public void BackButtonClicked()
    {
        _currentObject.PlayerObject.SetActive(false);

        _currentObject = playerObjects.FirstOrDefault(item => item.state == 2);
        _currentObject.PlayerObject.SetActive(true);

        CheckSates();

        shopUI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void LeftArrowClicked()
    {
        _currentObject.PlayerObject.SetActive(false);
        _currentObject = playerObjects[_currentObject.id - 1];
        _currentObject.PlayerObject.SetActive(true);
        CheckSates();
    }

    public void RightArrowClicked()
    {
        _currentObject.PlayerObject.SetActive(false);
        _currentObject = playerObjects[_currentObject.id + 1];
        _currentObject.PlayerObject.SetActive(true);
        CheckSates();
    }

    public void BuyButtonClicked()
    {
        if(save.GetJellyCoins() >= _currentObject.price)
        {
            _currentObject.state = 1;
            playerObjects[_currentObject.id].state = 1;
            CheckSates();
            SavePlayerObjects();
            save.EditJellyCoins(_currentObject.price * -1);
            SpawnRandomParticle();

        }
    }

    public void ChooseButtonClicked()
    {
        playerObjects.FirstOrDefault(entry => entry.state == 2).state = 1;
        _currentObject.state = 2;
        playerObjects[_currentObject.id].state = 2;
        CheckSates();
        SavePlayerObjects();
        SpawnRandomParticle();
    }

    private void CheckArrowsStates()
    {
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);

        if (_currentObject.id == 0) { leftArrow.SetActive(false); }
        if (_currentObject.id == amountOfPlayerObjects - 1) { rightArrow.SetActive(false); }

    }

    private void CheckGameObjectState()
    {
        if (_currentObject.state == 0)
        {
            buyButton.SetActive(true);
            priceObject.SetActive(true);
            priceText.text = _currentObject.price.ToString();

            chooseButton.SetActive(false);
            choosenButton.SetActive(false);
        }

        if (_currentObject.state == 1) { buyButton.SetActive(false); priceObject.SetActive(false); chooseButton.SetActive(true); choosenButton.SetActive(false); }
        if (_currentObject.state == 2) { buyButton.SetActive(false); priceObject.SetActive(false); chooseButton.SetActive(false); choosenButton.SetActive(true); }
    }

    private void CheckSates()
    {
        CheckArrowsStates();
        CheckGameObjectState();
    }

    private void SpawnRandomParticle()
    {
        var particleID = UnityEngine.Random.Range(0, _eventParicles.Length);

        Destroy(Instantiate(_eventParicles[particleID], new Vector3(0,1.5f,15), Quaternion.identity),1);
    }

    private void SavePlayerObjects()
    {
        var dictionaryPlayerObjects = new Dictionary<int, int>();
        for (int i = 0; i < playerObjects.Count; i++)
        {
            dictionaryPlayerObjects[i] = playerObjects[i].state;
        }
        save.SavePlayerObjects(dictionaryPlayerObjects);
    }
}

