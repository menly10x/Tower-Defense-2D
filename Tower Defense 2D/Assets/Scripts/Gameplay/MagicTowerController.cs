using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerController : MonoBehaviour
{
    int level;
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    public GameObject magicBullet;
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
            magicBullet.transform.localPosition = new Vector3(0, 0, 0);

            magicBullet.SetActive(true);
            magicBullet.transform.DOMove(monster.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                magicBullet.SetActive(false);
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
        skeletonAnimation.state.SetAnimation(0, "idle", true);
    }

    void SetAttack(int level)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "attack", false);
    }

}
