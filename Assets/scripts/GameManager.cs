using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject fast_load;
    public GameObject player;
    AudioSource beatBox;
    AudioSource bgm;
    public float score;
    public float spawnCool;
    public List<Objective> obj;
    public Text scoreText;

    public bool alive = false;
    void Start()
    {
        beatBox = GameObject.Find("beatBox").GetComponent<AudioSource>();
        bgm = GameObject.Find("bgm").GetComponent<AudioSource>();

        StartCoroutine(LoadAnim());
    }

    public IEnumerator LoadAnim()
    {
        BackgroundScrolling.Instance.speed = 0;
        alive = false;
        fast_load.SetActive(true);
        beatBox.Play();
        bgm.Stop();
        yield return new WaitForSeconds(0.3f);

        for (float i = 0; i < 2; i+=0.2f)
        {
            fast_load.transform.position = new Vector2(fast_load.transform.position.x - i, fast_load.transform.position.y);
            yield return new WaitForSeconds(0.03f);
        }

        for (float i = 0; i < 2; i += 0.2f)
        {
            fast_load.transform.position = new Vector2(fast_load.transform.position.x, fast_load.transform.position.y + i);
            yield return new WaitForSeconds(0.03f);
        }

        fast_load.transform.position = new Vector2(-2, 0);
        for (float i = 0.8f; i > 0.3; i -= 0.07f)
        {
            fast_load.transform.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.03f);
        }

        for (int i = 0; i < 4; i++)
        {
            foreach (Transform child in fast_load.transform)
            {
                var renderer  = child.GetComponent<SpriteRenderer>();

                renderer.color = new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), 1);
            }
            yield return new WaitForSeconds(0.1f);
        }

        fast_load.transform.position = new Vector2(-5.98f, 0.22f);
        player.transform.position = new Vector2(-6f, -2f);

        for (float i = 0.3f; i < 1; i+=0.1f)
        {
            fast_load.transform.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0.03f);
        }


        foreach (Transform child in fast_load.transform)
        {
            var renderer = child.GetComponent<SpriteRenderer>();

            renderer.color = Color.blue;
        }
        yield return new WaitForSeconds(0.1f);
        PlayerMove.Instance.super = 0;
        PlayerMove.Instance.useSuper = 0;
        fast_load.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        bgm.Play();

        spawnCool = 0;

        alive = true;
        BackgroundScrolling.Instance.speed = 600;

        score = 0;
    }

    void Update() {
        var yScreenHalfSize = Camera.main.orthographicSize;
        var xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;
        if (alive) {
            score += Time.deltaTime * 9;

            BackgroundScrolling.Instance.speed = 600 + score * 0.05f;

            if (PlayerMove.Instance.useSuper > 0) {
                BackgroundScrolling.Instance.speed = 2000 + score * 0.05f;
            }

            if (spawnCool > 3 - score * 0.008f + UnityEngine.Random.Range(0f, 2f)) {
                spawnCool = 0;

                int rv = UnityEngine.Random.Range(0, obj.Count);
                if (rv >= obj.Count) rv = obj.Count - 1;
                Objective ob = obj[rv];

                Instantiate(ob.gameObject, new Vector3(xScreenHalfSize, ob.yVal), quaternion.identity);
            }

            spawnCool += Time.deltaTime;
        }

        scoreText.text = "<color=lime>Score: </color> " + ((int)score).ToString();
    }
}
