﻿using DG.Tweening;
using SimpleJSON;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerController : MonoBehaviour
{
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    public Transform bulletParent;
    public GameObject magicBullet1;
    public GameObject magicBullet2;
    public GameObject magicBullet3;
    float bulletSpeed = 2f;
    List<GameObject> monsters = new List<GameObject>();
    float countDown = 0f;

    public Transform attackRange;

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
            SetIdle();

            attackRange.localScale = new Vector2(fireRange, fireRange);
            if (MonsterSpawnController.instance.WaveSpawn > 5)
            {
                damage += damage * MonsterSpawnController.instance.WaveSpawn * 2 / 100;
            }
        }
    }

    public float damage;
    private float fireRate;
    private float fireRange;
    public float price;
    public float priceToUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        
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

    public JSONNode loadTextData(string path)
    {
        TextAsset txt = (TextAsset)Resources.Load(path, typeof(TextAsset));

        return JSONArray.Parse(txt.text);
    }

    public void LoadDataTower()
    {
        JSONNode jsonNode = loadTextData("Data/MagicTower");
        foreach (JSONNode node in jsonNode)
        {
            if (Level == node["Level"].AsInt)
            {
                damage = node["Damage"].AsFloat;
                fireRate = node["FireRate"].AsFloat;
                fireRange = node["FireRange"].AsFloat;
                price = node["Price"].AsFloat;
            }
            if (node["Level"].AsInt == (Level + 1))
            {
                priceToUpgrade = node["Price"].AsFloat;
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

    void SetIdle()
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "idle", true);
    }

    IEnumerator SetShoot(GameObject monster)
    {
        countDown = fireRate;

        Transform spineLevel = spineLevelParent.GetChild(Level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "attack", false);

        GameObject bullet;

        switch (Level)
        {
            case 1:
                bullet = Instantiate(magicBullet1, bulletParent);
                break;
            case 2:
            case 4:
                bullet = Instantiate(magicBullet2, bulletParent);
                break;
            case 3:
                bullet = Instantiate(magicBullet3, bulletParent);
                break;
            default:
                bullet = Instantiate(magicBullet1, bulletParent);
                break;
        }

        bullet.transform.localPosition = new Vector3(0, 0, 0);

        float distance = Vector2.Distance(bullet.transform.position, monster.transform.position);
        float time = distance / bulletSpeed;

        AudioController.instance.PlaySound("magicShoot");

        bullet.transform.DOMove(monster.transform.position, time).SetEase(Ease.Linear);

        yield return new WaitForSeconds(time);

        AudioController.instance.PlaySound("magicHit");

        Destroy(bullet);

        if (monster.GetComponentInParent<MonsterController>().Health > 0)
        {
            monster.GetComponentInParent<MonsterController>().TakeDamage(damage);
        }

        skeletonAnimation.state.SetAnimation(0, "idle", true);
    }

}
