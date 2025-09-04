using UnityEngine;

public class SkeletonWarrior : Enemy
{
    private float initialHealth = 30f;
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
