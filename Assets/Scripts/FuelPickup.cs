using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField] private float fuelPickupAmount = 10f;

    public float GetFuelPickupAmount() => fuelPickupAmount;

    public void DestroySelf() => Destroy(gameObject);
}
