using UnityEngine;

public class playerSprites : MonoBehaviour
{
    playerMovement pMove;
    SpriteRenderer sr;
    public Sprite idle;
    public Sprite aerial;
    public Sprite parryR;
    public Sprite parryL;
    private Sprite currentSprite;
    private Rigidbody2D rb;
    private Spoon spoon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pMove = GetComponent<playerMovement>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spoon = transform.GetChild(0).GetComponent<Spoon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance.isTransitioning){
            sr.sprite = idle;
            return;
        }

        if(sr.sprite != currentSprite)
        {
            sr.sprite = currentSprite;
        }
        if(rb.linearVelocityX == 0 && rb.linearVelocityY == 0)
        {
            currentSprite = idle;
        }
        else if (spoon.checkIfSwinging())
        {
            if (rb.linearVelocityX < 0)
            {
                currentSprite = parryL;
            }
            else
            {
                currentSprite = parryR;
            }
        }
        else if (rb.linearVelocityY != 0)
        {
            currentSprite = aerial;
        }
    }
}
