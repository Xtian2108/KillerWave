using UnityEngine;

public class GameManager : SingletonComponent<GameManager>
{
    public static int playerLives = 3;
    public static int currentScene = 0;
    public static int gameLevelScene = 3;
    private bool _died = false;
    public bool Died
    {
        get { return _died; }
        set { _died = value; }
    }

    public void LifeLost()
    {
        //lose life
        if (playerLives >= 1)
        {
            playerLives--;
            Debug.Log("Lives left:" + playerLives);
            GetComponent<ScenesManager>().ResetScene();
        }
        else
        {
            GetComponent<ScenesManager>().GameOver();
            //reset lives back to 3. 
            playerLives = 3;
        }
    }
}
