using System.Collections;
using UnityEngine.Monetization;
using UnityEngine.Advertisements;
using UnityEngine;
using System;

public class PlayerShipBuild : MonoBehaviour
{

    [SerializeField] private GameObject[] shopButtons;
    private GameObject target;
    private GameObject tmpSelection;
    private GameObject textBoxPanel;
    [SerializeField] private GameObject[] visualWeapons;
    [SerializeField] private SOActorModel defaultPlayerShip;
    private GameObject playerShip;
    private GameObject buyButton;
    private GameObject bankObj;
    private int bank = 600;
    private bool purchaseMade = false;
    private string placementId_rewardedvideo = "rewardedVideo";
    private string gameId = "1234567";


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        TurnOffSelectionHighlights();
        textBoxPanel = GameObject.Find("textBoxPanel");
        purchaseMade = false;
        bankObj = GameObject.Find("bank");
        bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
        buyButton = textBoxPanel.transform.Find("BUY ?").gameObject;

        TurnOffPlayerShipVisuals();
        PreparePlayerShipForUpgrade();

        CheckPlatform();
    }

    private void CheckPlatform()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameId = "3810418";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            gameId = "3810419";
        }

        Monetization.Initialize(gameId, false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        AttemptSelection();
    }

    private void PreparePlayerShipForUpgrade()
    {
        playerShip = GameObject.Instantiate(Resources.Load("Prefab/Player/Player_Ship")) as GameObject;
        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0);
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
    }

    private void TurnOffPlayerShipVisuals()
    {
        for (int i = 0; i < visualWeapons.Length; i++)
        {
            visualWeapons[i].gameObject.SetActive(false);
        }
    }



    private GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 100, out hit))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }

    private void AttemptSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);

            if (target != null)
            {
                if (target.transform.Find("itemText"))
                {
                    TurnOffSelectionHighlights();
                    Select();
                    UpdateDescriptionBox();


                    //NOT ALREADY SOLD
                    if (target.transform.Find("itemText").GetComponent<TextMesh>().text != "SOLD")
                    {
                        //CAN AFFORD
                        Affordable();
                        //CAN'T AFFORD
                        LackOfCredits();
                    }
                    else if (target.transform.Find("itemText").GetComponent<TextMesh>().text == "SOLD")
                    {
                        SoldOut();
                    }
                }
                else if (target.name == "WATCH AD")
                {
                    WatchAdvert();
                }
                else if (target.name == "BUY ?")
                {
                    BuyItem();
                }
                else if (target.name == "START")
                {
                    StartGame();
                }

            }
        }
    }

    private void StartGame()
    {
        if (purchaseMade)
        {
            playerShip.name = "UpgradedShip";
            if (playerShip.transform.Find("energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health = 2;
            }

            DontDestroyOnLoad(playerShip);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("testLevel");
    }

    private void BuyItem()
    {
        Debug.Log("PURCHSED");
        purchaseMade = true;
        buyButton.SetActive(false);
        tmpSelection.SetActive(false);

        for (int i = 0; i < visualWeapons.Length; i++)
        {
            if (visualWeapons[i].name == tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName)
            {
                visualWeapons[i].SetActive(true);
            }
        }

        UpgradeToShip(tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName);

        bank = bank - System.Int32.Parse(tmpSelection.transform.parent.GetComponent<ShopPiece>().ShopSelection.cost);

        bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bank.ToString();
        tmpSelection.transform.parent.transform.Find("itemText").GetComponent<TextMesh>().text = "SOLD";
    }

    private void UpgradeToShip(string upgrade)
    {
        GameObject shipItem = GameObject.Instantiate(Resources.Load("Prefab/Player/" + upgrade)) as GameObject;
        shipItem.transform.SetParent(playerShip.transform);
        shipItem.transform.localPosition = Vector3.zero;
    }

    private void WatchAdvert()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            ShowRewardedAds();
        }
    }

    private void ShowRewardedAds()
    {
        StartCoroutine(WaitForAd());
    }

    private IEnumerator WaitForAd()
    {
        string placementId = placementId_rewardedvideo;
        while (!Advertisement.IsReady(placementId))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;

        ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show(AdFinished);
        }
    }

    private void AdFinished(UnityEngine.Monetization.ShowResult result)
    {
        if (result == UnityEngine.Monetization.ShowResult.Finished)
        {
            bank += 300;
            bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
            TurnOffSelectionHighlights();
        }
    }

    private void SoldOut()
    {
        Debug.Log("SOLD OUT");
    }

    private void LackOfCredits()
    {
        if (bank < System.Int32.Parse(target.transform.Find("itemText").GetComponent<TextMesh>().text))
        {
            Debug.Log("CAN'T BUY");
        }
    }

    private void Affordable()
    {
        if (bank >= System.Int32.Parse(target.transform.GetComponent<ShopPiece>().ShopSelection.cost))
        {
            Debug.Log("Can buy");
            buyButton.SetActive(true);
        }
    }

    private void UpdateDescriptionBox()
    {
        textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.iconName;
        textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.description;

    }

    private void Select()
    {
        tmpSelection = target.transform.Find("SelectionQuad").gameObject;
        tmpSelection.SetActive(true);
    }

    private void TurnOffSelectionHighlights()
    {
        for (int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].SetActive(false);
        }
    }

}
