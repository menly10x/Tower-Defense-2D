using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTowerController : MonoBehaviour
{
    public int level;
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    public GameObject bullet;
    List<GameObject> monsters = new List<GameObject>();
    float countDown = 0f;
    int damage = 5;

    List<float> angles = new List<float>();

    IEnumerator idleAnimCoroutine;
    bool isIdle = false;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        SetIdle(level);

        angles.Add(0);
        angles.Add(45);
        angles.Add(90);
        angles.Add(135);
        angles.Add(180);
        angles.Add(225);
        angles.Add(270);
        angles.Add(315);
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
        else if (!isIdle)
        {
            SetIdle(level);
        }
    }

    void Shoot(GameObject monster)
    {
        isIdle = false;
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
        isIdle = true;

        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();

        if (level < 3)
        {
            idleAnimCoroutine = SetIdleLoop(skeletonAnimation, 0.1f);
            StartCoroutine(idleAnimCoroutine);
        }
        else
        {
            skeletonAnimation.state.SetAnimation(0, "idle", true);
        }
    }

    IEnumerator SetShoot(GameObject monster)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();

        if (level < 3)
        {
            
            StopCoroutine(idleAnimCoroutine);

            Vector3 direct = monster.transform.position - transform.position;
            float angle = Mathf.Abs(90 - (Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg));

            Spine.TrackEntry trackEntry;

            float minAngle = Mathf.Abs(angles[0] - angle);
            float angleToRotate = 0;
            foreach (float angleTemp in angles)
            {
                if (Mathf.Abs(angleTemp - angle) < minAngle)
                {
                    minAngle = Mathf.Abs(angleTemp - angle);
                    angleToRotate = angleTemp;
                }
            }

            if (angleToRotate == 0)
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_08", false);
            }
            else if (angleToRotate == 45)
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_01", false);
            }
            else if (angleToRotate == 90)
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_02", false);
            }
            else if (angleToRotate == 135)
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_03", false);
            }
            else if (angleToRotate == 180)
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_04", false);
            }
            else if (angleToRotate == 225)
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_05", false);
            }
            else if (angleToRotate == 270)
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_06", false);
            }
            else
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_07", false);
            }

            yield return null;

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
        else
        {

        }
    }

    IEnumerator SetIdleLoop(SkeletonAnimation skeletonAnimation, float delayTime)
    {
        while (true)
        {
            skeletonAnimation.state.SetAnimation(0, "idle_01", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_02", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_03", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_04", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_05", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_06", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_07", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_08", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_01", false);

            yield return new WaitForSeconds(2f);
        }
    }
}
