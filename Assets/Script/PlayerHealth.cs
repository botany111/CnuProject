﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public GameObject chin;
    public GameObject Main;

    public Transform spawn;

    public static int Scene;





    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        HealthBar.HealthCurrent = health;
        HealthBar.HealthMax = health;

        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int Damage)
    {
        health -= Damage;
        HealthBar.HealthCurrent = health;

        if(health <= 0)
        {
            Destroy(gameObject);
            Destroy(chin);
            Destroy(Main);
            SceneManager.LoadScene(1);
            
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portal"))
        {
            //Debug.Log("123");
            collision.gameObject.transform.GetComponent<Portal>().ChangeScene();
            Vector3 move = gameObject.transform.position;
            move = new Vector2(move.x = -10, move.y = -4);
            gameObject.transform.position = move;




            

            //Scene = +1;

        }

    }
}
