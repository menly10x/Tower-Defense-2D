using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject btnBuyTower;
    public GameObject btnUpgradeTower;
    [SerializeField]
    int towerPlacementIndex;
    GameObject currentTower;

    public Text txtWave;
    public Text txtHealth;
    public Text txtWood;

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
        btnBuyTower.transform.DOKill();
        btnBuyTower.SetActive(false);
        btnBuyTower.transform.position = targetPosition.position;
        btnBuyTower.transform.localScale = new Vector3(0, 0, 0);
        btnBuyTower.SetActive(true);
        btnBuyTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        towerPlacementIndex = placementIndex;
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

    public void OpenBtnUpgradeTower(Transform targetPosition, GameObject tower)
    {
        btnUpgradeTower.transform.DOKill();
        btnUpgradeTower.SetActive(false);
        btnUpgradeTower.transform.position = targetPosition.position;
        btnUpgradeTower.transform.localScale = new Vector3(0, 0, 0);
        btnUpgradeTower.SetActive(true);
        btnUpgradeTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        currentTower = tower;
    }

    public void CloseBtnUpgradeTower()
    {
        btnUpgradeTower.transform.DOKill();
        btnUpgradeTower.SetActive(false);
    }

    public void ButtonUpgradeTower()
    {
        CloseBtnUpgradeTower();
        StartCoroutine(TowerManager.instance.UpgradeTower(currentTower));
    }
}
