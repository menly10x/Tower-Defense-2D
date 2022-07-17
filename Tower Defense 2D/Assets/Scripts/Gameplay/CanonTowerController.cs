using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class CanonTowerController : MonoBehaviour
{
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    public GameObject bullet1;
    List<GameObject> monsters = new List<GameObject>();
    float countDown = 0f;

    SkeletonAnimation effectShoot;

    List<float> angles = new List<float>();

    IEnumerator idleAnimCoroutine;
    bool isIdle = false;

    private int level;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            LoadDataTower();
        }
    }

    private float damage;
    private float fireRate;
    private float fireRange;
    private float buyPrice;
    private float sellPrice;

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        SetIdle(Level);

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
            SetIdle(Level);
        }
    }

    public JSONNode loadTextData(string path)
    {
        TextAsset txt = (TextAsset)Resources.Load(path, typeof(TextAsset));

        return JSONArray.Parse(txt.text);
    }

    public void LoadDataTower()
    {
        JSONNode jsonNode = loadTextData("Data/CanonTower");
        foreach (JSONNode node in jsonNode)
        {
            if (Level == node["Level"].AsInt)
            {
                damage = node["Damage"].AsFloat;
                fireRate = node["FireRate"].AsFloat;
                fireRange = node["FireRange"].AsFloat;
                buyPrice = node["BuyPrice"].AsFloat;
                sellPrice = node["SellPrice"].AsFloat;
            }
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
        Transform spineLevel = spineLevelParent.GetChild(Level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        effectShoot = spineLevelParent.GetChild(Level - 1).GetChild(2).GetComponent<SkeletonAnimation>();

        if (Level < 3)
        {

            StopCoroutine(idleAnimCoroutine);

            Vector3 direct = monster.transform.position - transform.position;
            float angle = Mathf.Abs(90 - (Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg));

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

            switch (angleToRotate)
            {
                case 0:
                    skeletonAnimation.state.SetAnimation(0, "attack_08", false);
                    break;
                case 45:
                    skeletonAnimation.state.SetAnimation(0, "attack_01", false);
                    break;
                case 90:
                    skeletonAnimation.state.SetAnimation(0, "attack_02", false);
                    break;
                case 135:
                    skeletonAnimation.state.SetAnimation(0, "attack_03", false);
                    break;
                case 180:
                    skeletonAnimation.state.SetAnimation(0, "attack_04", false);
                    break;
                case 225:
                    skeletonAnimation.state.SetAnimation(0, "attack_05", false);
                    break;
                case 270:
                    skeletonAnimation.state.SetAnimation(0, "attack_06", false);
                    break;
                case 315:
                    skeletonAnimation.state.SetAnimation(0, "attack_07", false);
                    break;
            }

            effectShoot.state.SetAnimation(0, "play", false);

            yield return null;

            bullet1.transform.localPosition = new Vector3(0, 0, 0);

            bullet1.SetActive(true);
            bullet1.transform.GetChild(0).gameObject.SetActive(true);
            bullet1.transform.GetChild(3).gameObject.SetActive(true);
            Transform effectHit = bullet1.transform.GetChild(2);
            effectHit.gameObject.SetActive(false);
            bullet1.transform.DOJump(monster.transform.position, 1f, 1, 1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(1f);

            bullet1.transform.GetChild(0).gameObject.SetActive(false);

            effectHit.gameObject.SetActive(true);
            SkeletonAnimation effectHit1 = effectHit.GetChild(0).GetComponent<SkeletonAnimation>();
            SkeletonAnimation effectHit2 = effectHit.GetChild(1).GetComponent<SkeletonAnimation>();
            Spine.TrackEntry trackEntry = effectHit1.state.SetAnimation(0, "hit", false);
            effectHit2.state.SetAnimation(0, "hit", false);

            foreach (GameObject monsterTakeDamage in bullet1.GetComponent<CanonBulletController>().monsters)
            {
                if (monsterTakeDamage.GetComponentInParent<MonsterController>().Health > 0)
                {
                    monsterTakeDamage.GetComponentInParent<MonsterController>().TakeDamage(damage);
                }
            }

            bullet1.transform.GetChild(3).gameObject.SetActive(false);

            yield return new WaitForSpineAnimationComplete(trackEntry);

            bullet1.SetActive(false);
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
