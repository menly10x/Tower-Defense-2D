using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed;
    SkeletonAnimation skeletonAnimation;
    public Vector3[] wayPoints;

    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        Move();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move()
    {
        SetWalk();
        float timeToMove = CalculateDistance(wayPoints) / moveSpeed;
        transform.DOPath(wayPoints, timeToMove, PathType.Linear, PathMode.TopDown2D, 0, Color.red).OnWaypointChange(MyCallBack).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    void MyCallBack(int waypointIndex)
    {
        float scaleX = Mathf.Abs(skeletonAnimation.gameObject.transform.localScale.x);
        // face right
        if (wayPoints[waypointIndex + 1].x - wayPoints[waypointIndex].x > 0)
        {
            skeletonAnimation.gameObject.transform.localScale = new Vector3(-scaleX, skeletonAnimation.gameObject.transform.localScale.y, skeletonAnimation.gameObject.transform.localScale.z);
        }
        // face left
        else
        {
            skeletonAnimation.gameObject.transform.localScale = new Vector3(scaleX, skeletonAnimation.gameObject.transform.localScale.y, skeletonAnimation.gameObject.transform.localScale.z);
        }
    }

    float CalculateDistance(Vector3[] wayPoints)
    {
        float distance = 0;
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            distance += Vector3.Distance(wayPoints[i], wayPoints[i + 1]);
        }
        return distance;
    }

    void SetWalk()
    {
        skeletonAnimation = transform.GetChild(0).GetChild(0).GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "walk", true);
    }
}
