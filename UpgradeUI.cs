using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image gemImage;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private Image interactImage; 
    [SerializeField] private Image[] bars;
    public static UpgradeUI Instance { set; get; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        interactImage.gameObject.SetActive(false);
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        button.onClick.AddListener(closeImage);
    }

    private void UpdateUI()
    {

        for (int i = 0; i < bars.Length; i++)
            bars[i].color = i < UpgradeSystem.Instance.index ? Color.green : Color.gray;

        if (UpgradeSystem.Instance.IsMax())
        {
            costText.text = "MAX";
            costText.rectTransform.offsetMax = Vector2.zero;
            gemImage.gameObject.SetActive(false);
            upgradeButton.interactable = false;
        }
        else
        {
            costText.text = UpgradeSystem.Instance.costs[UpgradeSystem.Instance.index].ToString();
            upgradeButton.interactable = true;
        }
    }

    public void ShowImage()
    {
        image.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void closeImage()
    {
        image.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveAllListeners();
        button.onClick.RemoveAllListeners();
        Instance = null;
    }

    public void Show()
    {
        interactImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        interactImage.gameObject.SetActive(false);
    }

    private void OnUpgradeClicked()
    {
        var player = Player.Instance;
        var manager = UpgradeSystem.Instance;

        if (manager.CanUpgrade(player.GetCoin()))
        {
            player.UseCoin(manager.GetCurrentCost());
            manager.Upgrade();
            player.UpgradeWeapon();
            UpdateUI();
        }
    }
}
