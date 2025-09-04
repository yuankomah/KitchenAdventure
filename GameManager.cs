using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance { get; set; }
    private EnemySpawner enemySpawner;
    private Vector3 NextSpawnPosition;
    private Quaternion NextSpawnRotation;

    private bool isGamePaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitEnemy()
    {
        if (enemySpawner != null)
            enemySpawner.Attacked();
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void SetNextSpawnPosition(Vector3 position)
    {
        NextSpawnPosition = position;
    }

    public void SetNextSpawnRotation(Quaternion rotation)
    {
        NextSpawnRotation = rotation;
    }

    public Vector3 GetNextSpawnPosition() => NextSpawnPosition;

    public Quaternion GetNextSpawnRotation() => NextSpawnRotation;

    public void UpdateEnemySpawner()
    {
        if (EnemySpawner.Instance != null)
            enemySpawner = EnemySpawner.Instance;
    }

    public void PlayerWin()
    {
        // TODO:
        SceneManager.LoadScene("GameWinScene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
