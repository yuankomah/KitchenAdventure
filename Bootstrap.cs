// Example: in Bootstrap scene
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    public static Bootstrap Instance { get; set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Start()
    {
        startButton.onClick.AddListener(() => { SceneManager.LoadScene("StoryGame"); });
        exitButton.onClick.AddListener(() => Application.Quit());
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }
}
