using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Text ScoreBoard;

    private int player1Score, player2Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * -100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        ScoreBoard.text = "Player 1: " + player1Score + ", Player 2: " + player2Score;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player 1 Goal"))
        {
            player1Score++;
        }
        else if (other.gameObject.CompareTag("Player 2 Goal"))
        {
            player2Score++;
        }

        resetBall();
    }

    private void resetBall()
    {
        this.transform.position = new Vector3(0, 0, 0);
    }
}
