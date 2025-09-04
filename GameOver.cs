using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() => { Application.Quit(); });
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
