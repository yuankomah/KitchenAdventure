using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Furniture : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject Unfixed;
    [SerializeField] private Canvas canvas;
    [SerializeField] private int cost;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    private bool isFixed = false;

    private void Start()
    {
        text.text = cost.ToString();
        image.gameObject.SetActive(false);
    }

    public void PlayerInteract(bool status)
    {
        if (isFixed) return;
        image.gameObject.SetActive(status);
    }

    public void OnInteract()
    {
        if (isFixed) return;
        if (Player.Instance.GetCoin() < cost)
        {
            if (!Dialogue.Instance.IsActive())
            {
                string[] messages = { "I don't have enough gems to repair..." };
                Dialogue.Instance.StartDialogue(messages);
            }
        } else
        {
            Player.Instance.UseCoin(cost);
            isFixed = true;
            RestaurantManager.Instance.Repair(this);
            Unfixed.gameObject.SetActive(false);
            canvas.gameObject.SetActive(false);
            string[] messages = { "I've successfully repaired this furniture!" };
            Dialogue.Instance.StartDialogue(messages);
            BoxCollider[] colliders = GetComponents<BoxCollider>();
            foreach (BoxCollider bc in colliders)
            {
                Destroy(bc);
            }
        }
    }
}
