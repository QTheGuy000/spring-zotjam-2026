using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public bool levelComplete = false;

    [SerializeField] private enemy[] enemies;

    void Start(){

    }

    void Update()
    {
        if (!levelComplete)
        {
            levelComplete = AllEnemiesDead();
        }
    }

    bool AllEnemiesDead()
    {
        if (enemies.Length == 0){
            return false;
        }

        foreach (enemy enemy in enemies){
            // If enemy object is destroyed, skip it
            if (enemy == null){
                continue;
            } 

            if (!enemy.isDead){
                return false;
            } 
        }
        // All enemies dead.
        return true;
    }
}