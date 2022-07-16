using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTowerController : MonoBehaviour
{
    int level;
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    public GameObject bullet;
    List<GameObject> monsters = new List<GameObject>();
    float countDown = 0f;
    int damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        SetIdle(level);
    }

    // Update is called once per frame
    void Update()
    {
        if (monsters.Count > 0)
        {
            if (monsters[0] != null)
            {
                Shoot(monsters[0]);
            }
            else
            {
                monsters.RemoveAt(0);
            }
        }
    }

    void Shoot(GameObject monster)
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            countDown = 1.2f;
            bullet.transform.localPosition = new Vector3(0, 0, 0);

            bullet.SetActive(true);
            bullet.transform.DOJump(monster.transform.position, 1f, 1, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                bullet.SetActive(false);
                if (monster.GetComponentInParent<MonsterController>().Health > 0)
                {
                    monster.GetComponentInParent<MonsterController>().Health -= damage;
                }
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Monster"))
        {
            monsters.Add(target.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Monster"))
        {
            GameObject monster = target.gameObject;
            if (monster.GetComponentInParent<MonsterController>().Health <= 0)
            {
                if (monsters.IndexOf(monster) >= 0)
                {
                    monsters.Remove(monster);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Monster"))
        {
            monsters.Remove(target.gameObject);
        }
    }

    void SetIdle(int level)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "idle_01", true);
    }

    void SetAttack(int level)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "attack", false);
    }
}
