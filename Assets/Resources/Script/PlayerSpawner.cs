using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public SOActorModel actorModel;
    private GameObject playerShip;
    private bool upgradedShip = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        CreatePlayer();
        GetComponentInChildren<Player>().enabled = true;
    }

    private void CreatePlayer()
    {
        // //CREATE PLAYER
        // actorModel = GameObject.Instantiate(Resources.Load("Script/ScriptableObject/Player_Default")) as SOActorModel;
        // playerShip = GameObject.Instantiate(actorModel.actor) as GameObject;
        // playerShip.GetComponent<Player>().ActorStats(actorModel);

        // //SET PLAYER UP
        // playerShip.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        // playerShip.transform.localScale = new Vector3(60f, 60f, 60f);
        // playerShip.name = "Player";
        // playerShip.transform.SetParent(this.transform);
        // playerShip.transform.position = Vector3.zero;

        //been shopping
        if (GameObject.Find("UpgradedShip"))
        {
            upgradedShip = true;
        }

        //not shopped or died
        //default ship build
        if (!upgradedShip || GameManager.Instance.Died)
        {
            GameManager.Instance.Died = false;
            actorModel = GameObject.Instantiate(Resources.Load("Script/ScriptableObject/Player_Default")) as SOActorModel;
            playerShip = GameObject.Instantiate(actorModel.actor, this.transform.position, Quaternion.Euler(270, 180, 0)) as GameObject;
            playerShip.GetComponent<IActorTemplate>().ActorStats(actorModel);
        }
        else
        {
            playerShip = GameObject.Find("UpgradedShip");
            playerShip.transform.rotation = Quaternion.Euler(0, 180, 0);
            playerShip.transform.localScale = new Vector3(60, 60, 60);
            // playerShip.GetComponentInChildren<ParticleSystem>
            //   ().transform.localScale = new Vector3(25, 25, 25);
            playerShip.name = "Player";
            playerShip.transform.SetParent(this.transform);
            playerShip.transform.position = Vector3.zero;
        }
    }
}
