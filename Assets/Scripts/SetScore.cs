using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetScore : MonoBehaviour
{
    TMP_Text player1;
    TMP_Text player2;
    private int p1Score = 0;
    private int p2Score = 0;

    // Start is called before the first frame update
    void Start()
    {
        player1 = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        player2 = this.gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        player1.text = "" + p1Score;
        player2.text = "" + p2Score;
    }

    public void updateScore(int p1, int p2)
    {
        p1Score = p1;
        p2Score = p2;
    }
    public int getp1Score()
    {
        return p1Score;
    }
    public int getp2Score()
    {
        return p2Score;
    }

    public void addP1Score(int i)
    {
        p1Score += i;
    }

    public void addP2Score(int i)
    {
        p2Score += i;
    }

}
