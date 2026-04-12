using UnityEngine;
using System.Collections;

public class spoonSprite : MonoBehaviour
{
    private Transform player;
    private Transform spoon;
    public Transform spriteHolder;
    private Camera mainCamera;
    private float radius;
    private bool parrying;
    private Vector3 SpinPos;
    private Vector3 SpinDirection;
    private float spinAngle;
    public float spinSpeed;
    private float spoonRadius;
    private SpriteRenderer spriteRenderer;
    public Sprite leftSwing;
    public Sprite rightSwing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform.parent.parent;
        spoon = transform.parent;
        //spriteHolder = transform.GetChild(0);
        mainCamera = Camera.main;
        radius = Vector2.Distance(transform.localPosition, Vector2.zero);
        radius = 0.5795338f;
        parrying = false;
        float spoonRadius = spoon.GetComponent<Spoon>().radius;
        spriteRenderer = spriteHolder.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (parrying)
        {
            swing();
        }
        else
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 direction = (mouseWorldPos - player.position).normalized;
            transform.position = player.position + direction * radius;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }*/
        spriteHolder.transform.position = new Vector2(player.transform.position.x, spoonRadius + player.transform.position.y);
    }

    public void LeftSwing()
    {
        //Debug.Log("SwingStart");
        parrying = true;
        spriteRenderer.sprite = leftSwing;
        spriteRenderer.enabled = true;
        StartCoroutine(SpinSpoon());
        //Debug.Log("SwingEnd");
    }

    public void RightSwing()
    {
        //Debug.Log("Holder " + spriteHolder);
        //Debug.Log("Renderer " + spriteRenderer);
        //Debug.Log("SwingStart");
        parrying = true;
        spriteRenderer.sprite = rightSwing;
        spriteRenderer.enabled = true;
        StartCoroutine(SpinSpoon());
        //Debug.Log("SwingEnd");
    }

    IEnumerator SpinSpoon()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = spoon.position;
        float turnSpeed = 1000f;
        float rotated = 0;
        transform.RotateAround(player.position, Vector3.forward, turnSpeed * Time.deltaTime);
        while (rotated < 360f)
        {
            rotated += turnSpeed * Time.deltaTime;
            transform.RotateAround(player.position, Vector3.forward, turnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = spoon.position;
        parrying = false;
        hideSprite();
    }

    void hideSprite()
    {
        spriteRenderer.enabled = false;
        spoon.GetComponent<Spoon>().returnSprite();
    }
}
