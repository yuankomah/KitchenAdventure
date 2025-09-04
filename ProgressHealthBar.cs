using UnityEngine;
using UnityEngine.UI;

public class ProgressHealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Image image;
    void Start()
    {
        image.fillAmount = 1f;
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateHealth(float percentage)
    {
        if (percentage == 0f)
        {
            Hide();
        } else
        {
            image.fillAmount= percentage;
            Show();
        }
    }
}
