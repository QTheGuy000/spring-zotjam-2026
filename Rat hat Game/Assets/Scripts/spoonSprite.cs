using UnityEngine;
using System.Collections;

public class spoonSprite : MonoBehaviour
{
    private Transform player;
    private Transform spoon;
    private Transform spriteHolder;
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
        spriteHolder = transform.GetChild(1);
        mainCamera = Camera.main;
        radius = Vector2.Distance(transform.localPosition, Vector2.zero);
        radius = 0.5795338f;
        parrying = false;
        float spoonRadius = spoon.GetComponent<Spoon>().radius;
        spriteRenderer = spriteHolder.GetComponent<SpriteRenderer>();
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
        parrying = true;
        StartCoroutine(SpinSpoon());
        spriteRenderer.sprite = leftSwing;
        Debug.Log("Swing");
    }

    public void RightSwing()
    {
        parrying = true;
        StartCoroutine(SpinSpoon());
        spriteRenderer.sprite = rightSwing;
        Debug.Log("Swing");
    }

    IEnumerator SpinSpoon()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = spoon.position;
        float turnSpeed = 1000f;
        float rotated = 0;
        Quaternion origRotation = transform.rotation;
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
    }
}
