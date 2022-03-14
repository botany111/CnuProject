﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpspeed;

    private Rigidbody2D myRigidbody;

    private BoxCollider2D myFeet;
    private bool isGround;
    // Start is called before the first frame update
    void Start()
    {
        // 遊戲開始後取得組件
        myRigidbody = GetComponent<Rigidbody2D>();

        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 在遊戲進行當中不斷取得Run的數值
        Run();
        Jump();
        checkGround();
    }

    void checkGround()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        //Debug.Log(isGround);
    }

    void Run()
    {
        // Input.GetAxis是Unity內建偵測輸入的函式 | Horizontal = 水平 | 偵測玩家按的按鍵取的方向
        float moveDir = Input.GetAxis("Horizontal");

        // Vector2 = 2D向量和點的表示形式，用於表示2D位置，只有兩個軸x＆y | 將取得的方向乘上玩家的速度進行移動
        Vector2 playerVal = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);

        // 回傳結果
        myRigidbody.velocity = playerVal;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround == true)
            {
                Vector2 jumpVal = new Vector2(0, jumpspeed);
                myRigidbody.velocity = Vector2.up * jumpVal;
            }
            
        }
    }
}
