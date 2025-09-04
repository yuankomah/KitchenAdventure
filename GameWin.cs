using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameWin : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() => { Application.Quit(); });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}