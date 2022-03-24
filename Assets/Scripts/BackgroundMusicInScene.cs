using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicInScene : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}
