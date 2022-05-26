using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
