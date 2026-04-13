using System.Collections;
using UnityEngine;

public class Death : MonoBehaviour
{
    public Sprite[] deathEffect;
    public float frameDuration = 0.08f;

    public void Play(GameObject target)
    {
        // Spawn a standalone GO at the enemy's position
        GameObject fx = new GameObject("DeathFX");
        fx.transform.position = target.transform.position;

        SpriteRenderer sr = fx.AddComponent<SpriteRenderer>();

        // Match the enemy's sorting layer/order so it draws correctly
        SpriteRenderer enemySR = target.GetComponent<SpriteRenderer>();
        if (enemySR != null)
        {
            sr.sortingLayerID = enemySR.sortingLayerID;
            sr.sortingOrder   = enemySR.sortingOrder;
        }

        // Copy the sprites + duration to the new GO and let it run
        fx.AddComponent<DeathFX>().Init(deathEffect, frameDuration);

        target.SetActive(false);
    }
}

public class DeathFX : MonoBehaviour
{
    public void Init(Sprite[] frames, float frameDuration)
    {
        StartCoroutine(Animate(frames, frameDuration));
    }

    private IEnumerator Animate(Sprite[] frames, float frameDuration)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        foreach (Sprite frame in frames)
        {
            sr.sprite = frame;
            yield return new WaitForSeconds(frameDuration);
        }

        Destroy(gameObject); // clean up after itself
    }
}