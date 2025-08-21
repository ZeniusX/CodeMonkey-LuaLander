using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinPickupAmount = 500;

    public int GetCoinPickUpAmount() => coinPickupAmount;

    public void DestroySelf() => Destroy(gameObject);
}
