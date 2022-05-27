using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCardTransition : MonoBehaviour
{
    float timer = 0;

    private void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }

        objs = GameObject.FindGameObjectsWithTag("pcm");
        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }
    }
    void FixedUpdate()
    {
        // go to title screen after 10 seconds
        timer += Time.deltaTime;
        if (timer > 10f)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
