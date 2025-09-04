using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static EnemySpawner Instance { get; private set; }
    [SerializeField] private List<Enemy> enemies;
    private bool eventTriggered = false;
    private List<Enemy> activeEnemies;

    private void Awake()
    {
        if (Instance != null)
        {
            // Throw error
        }

        Instance = this;
    }

    void Start()
    {
        activeEnemies = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Enemy newEnemy = Instantiate(enemy, enemy.transform.position, enemy.transform.rotation);
            newEnemy.gameObject.SetActive(true);
            activeEnemies.Add(newEnemy);
        }
    }

    public List<Enemy> GetActiveEnemies()
    {
        return activeEnemies;
    }

    public void Attacked()
    {
        print("Attack");
        List<Enemy> toRemoved = new List<Enemy>();
        foreach(Enemy enemy in activeEnemies)
        {
            if (Player.Instance.InRange(enemy))
            {
                print("Enemy got hit!");
                if (enemy.Attacked(Player.Instance))
                    toRemoved.Add(enemy);
            }
        }

        foreach (Enemy enemy in toRemoved)
            activeEnemies.Remove(enemy);
    }
}
