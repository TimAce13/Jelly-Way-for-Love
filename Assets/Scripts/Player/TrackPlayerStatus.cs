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

    private int _maxAliveParticlesAmount = 6;
    private int _curAliveParticlesAmount = 0;

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

        var _particlesAlive = new List<GameObject>();

        while (_curAliveParticlesAmount < _maxAliveParticlesAmount)
        {
            _curAliveParticlesAmount += 1;
            var _p = Instantiate(_particles, new Vector3(-100, -10 - 100), Quaternion.identity);
            _particlesAlive.Add(_p);
        }

        StartCoroutine(RespawnParticles(_particlesAlive));
    }

    private void LoadPlayerObjectsFromSave()
    {
        var dictionaryFromSave = save.GetPlayerObjects();

        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].state = dictionaryFromSave[i];
        }
    }

    IEnumerator RespawnParticles(List<GameObject> _particlesAlive)
    {
        while (true)
        {
            var _p = _particlesAlive[0];
            _particlesAlive.RemoveAt(0);
            _p.transform.position = new Vector3(_player.transform.position.x, 2f, _player.transform.position.z);
            _p.GetComponent<ParticleSystem>().Play();
            _particlesAlive.Add(_p);
            yield return new WaitForSeconds(0.5f); // Wait for 0.25 seconds
        }
    }
}
