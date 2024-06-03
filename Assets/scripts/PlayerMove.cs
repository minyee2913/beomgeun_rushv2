using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance {get; private set;}
    GameManager gameManager;

    public float maxSpeed;

    private Rigidbody2D rigid;
    private SpriteRenderer render;
    IEnumerator action;
    bool jumping, slide;
    public Sprite superOn;
    public Sprite superOff;
    public float super;
    public bool supered;
    public Slider superBar;
    public Button superButton;
    public Image cutscene;

    public float useSuper;

    private void Awake()
    {
        Instance = this;
        rigid= GetComponent<Rigidbody2D>();
        render= GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Slide();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Super();
        }

        superBar.value = super / 100f;

        if (super >= 100) {
            superButton.image.sprite = superOn;

            if (!supered) {
                StartCoroutine(GetSuper());
            }

            supered = true;
        } else {
            superButton.image.sprite = superOff;

            supered = false;
        }

        if (useSuper > 0) {
            useSuper -= Time.deltaTime;
        }
    }

    IEnumerator GetSuper() {
        superButton.transform.DOScale(new Vector3(1.1f, 1.1f), 0.2f);
        yield return new WaitForSeconds(0.2f);

        superButton.transform.DOScale(new Vector3(1f, 1f), 0.2f);
    }

    public void Jump() {
        if (jumping) return;

        if (action != null) {
            StopCoroutine(action);
        }
        action = jump_();
        StartCoroutine(action);
    }

    IEnumerator jump_() {
        jumping = true;
        slide = false;

        transform.DOScale(new Vector3(4, 4), 0.2f);
        transform.DOMove(new Vector3(transform.position.x, 2), 0.4f);
        transform.DORotate(new Vector3(0, 0, -180), 0.4f);
        yield return new WaitForSeconds(0.4f);

        jumping = false;

        transform.DOMove(new Vector3(transform.position.x, 1), 0.3f);

        yield return new WaitForSeconds(0.3f);

        transform.DOMove(new Vector3(transform.position.x, -2), 0.5f);

        transform.DORotate(new Vector3(0, 0, -360), 0.5f);
        yield return new WaitForSeconds(0.5f);

        jumping = false;
    }

    public void Slide() {
        if (slide) return;

        if (action != null) {
            StopCoroutine(action);
        }
        action = slide_();
        StartCoroutine(action);
    }

    public void Super() {
        if (action != null) {
            StopCoroutine(action);
        }
        action = super_();
        StartCoroutine(action);
    }

    IEnumerator super_() {
        useSuper = 6f;
        cutscene.transform.localPosition = new Vector3(970, 600);
        cutscene.transform.DOLocalMove(new Vector3(-56.41f, -22.91f), 0.2f);
        yield return new WaitForSeconds(0.5f);

        cutscene.transform.DOLocalMove(new Vector3(-933f, -581f), 0.2f);

        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1;
    }

    IEnumerator slide_() {
        slide = true;
        jumping = false;

        transform.DOScale(new Vector3(6, 2), 0.4f);
        transform.DORotate(new Vector3(0, 0, 50), 0.4f);
        transform.DOMoveY(-3, 0.3f);
        yield return new WaitForSeconds(0.4f);

        slide = false;

        transform.DOMoveY(-2, 0.4f);
        yield return new WaitForSeconds(0.1f);

        transform.DOScale(new Vector3(4, 4), 0.3f);
        transform.DORotate(new Vector3(0, 0, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);

        slide = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "object")
        {
            if (useSuper > 0) {
                Destroy(other.gameObject);

                return;
            }
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        gameManager.alive = false;
        render.color = Color.red;

        yield return new WaitForSeconds(1f);
        StartCoroutine(gameManager.LoadAnim());

        render.color = Color.white;
    }

}
