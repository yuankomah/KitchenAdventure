using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Transform transformPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetNextSpawnPosition(transformPosition.position);
            GameManager.Instance.SetNextSpawnRotation(transformPosition.rotation);

            // Subscribe to sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Load the new scene
            SceneManager.LoadScene(sceneToLoad);
            if (sceneToLoad == "SampleScene")
                Player.Instance?.ResetHealth();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Move the player after scene finishes loading
        if (Player.Instance != null)
        {
            Player.Instance.transform.position = GameManager.Instance.GetNextSpawnPosition();
            Player.Instance.transform.rotation = GameManager.Instance.GetNextSpawnRotation();
        }

        GameManager.Instance.UpdateEnemySpawner();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
