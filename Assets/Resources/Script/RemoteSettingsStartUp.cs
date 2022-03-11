using UnityEngine;

public class RemoteSettingsStartUp : MonoBehaviour 
{
  void Awake()
  {
    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
		{
      if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
          Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
      {
        RemoteSettings.Updated += () =>  
        { 
          GameManager.playerLives = RemoteSettings.GetInt("PlayersStartUpLives",GameManager.playerLives);
        };
      }
    }
  }
}