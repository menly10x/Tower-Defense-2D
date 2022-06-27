using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterSpawnController : MonoBehaviour
{
    public static MonsterSpawnController instance;

    public Transform pathWayParent;

    public Transform monsterParent;

    float countDown = 3f;

    int wave;

    public GameObject[] monster;

    public GameObject[] boss;
    public GameObject[] leader;
    public GameObject[] normal;

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
        wave = 1;
        SpawnMonster(monster[Random.Range(0, monster.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            countDown = 3f;
            SpawnMonster(monster[Random.Range(0, monster.Length)]);
        }
    }

    void SpawnMonster(GameObject monster)
    {
        Transform path = pathWayParent.GetChild(Random.Range(0, pathWayParent.childCount));
        Vector3[] wayPoints = new Vector3[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            wayPoints[i] = path.GetChild(i).position;
        }
        GameObject newMonster = Instantiate(monster, monsterParent);
        newMonster.transform.position = path.GetChild(0).position;
        newMonster.GetComponent<MonsterController>().wayPoints = wayPoints;
    }

}
