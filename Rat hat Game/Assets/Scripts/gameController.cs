using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class gameController : MonoBehaviour
{

    public static gameController instance;
    public playerMovement player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<playerMovement>();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
