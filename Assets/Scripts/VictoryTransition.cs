using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>().IsP1() && this.gameObject.name == "Sandwich Yay")
        {
            SceneManager.LoadScene("End Blue Yes");
        }
        else if (collision.gameObject.GetComponent<PlayerControls>().IsP1() && this.gameObject.name == "Sandwich Neigh")
        {
            SceneManager.LoadScene("End Blue No");
        }
        else if (!collision.gameObject.GetComponent<PlayerControls>().IsP1() && this.gameObject.name == "Sandwich Yay")
        {
            SceneManager.LoadScene("End Orange Yes");
        }
        else if (!collision.gameObject.GetComponent<PlayerControls>().IsP1() && this.gameObject.name == "Sandwich Neigh")
        {
            SceneManager.LoadScene("End Orange No");
        }

        // remove everything from DontDestroyOnLoad


    }
}
