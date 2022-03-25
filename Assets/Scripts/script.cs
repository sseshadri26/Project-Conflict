using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    private AudioSource[] source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // play the sound
            foreach (AudioSource g in source) { 
                if (g.name == "SFX")
                    g.Play();
            }
        }
    }
}
