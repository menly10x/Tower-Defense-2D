﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ArcherTowerController : MonoBehaviour
{
    int level;
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    List<GameObject> monsters;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        SetIdle(level);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetIdle(int level)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "idle", true);
    }

    void SetAttackStart(int level)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "attack_start", true);
    }
    
    void SetAttackIdle(int level)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "attack_idle", true);
    }
    
    void SetAttackEnd(int level)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "attack_end", true);
    }

    //private void OnTriggerEnter2D(Collider2D target)
    //{
    //    if (target.gameObject.tag.Equals("Monster"))
    //    {
    //        monsters.Add(target.gameObject);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D target)
    //{
    //    if (target.gameObject.tag.Equals("Monster"))
    //    {
    //        monsters.Remove(target.gameObject);
    //    }
    //}
}