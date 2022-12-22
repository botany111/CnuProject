﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMageAI : Enemy
{
    public enum Status { idle, Chase, Death, Attack, SkillAttack };

    [Header("目前狀態")]
    public Status status;
    public enum Skill { Random, FireBall, small, Teleport, Summon };

    [Header("目前技能狀態")]
    public Skill skill;

    private Transform player;

    private float wait;

    [Header("技能結束等待時間")]
    public float waitTime;
    private float waittime;

    private BoxCollider2D box2D;

    [Header("火球(普通攻擊)")]
    public GameObject FlameArrow;
    public Transform FlameArrowPoint;

    [Header("普通攻擊冷卻時間")]
    public float NoramlAttackCD;
    private float normalAttackCD;

    [Header("普通攻擊幾次後施放技能")]
    public int NormalAttackNum;
    private int normalAttackNum;

    [Header("火球術")]
    public GameObject fireBall;
    public Transform firePoint;
    private int firePointNum;
    [Header("打斷火球術傷害")]
    public int Disrupt;
    public static int disrupt;

    [Header("縮小術")]
    private bool isSmall = false;

    [Header("縮小時間")]
    public float smillTime;
    private float smilltime;

    private GameObject[] fire;

    private PolygonCollider2D AttackColl;

    // public float time;


    // public float turnSpeed = 0.1f;


    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();

        status = Status.Chase;

        player = GameObject.Find("player").GetComponent<Transform>();

        box2D = GetComponent<BoxCollider2D>();

        AttackColl = GetComponent<PolygonCollider2D>();

        wait = waitTime;

        transform.localRotation = Quaternion.Euler(0, 180, 0);




    }

    // Update is called once per frame
    public new void Update()
    {


        base.Update();

        if (health <= 0)
        {
            status = Status.Death;
        }
        //Debug.Log(status);

        Smillimg();



        Debug.Log(smilltime);

        switch (status)
        {
            case Status.idle:


                disrupt = 0;
                firePointNum = 0;
                normalAttackNum = 0;

                waittime -= Time.deltaTime;
                //anim.SetBool("Attack", false);
                if (myTransform)
                {
                    if (Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) < Distance && waittime <= 0f)
                    {
                        status = Status.Chase;
                    }
                }
                break;

            case Status.Chase:

                if (PlayerTransform)
                {
                    if (myTransform.position.x >= PlayerTransform.position.x)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        face = Face.Left;
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        face = Face.Right;
                    }
                }
                switch (face)
                {
                    case Face.Right:
                        myTransform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                        break;
                    case Face.Left:
                        myTransform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                        break;
                }

                if (Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) > Distance)
                {
                    status = Status.idle;
                }
                if (Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) <= 8)
                {
                    status = Status.Attack;
                }
                break;

            case Status.Attack:

                if (normalAttackCD <= 0)
                {
                    anim.SetTrigger("Attack");
                    normalAttackCD = NoramlAttackCD;
                    normalAttackNum++;
                }

                //Debug.Log(normalAttackCD);
                normalAttackCD -= Time.deltaTime;

                if (normalAttackNum >= NormalAttackNum)
                {
                    status = Status.SkillAttack;
                    //skill = Skill.Random;
                }

                if (Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) >= 8)
                {
                    anim.SetBool("Attack", false);
                    status = Status.idle;
                }
                break;

            case Status.SkillAttack:

                switch (skill)
                {
                    case Skill.Random:
                        break;

                    case Skill.FireBall:

                        anim.SetBool("FireBall", true);

                        DisruptFireBall();

                        break;

                    case Skill.small:

                        anim.SetBool("Small", true);


                        break;

                    case Skill.Teleport:
                        break;

                    case Skill.Summon:
                        break;
                }

                break;

            case Status.Death:
                box2D.enabled = false;
                anim.SetTrigger("Death");
                break;
        }
    }


    void FireAttack()
    {
        Instantiate(FlameArrow, FlameArrowPoint.position, FlameArrowPoint.rotation);
    }

    //火球術(動畫方法)
    void FireBall()
    {
        if (GameObject.Find("FireBall(Clone)") == null)
        {
            Instantiate(fireBall, firePoint.position, firePoint.rotation);
        }
        else
        {
            firePointNum++;
        }

        if (firePointNum == 2)
        {
            var i = GameObject.Find("FireBall(Clone)").GetComponent<Transform>().localScale = new Vector3(3, 3, 3);
        }

        if (firePointNum == 4)
        {
            anim.SetBool("FireBall", false);
            status = Status.idle;
            var i = GameObject.Find("FireBall(Clone)").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            waittime = waitTime;
        }
        Debug.Log(disrupt);
    }

    //火球術(打斷)
    void DisruptFireBall()
    {
        if (disrupt > Disrupt)
        {
            status = Status.idle;
            anim.SetBool("FireBall", false);

            anim.SetTrigger("Hit");
            if (GameObject.Find("FireBall(Clone)"))
            {
                Destroy(GameObject.Find("FireBall(Clone)"), 2);
            }

            waittime = waitTime;
        }
    }

    //變小術(動畫方法)
    void Small()
    {

        var i = GameObject.Find("player").GetComponent<Transform>().localScale = new Vector2(1, 1);
        
        isSmall = true;

        if(isSmall == true)
        {
            smilltime = smillTime;
            anim.SetBool("Small", false);
            status = Status.idle;            
        }

    }

    //變小術(還原)
    void Smillimg()
    {
        if (isSmall == true && smilltime <= 0)
        {
            isSmall = false;

            player.localScale = new Vector2(5, 5);

            player.position = new Vector2(player.position.x, player.position.y + 2);

        }
        else if (isSmall == true)
        {
            smilltime -= Time.deltaTime;
        }
    }

}