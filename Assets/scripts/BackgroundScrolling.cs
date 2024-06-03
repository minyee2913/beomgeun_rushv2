using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BackgroundScrolling : MonoBehaviour
{
    public static BackgroundScrolling Instance {get; private set;}
    public float speed;
    public Transform[] backgrounds;
 
    float leftPosX = 0f;
    float rightPosX = 0f;
    float xScreenHalfSize;
    float yScreenHalfSize;
    void Awake()
    {
        Instance = this;
        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        leftPosX = -(xScreenHalfSize * 2);
        rightPosX = xScreenHalfSize * backgrounds.Length * 0.8f;
    }
    void Update()
    {
       for(int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(-speed * Time.deltaTime, 0, 0) * Time.deltaTime;
 
            if(backgrounds[i].position.x < leftPosX)
            {
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(nextPos.x + rightPosX, nextPos.y, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }
}
