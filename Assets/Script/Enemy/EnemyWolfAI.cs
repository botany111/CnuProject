﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWolfAI : Enemy
{
    public enum Status { idle, walk, patrol };

    public Status status;

    private Transform myTransform;

    public Transform PlayerTransform;
    private SpriteRenderer spr;

    public Transform[] moveSpots;

    public int i = 0;

    public float Distance;

    private float wait;
    public float waitTime = 5;
    private bool movingRight = true;


    public enum Face { Right, Left }
    public Face face;
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();

        status = Status.idle;

        spr = this.transform.GetComponent<SpriteRenderer>();

        wait = waitTime;
        if (spr.flipX)
        {
            face = Face.Left;
        }
        else
        {
            face = Face.Right;

        }
        myTransform = this.transform;
        if (GameObject.Find("player") != null)
        {
            PlayerTransform = GameObject.Find("player").transform;
        }

    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();

        //Debug.Log(status);

        switch (status)
        {
            case Status.idle:
                if (myTransform)
                {
                    if (Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) < Distance)
                    {
                        status = Status.walk;
                    }
                    else
                    {
                        status = Status.patrol;
                    }
                    if (Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) <= 1.2)
                    {
                        status = Status.idle;
                    }
                }
                break;

            case Status.walk:
                if (PlayerTransform)
                {
                    if (myTransform.position.x >= PlayerTransform.position.x)
                    {
                        spr.flipX = true;
                        face = Face.Left;
                    }
                    else
                    {
                        spr.flipX = false;
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
                    status = Status.patrol;
                    if (movingRight == true)
                    {
                        spr.flipX = false;
                        //movingRight = true;
                    }
                    else
                    {
                        spr.flipX = true;
                       // movingRight = false;
                    }
                }
                if(Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) <= 1.2)
                {
                    status = Status.idle;
                }

                break;

            case Status.patrol:
                transform.position = Vector2.MoveTowards(transform.position, moveSpots[i].position, speed * Time.deltaTime);


                if (Vector2.Distance(transform.position, moveSpots[i].position) < 0.1f)
                {
                    if (waitTime <= 0)
                    {
                        if (movingRight == true)
                        {
                            spr.flipX = false;
                            movingRight = false;
                        }
                        else
                        {
                            spr.flipX = true;
                            movingRight = true;
                        }

                        if (i == 0)
                        {

                            i = 1;
                        }
                        else
                        {

                            i = 0;
                        }

                        waitTime = wait;
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                    }
                    if (Mathf.Abs(myTransform.position.x - PlayerTransform.position.x) < Distance)
                    {
                        status = Status.walk;
                    }
                }


                break;
        }

    }
}