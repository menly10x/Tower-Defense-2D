using DG.Tweening;
using SimpleJSON;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerController : MonoBehaviour
{
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    public GameObject magicBullet;
    List<GameObject> monsters = new List<GameObject>();
    float countDown = 0f;

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
    private float price;

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        SetIdle(Level);
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
        Transform spineLevel = spineLevelParent.GetChild(Level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        Spine.TrackEntry trackEntry = skeletonAnimation.state.SetAnimation(0, "attack", false);

        magicBullet.transform.localPosition = new Vector3(0, 0, 0);

        magicBullet.SetActive(true);
        magicBullet.transform.DOMove(monster.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            magicBullet.SetActive(false);
            if (monster.GetComponentInParent<MonsterController>().Health > 0)
            {
                monster.GetComponentInParent<MonsterController>().TakeDamage(damage);
            }
        });

        yield return new WaitForSpineAnimationComplete(trackEntry);

        skeletonAnimation.state.SetAnimation(0, "idle", true);
    }

}
