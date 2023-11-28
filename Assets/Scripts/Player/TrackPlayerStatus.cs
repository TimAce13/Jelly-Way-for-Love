using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackPlayerStatus : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _particles;

    [SerializeField] private List<SerializableKeyValue> playerObjects = new List<SerializableKeyValue>();

    private Saves save;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finish")
            SceneManager.LoadScene(0);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
    }

    void Start()
    {
        save = new Saves();

        LoadPlayerObjectsFromSave();

        playerObjects.FirstOrDefault(entry => entry.state == 2).PlayerObject.SetActive(true);

        StartCoroutine(PerformActionEveryQuarterSecond());
    }

    private void LoadPlayerObjectsFromSave()
    {
        var dictionaryFromSave = save.GetPlayerObjects();

        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].state = dictionaryFromSave[i];
        }
    }

    IEnumerator PerformActionEveryQuarterSecond()
    {
        while (true)
        {
            Instantiate(_particles, new Vector3(_player.transform.position.x, 0.5f, _player.transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
