using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fall : MonoBehaviour
{
    GameManager gameManager;
    bool supered;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var yScreenHalfSize = Camera.main.orthographicSize;
        var xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        var leftPosX = -(xScreenHalfSize * 2);

        transform.position += new Vector3(-BackgroundScrolling.Instance.speed * Time.deltaTime / 100, 0);

        if (!supered && Mathf.Abs(PlayerMove.Instance.transform.position.x - transform.position.x) < 1) {
            supered = true;
            PlayerMove.Instance.super += 10f;
        }

        if (!gameManager.alive || transform.position.x < leftPosX) {
            Destroy(gameObject);
        }
    }
}
