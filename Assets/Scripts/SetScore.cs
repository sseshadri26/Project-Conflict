using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetScore : MonoBehaviour
{
    TMP_Text player1;
    TMP_Text player2;

    // Start is called before the first frame update
    void Start()
    {
        player1 = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        player2 = this.gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();    
    }

    public void updateScore(int p1Score, int p2Score) {
        player1.text = ""+p1Score;
        player2.text = ""+p2Score;
    }
}
