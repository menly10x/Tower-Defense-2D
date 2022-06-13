using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject btnBuyTower;
    [SerializeField]
    int towerPlacementIndex;

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
}
