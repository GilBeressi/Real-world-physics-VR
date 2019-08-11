﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour {

    public GameObject fencePrefab;

    public GameObject wallEPrefab;

    public GameObject elephantPrefab;

    public GameObject explosionPrefab;


    private GameObject[] movingItems = new GameObject[NUMBER_OF_ITEMS];


    private const int NUMBER_OF_ITEMS = 3;

    private const int BOMB_BALL_DAMAGE = 200;

    private const int WOODEN_BALL_DAMAGE = 100;

    private const int SPIKE_BALL_DAMAGE = 150;


    private static bool object_exists = false;

    private static MovingObject moving = new MovingObject();

    private static int current_score = 0;

    private static GameObject score_board;

    private static bool score_board_flag = false;



    public class MovingObject
    {
        int hp;
        int score;
        float speed;
        Vector3 startingPosition;
        Vector3 scale;
        Quaternion rotation;
        GameObject currentObject;

        public int Hp
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        public Vector3 StartingPosition
        {
            get
            {
                return startingPosition;
            }

            set
            {
                startingPosition = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }

        public GameObject CurrentObject
        {
            get
            {
                return currentObject;
            }

            set
            {
                currentObject = value;
            }
        }

        public bool offBounds()
        {
            return (CurrentObject.transform.position.z < 80f) ? true : false;
        }
    }

    class MovingFence : MovingObject
    {
        public MovingFence(GameObject fence)
        {
            Hp = 100;
            Score = 5;
            Speed = 10f;
            StartingPosition = new Vector3(254.665f, 1.02f, 170.023f);
            Scale = new Vector3(0.8f, 0.8f, 0.8f);
            Rotation = Quaternion.identity;
            CurrentObject = Instantiate(fence);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

    }

    class MovingWallE : MovingObject
    {
        public MovingWallE(GameObject wallE) {
            Hp = 180;
            Score = 10;
            Speed = 5f;
            StartingPosition = new Vector3(256.665f, 1.02f, 170.023f);
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Rotation = Quaternion.Euler(0f,90f,0f);
            CurrentObject = Instantiate(wallE);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

    }

    class MovingElephant : MovingObject
    {
        public MovingElephant(GameObject elephant)
        {
            Hp = 150;
            Score = 6;
            Speed = 8f;
            StartingPosition = new Vector3(260f, 0.6f, 170.023f);
            Scale = new Vector3(100f, 100f, 100f);
            Rotation = Quaternion.Euler(0f, 180f, 0f);
            CurrentObject = Instantiate(elephant);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

    }


    // Use this for initialization
    void Start () {
        movingItems[0] = fencePrefab;
        movingItems[1] = wallEPrefab;
        movingItems[2] = elephantPrefab;

        if (!score_board_flag)
        {
            score_board = new GameObject();
            score_board.AddComponent<TextMesh>();
            score_board.transform.position = new Vector3(247.7f, 21.9f, 175.723f);
            score_board.GetComponent<TextMesh>().text = "0";
            score_board_flag = true;
        }
    }
	
	// Update is called once per frameW
	void Update () {
        if (!object_exists)
        {
            int rand = Random.Range(0,NUMBER_OF_ITEMS);
            switch (rand)
            {
                case 0:
                    moving = new MovingFence(movingItems[0]);
                    break;
                case 1:
                     moving = new MovingWallE(movingItems[1]);
                    break;
                case 2:
                     moving = new MovingElephant(movingItems[2]);
                    break;
                default:
                    moving = new MovingFence(movingItems[0]);
                    break;
            }
            object_exists = true;

        }

        moving.CurrentObject.transform.Translate(Vector3.back * Time.deltaTime * moving.Speed, Space.World);

        if (object_exists && moving.offBounds())
        {
            moving.CurrentObject.transform.parent = null;
            Destroy(moving.CurrentObject);
            object_exists = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            Destroy(gameObject);
        }
        else if (gameObject.name == "BombBall(Clone)")
        {
            GameObject exp = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
            moving.Hp = moving.Hp - BOMB_BALL_DAMAGE;
            Destroy(exp, 3);
        }
        else if (gameObject.name == "WoodenBall(Clone)")
        {
            moving.Hp = moving.Hp - WOODEN_BALL_DAMAGE;
        }
        else if (gameObject.name == "SpikeBall(Clone)")
        {
            moving.Hp = moving.Hp - SPIKE_BALL_DAMAGE;
        }

        Destroy(gameObject);
        if (moving.Hp <= 0)
        {
            Destroy(collision.gameObject);
            current_score = current_score + moving.Score;
            score_board.GetComponent<TextMesh>().text = (current_score).ToString();
            score_board.GetComponent<TextMesh>().fontSize = 35;
         //   Debug.Log(score_board.text);
            object_exists = false;
        }

    }

}
