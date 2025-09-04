using UnityEngine;
using System;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance { get; private set; }

    public int index = 0;          // current upgrade stage
    public int[] costs = { 10, 20, 50, 100, 150 };
    public event EventHandler onInteract;
    private bool interact = false;

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


    public bool CanUpgrade(int playerCoins)
    {
        return index < costs.Length && playerCoins >= costs[index];
    }

    public int GetCurrentCost()
    {
        return costs[index];
    }

    public void Upgrade()
    {
        if (index < costs.Length)
            index++;
    }

    public bool IsMax()
    {
        return index >= costs.Length;
    }

    public bool isInteract()
    {
        return interact;
    }

    public void SetInteract(bool status)
    {
        if (status == false)
            UpgradeUI.Instance?.Hide();
        else
            UpgradeUI.Instance?.Show();

        interact = status;
    }
}
