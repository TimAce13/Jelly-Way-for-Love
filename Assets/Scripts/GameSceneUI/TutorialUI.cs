using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private Saves _save;

    private int _level;

    [SerializeField] GameObject[] tutorial_level_1;
    
    [SerializeField] GameObject continueButton;

    private GameObject[] _currentTutorial;

    private int index = 0;
    void Start()
    {
        _save = new Saves();

        _level = _save.GetCurrentLevel();
        Debug.Log(_level.ToString());

        StartTutorial();
    }

    private void StartTutorial()
    {
        if(_level == 1)
        {
            _currentTutorial = tutorial_level_1;
            continueButton.SetActive(true);
            ContinueButton();
        }
    }

    public void ContinueButton()
    {
        Debug.Log(_currentTutorial.Length);
        if (index < _currentTutorial.Length)
        {
            _currentTutorial[index].SetActive(true);
            if (index > 0)
            {
                _currentTutorial[index - 1].SetActive(false);
            }
            index++;
        }
        else
        {
            continueButton.SetActive(false);
            _currentTutorial[index - 1].SetActive(false);
        }
    }
}
