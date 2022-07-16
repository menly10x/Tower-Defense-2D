using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class ArcherTowerController : MonoBehaviour
{
    public int level;
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    List<GameObject> monsters = new List<GameObject>();
    public GameObject arrow;
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
            StartCoroutine(SetShoot(monster));
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

    IEnumerator SetShoot(GameObject monster)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        Spine.TrackEntry trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_start", false);

        yield return new WaitForSpineAnimationComplete(trackEntry);

        skeletonAnimation.state.SetAnimation(0, "attack_idle", true);

        arrow.transform.localPosition = new Vector3(0, 0, 0);
        Vector3 direct = monster.transform.position - arrow.transform.position;
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        arrow.SetActive(true);
        arrow.transform.DOMove(monster.transform.position, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            arrow.SetActive(false);
            if (monster.GetComponentInParent<MonsterController>().Health > 0)
            {
                monster.GetComponentInParent<MonsterController>().Health -= damage;
            }

            trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_end", false);
        });

        yield return new WaitForSpineAnimationComplete(trackEntry);

        skeletonAnimation.state.SetAnimation(0, "idle", true);
    }

}
