using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Delete all objects with tag player, weapon, or pcm
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("weapon");
        GameObject[] pcm = GameObject.FindGameObjectsWithTag("pcm");
        foreach (GameObject player in players)
        {
            Destroy(player);
        }
        foreach (GameObject weapon in weapons)
        {
            Destroy(weapon);
        }
        foreach (GameObject p in pcm)
        {
            Destroy(p);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GoToNextScene()
    {
        //go to PlayerSetup Scene
        SceneManager.LoadScene("PlayerSetup");
    }
}
