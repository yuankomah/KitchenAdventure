using UnityEngine;

public class SkeletonMinionShield : Enemy
{
    private float initialHealth = 20f;
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
