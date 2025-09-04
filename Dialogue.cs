using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Dialogue Instance { get; set; }
    [SerializeField] private Image image;
    [SerializeField] private Image image2;
    [SerializeField] private float textSpeed;
    private bool mine;
    private bool restaurant;
    private bool blacksmith;
    private string[] messages;
    private int index;
    private string[] mineMessage =
    {
        "wow... is this the mine everyone's talking about?... I should explore it now!",
    };

    private string[] restaurantMessage =
    {
        "So the restaurant is straight to the park entrance... I should remember that...",
    };

    private string[] blacksmithMessage =
    {
        "I can upgrade my weapon in the blacksmith... But it costs me some gems..."
    };
    private string[] startMessages =
    {
        "You wake up to the smell of smoke. Rushing outside, you find your papa kneeling in front of the charred remains of his beloved kitchen. The fire took everything: his tools, his recipes, his pride.",
        "Papa: \"It’s all gone, Son... the stove, the spice rack, even Grandma’s soup pot. I don’t know how we’ll start again.\"",
        "You: \"We will, Papa. I’ll find a way. I heard the old mines still have treasure... and trouble. Skeletons guard the depths, but they say they drop coin and gear.\"",
        "Papa:\"Skeletons? You’re serious? That place is cursed!\"",
        "You: \"So is losing everything. I’ll go. One piece at a time, I’ll rebuild your kitchen.\""
    };
    [SerializeField] private TextMeshProUGUI totalCoin;
    [SerializeField] private TextMeshProUGUI text;

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

    public void ShowMessage(string str)
    {
        if (str == "mine" && !mine)
        {
            StartDialogue(mineMessage);
            mine = true;
        } else if (str == "restaurant" && !restaurant)
        {
            StartDialogue(restaurantMessage);
            restaurant = true;
        } else if (str == "blacksmith" && !blacksmith)
        {
            StartDialogue(blacksmithMessage);
            blacksmith = true;
        }
    }

    private void Start()
    {
        Hide();
        image2.gameObject.SetActive(false);
        UpdateTotalCoin(0);
        index = 0;
        messages = startMessages;
        StartCoroutine(StartSequence());
;    }

    public bool IsActive()
    {
        return image.gameObject.activeSelf;
    }

    public void StartDialogue(string[] messages)
    {
        index = 0;
        text.text = string.Empty;
        this.messages = messages;
        StartCoroutine(TypeDialogue());
    }

    IEnumerator StartSequence()
    {
        yield return StartCoroutine(TypeDialogue());
        SceneManager.LoadScene("RestaurantScene");
        string[] restaurantMessage = { "Uh... I need to work really hard to repair all these furnitures...", "I should start exploring the mine ASAP!" };
        image2.gameObject.SetActive(true);
        StartDialogue(restaurantMessage);
    }

    IEnumerator TypeDialogue()
    {
        Show();
        while (index < messages.Length)
        {
            text.text = string.Empty;
            foreach (char c in messages[index].ToCharArray())
            {
                text.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            // Wait 3 seconds after finishing the current line
            yield return new WaitForSeconds(1f);
            // Move to next message
            index++;
        }
        Hide();
    }


    private void Hide()
    {
        image.gameObject.SetActive(false);
    }

    private void Show()
    {
        image.gameObject.SetActive(true);
    }

    public void UpdateTotalCoin(int coin)
    {
        totalCoin.text = coin.ToString();
    }
}
