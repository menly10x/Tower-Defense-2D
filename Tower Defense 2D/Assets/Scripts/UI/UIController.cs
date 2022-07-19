using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject btnBuyTower;
    public GameObject btnUpgradeAndSellTower;
    public GameObject btnUpgradeTower;
    public GameObject btnSellTower;
    [SerializeField]
    int towerPlacementIndex;
    GameObject currentTower;

    public Text txtWave;
    public Text txtHealth;
    public Text txtWood;

    public GameObject btnBuyCanonTower;
    public GameObject btnBuyArcherTower;
    public GameObject btnBuyMagicTower;
    public GameObject btnBuyLightningTower;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenBtnBuyTower(Transform targetPosition, int placementIndex)
    {
        CheckBtnBuy();

        btnBuyTower.transform.DOKill();
        btnBuyTower.SetActive(false);
        btnBuyTower.transform.position = targetPosition.position;
        btnBuyTower.transform.localScale = new Vector3(0, 0, 0);
        btnBuyTower.SetActive(true);
        btnBuyTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        towerPlacementIndex = placementIndex;
    }

    void CheckBtnBuy()
    {
        btnBuyArcherTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyArcherTower.GetComponent<AnimatedButton>().interactable = true;

        btnBuyCanonTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyCanonTower.GetComponent<AnimatedButton>().interactable = true;

        btnBuyMagicTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyMagicTower.GetComponent<AnimatedButton>().interactable = true;

        btnBuyLightningTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        btnBuyLightningTower.GetComponent<AnimatedButton>().interactable = true;

        if (TowerManager.instance.archerPrice > PlayerSetting.instance.Coin)
        {
            btnBuyArcherTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyArcherTower.GetComponent<AnimatedButton>().interactable = false;
        }
        if (TowerManager.instance.canonPrice > PlayerSetting.instance.Coin)
        {
            btnBuyCanonTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyCanonTower.GetComponent<AnimatedButton>().interactable = false;
        }
        if (TowerManager.instance.magicPrice > PlayerSetting.instance.Coin)
        {
            btnBuyMagicTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyMagicTower.GetComponent<AnimatedButton>().interactable = false;
        }
        if (TowerManager.instance.lightningPrice > PlayerSetting.instance.Coin)
        {
            btnBuyLightningTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnBuyLightningTower.GetComponent<AnimatedButton>().interactable = false;
        }
    }

    public void CloseBtnBuyTower()
    {
        btnBuyTower.transform.DOKill();
        btnBuyTower.SetActive(false);
    }

    public void ButtonBuyTower(int index)
    {
        CloseBtnBuyTower();
        switch (index)
        {
            case 1:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.barackTower, towerPlacementIndex));
                break;
            case 2:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.archerTower, towerPlacementIndex));
                break;
            case 3:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.canonTower, towerPlacementIndex));
                break;
            case 4:
                StartCoroutine(TowerManager.instance.SpawnTower(TowerManager.instance.magicTower, towerPlacementIndex));
                break;
        }
    }

    float priceToUpgrade;
    float priceToSell;

    public void OpenBtnUpgradeAndSellTower(Transform targetPosition, GameObject tower)
    {
        SetPrice(tower);
        btnUpgradeAndSellTower.transform.DOKill();
        btnUpgradeAndSellTower.SetActive(false);
        btnUpgradeAndSellTower.transform.position = targetPosition.position;
        btnUpgradeAndSellTower.transform.localScale = new Vector3(0, 0, 0);
        btnUpgradeAndSellTower.SetActive(true);
        btnUpgradeAndSellTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        currentTower = tower;
    }

    void SetPrice(GameObject tower)
    {
        if (tower.GetComponent<ArcherTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<ArcherTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<ArcherTowerController>().price;
        }
        else if (tower.GetComponent<CanonTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<CanonTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<CanonTowerController>().price;
        }
        else if (tower.GetComponent<MagicTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<MagicTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<MagicTowerController>().price;
        }
        else if (tower.GetComponent<LightningTowerController>() != null)
        {
            priceToUpgrade = tower.GetComponent<LightningTowerController>().priceToUpgrade;
            priceToSell = tower.GetComponent<LightningTowerController>().price;
        }

        btnUpgradeTower.transform.GetChild(2).GetComponent<Text>().text = priceToUpgrade.ToString();
        btnSellTower.transform.GetChild(2).GetComponent<Text>().text = priceToSell.ToString();

        if (PlayerSetting.instance.Coin < priceToUpgrade)
        {
            btnUpgradeTower.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            btnUpgradeTower.GetComponent<AnimatedButton>().interactable = false;
        }
        else
        {
            btnUpgradeTower.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            btnUpgradeTower.GetComponent<AnimatedButton>().interactable = true;
        }

        Transform towerLevel = tower.transform.GetChild(0);

        for (int i = 0; i < towerLevel.childCount; i++)
        {
            if (towerLevel.GetChild(i).gameObject.activeSelf)
            {
                if (i == towerLevel.childCount - 1)
                {
                    btnUpgradeTower.SetActive(false);
                }
                else
                {
                    btnUpgradeTower.SetActive(true);
                }
                break;
            }
        }
    }

    public void CloseBtnUpgradeAndSellTower()
    {
        btnUpgradeAndSellTower.transform.DOKill();
        btnUpgradeAndSellTower.SetActive(false);
    }

    public void ButtonUpgradeTower()
    {
        PlayerSetting.instance.Coin -= priceToUpgrade;
        CloseBtnUpgradeAndSellTower();
        StartCoroutine(TowerManager.instance.UpgradeTower(currentTower));
    }

    public void ButtonSellTower()
    {
        PlayerSetting.instance.Coin += priceToSell;
        CloseBtnUpgradeAndSellTower();
        Destroy(currentTower);
        TowerManager.instance.towerPlacementParent.GetChild(towerPlacementIndex).gameObject.SetActive(true);
    }
}
