using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public static Lander Instance { get; private set; }

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler<OnCoinPickupEventArgs> OnCoinPickup;
    public event EventHandler<OnLandedEventArgs> OnLanded;

    public class OnCoinPickupEventArgs : EventArgs
    {
        public int addAmount;
    }

    public class OnLandedEventArgs : EventArgs
    {
        public int score;
    }

    [SerializeField] private float force = 700f;
    [SerializeField] private float turnSpeed = 100;

    [Space]
    [SerializeField] private float landingVelocityMagnitude = 4;
    [SerializeField] private float minDotVector = 0.90f;

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount = 10f;

    private void Awake()
    {
        Instance = this;

        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        if (fuelAmount <= 0f) return;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            ConsumeFuel();
        }

        if (Keyboard.current.wKey.isPressed)
        {
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            landerRigidbody2D.AddTorque(+turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            landerRigidbody2D.AddTorque(-turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ConsumeFuel()
    {
        float fuelConsumption = 1f;
        fuelAmount = Mathf.Max(fuelAmount - fuelConsumption * Time.fixedDeltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the terrain");
            return;
        }

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > landingVelocityMagnitude)
        {
            Debug.Log("Landed too hard!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a steep angle");
            return;
        }

        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore =
            maxScoreAmountLandingAngle -
            Mathf.Abs(dotVector - 1f) *
            scoreDotVectorMultiplier *
            maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (landingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log($"landingAngleScore {landingAngleScore: 0}");
        Debug.Log($"landingSpeedScore {landingSpeedScore: 0}");

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());

        Debug.Log($"Total Score: {score}");

        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            score = score
        });
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            fuelAmount += fuelPickup.GetFuelPickupAmount();
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, new OnCoinPickupEventArgs
            {
                addAmount = coinPickup.GetCoinPickUpAmount()
            });
            coinPickup.DestroySelf();
        }
    }
}
