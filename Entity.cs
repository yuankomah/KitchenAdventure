using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    // Use this for initialization
    private float currentHealth;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject leftSlot;
    [SerializeField] protected GameObject rightSlot;
    [SerializeField] protected Item item;
    [SerializeField] private ProgressHealthBar healthBar;

    public bool Attacked(Entity entity)
    {
        currentHealth -= entity.item.damage;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            healthBar.UpdateHealth(currentHealth / GetInitialHealth());
            animator.Play("Death_A", 0, 0f);
            if (this is Player)
            {
                SceneManager.LoadScene("GameOverScene");
            }
            StartCoroutine(DestroyAfterAnimation(0.4f));
            return true;
        }
        else
        {
            healthBar.UpdateHealth(currentHealth / GetInitialHealth());
            animator.SetTrigger("hit");
        }

        return false;
    }

    protected void AssignWeapon()
    {
        clearWeapon();
        if (item.itemLeft != null)
            Instantiate(item.itemLeft, leftSlot.transform);

        if (item.itemRight != null)
            Instantiate(item.itemRight, rightSlot.transform);
    }

    protected void clearWeapon()
    {
        foreach (Transform child in leftSlot.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in rightSlot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    abstract public float GetInitialHealth();
    abstract public void HandleAttack();
    abstract public float GetAttackPerSecond();
    protected void SetInitialHealth(float health)
    {
        currentHealth = health;
    }

    IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (this is Enemy enemy)
        {

            Vector3 spawnPosition = transform.position + new Vector3(0f, 0.2f, 0f);
            Instantiate(enemy.GetCoin(), spawnPosition, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    protected void ResetHealthBar()
    {
        healthBar.UpdateHealth(1f);
    }
}
