using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkeletonNPC : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float odds;
    [SerializeField] private int cost;
    [SerializeField] private Image textImage;
    [SerializeField] private Image image;
    private bool isOpened = false;
    [SerializeField] private GameObject gems;

    private bool Success()
    {
        return UnityEngine.Random.value < (odds / 100f);
    }

    private void Start()
    {
        Hide();
    }


    public void Show()
    {
        if (isOpened) return;
        image.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (isOpened) return;
        image.gameObject.SetActive(false);
    }

    public bool CanInteract()
    {
        return image.gameObject.activeSelf && !isOpened;
    }

    public void Gamble()
    {
        if (isOpened) return;
        if (Player.Instance.GetCoin() < cost)
        {
            if (!Dialogue.Instance.IsActive())
            {
                string[] messages = { "I don't have enough gems to buy it..." };
                Dialogue.Instance.StartDialogue(messages);
            }
        } else
        {
            isOpened = true;
            animator.SetTrigger("open");
            Player.Instance.UseCoin(cost);
            if (Success())
            {
                int reward = (int)((100 / odds) * cost);
                gems.gameObject.SetActive(true);
                string[] messages = { "You hit the jackpot! You got " + reward + " gems!" };
                Dialogue.Instance.StartDialogue(messages);
                Player.Instance.CollectCoin(reward);
            } else
            {
                int reward = (int)(odds * cost / 100);
                gems.gameObject.SetActive(false);
                string[] messages = { "You are unlucky! You got " + reward + " gems!" };
                Dialogue.Instance.StartDialogue(messages);
                Player.Instance.CollectCoin(reward);
            }

            textImage.gameObject.SetActive(false);
            image.gameObject.SetActive(false);
        }
    }
}
