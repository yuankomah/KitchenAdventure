using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private List<Furniture> furnitures;
    public static RestaurantManager Instance { get; set; }

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

    public void Repair(Furniture furniture)
    {
        furnitures.Remove(furniture);
        if (furnitures.Count == 0)
        {
            GameManager.Instance.PlayerWin();
        }
    }
}
