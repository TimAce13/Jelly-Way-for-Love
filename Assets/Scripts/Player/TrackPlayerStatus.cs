using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackPlayerStatus : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finish")
            SceneManager.LoadScene(0);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
    }
}
