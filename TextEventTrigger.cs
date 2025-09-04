using UnityEngine;

public class TextEventTrigger : MonoBehaviour
{
    [SerializeField] private string status;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Dialogue.Instance?.ShowMessage(status);
            Destroy(gameObject);
        }
    }
}
