using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterSpawnController : MonoBehaviour
{
    public static MonsterSpawnController instance;

    public Transform pathWayParent;

    public Transform monsterParent;
    public GameObject skeletonLeader;
    public GameObject skeleton;

    float countDown = 5f;

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
         StartCoroutine(delaySpawnMonster());
    }

    // Update is called once per frame
    void Update()
    {
        if(countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            countDown = 5f;
            IEnumerator c = delaySpawnMonster();
            StartCoroutine(c); 
        }
    }

    IEnumerator delaySpawnMonster()
    {
        SpawnMonster(skeletonLeader);

        yield return new WaitForSeconds(1f);

        SpawnMonster(skeleton);
    }

    void SpawnMonster(GameObject monster)
    {
        Vector3[] paths = new Vector3[pathWayParent.GetChild(0).childCount];
        for (int i = 0; i < pathWayParent.GetChild(0).childCount; i++)
        {
            paths[i] = pathWayParent.GetChild(0).GetChild(i).position;
        }

        GameObject newMonster = Instantiate(monster, monsterParent);
        newMonster.transform.position = pathWayParent.GetChild(0).GetChild(0).position;
        newMonster.transform.DOPath(paths, 30f, PathType.Linear, PathMode.TopDown2D, 0).OnComplete(() =>
        {
            Destroy(newMonster);
        });
    }
}
