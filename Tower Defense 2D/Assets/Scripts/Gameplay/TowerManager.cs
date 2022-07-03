using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public Transform towerParent;
    public Transform towerPlacementParent;

    public GameObject barackTower;
    public GameObject archerTower;
    public GameObject canonTower;
    public GameObject magicTower;

    public GameObject effectSpawnTower;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public IEnumerator SpawnTower(GameObject tower, int towerPlacementIndex)
    {
        Transform towerPlacement = towerPlacementParent.GetChild(towerPlacementIndex);

        GameObject effectSpawn = Instantiate(effectSpawnTower);
        effectSpawn.transform.position = towerPlacement.position;

        yield return new WaitForSeconds(0.4f);

        GameObject newTower = Instantiate(tower, towerParent);
        towerPlacement.gameObject.SetActive(false);
        newTower.transform.position = towerPlacement.position;
        newTower.GetComponent<TowerController>().towerPlacementIndex = towerPlacementIndex;
    }

    public IEnumerator UpgradeTower(GameObject tower)
    {
        GameObject effectSpawn = Instantiate(effectSpawnTower);
        effectSpawn.transform.position = tower.transform.position;

        yield return new WaitForSeconds(0.4f);

        Transform towerLevel = tower.transform.GetChild(0);
        for (int i = 0; i < towerLevel.childCount - 1; i++)
        {
            if (towerLevel.GetChild(i).gameObject.activeSelf)
            {
                if (towerLevel.GetChild(i + 1) != null)
                {
                    towerLevel.GetChild(i).gameObject.SetActive(false);
                    towerLevel.GetChild(i + 1).gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
