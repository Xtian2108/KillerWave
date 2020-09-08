using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int playerScore;
    public int PlayersScore
    {
        get
        {
            return playerScore;
        }
    }

    public void SetScore(int incomingScore)
    {
        playerScore += incomingScore;
    }

    public void ResetScore()
    {
        playerScore = 0000000;
    }
}
