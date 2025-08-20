using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;

    public static Lander Instance { get; private set; }

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler<OnCoinPickupEventArgs> OnCoinPickup;
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnCoinPickupEventArgs : EventArgs
    {
        public int addAmount;
    }

    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
        public int score;
    }

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver
    }

    [SerializeField] private float force = 700f;
    [SerializeField] private float turnSpeed = 100;
    [SerializeField] private float fuelAmountMax = 25f;

    [Space]
    [SerializeField] private float landingVelocityMagnitude = 4;
    [SerializeField] private float minDotVector = 0.90f;

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private State state;

    private void Awake()
    {
        Instance = this;

        fuelAmount = fuelAmountMax;
        state = State.WaitingToStart;

        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
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
                break;
            case State.GameOver:
                break;
        }

        if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            ConsumeFuel();
        }
    }

    private void ConsumeFuel()
    {
        float fuelConsumption = 1f;
        fuelAmount = Mathf.Max(fuelAmount - fuelConsumption * Time.fixedDeltaTime, 0);
    }

    private void SetState(State state)
    {
        this.state = state;

        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the terrain");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0,
                landingSpeed = 0,
                scoreMultiplier = 0,
                score = 0
            });
            SetState(State.GameOver);

            return;
        }

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        float dotVector = Vector2.Dot(Vector2.up, transform.up);

        if (relativeVelocityMagnitude > landingVelocityMagnitude)
        {
            Debug.Log("Landed too hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0
            });
            SetState(State.GameOver);

            return;
        }

        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a steep angle");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0
            });
            SetState(State.GameOver);

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
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            fuelAmount = Mathf.Min(fuelAmount + fuelPickup.GetFuelPickupAmount(), fuelAmountMax);
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

    public float GetFuel() => fuelAmount;

    public float GetFuelAmountNormalized() => fuelAmount / fuelAmountMax;

    public float GetSpeedX() => landerRigidbody2D.linearVelocityX;

    public float GetSpeedY() => landerRigidbody2D.linearVelocityY;
}
