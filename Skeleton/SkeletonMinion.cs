using UnityEngine;

public class SkeletonMinion : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float initialHealth = 10f;

    private void Start()
    {
        AssignWeapon();
        SetInitialHealth(initialHealth);
    }

    override public float GetInitialHealth()
    {
        return initialHealth;
    }
}
