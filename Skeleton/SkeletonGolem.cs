using UnityEngine;

public class SkeletonGolem : Enemy
{
    private float initialHealth = 100f;
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
