using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    [SerializeField] private float force = 700f;
    [SerializeField] private float turnSpeed = 100;

    [Space]
    [SerializeField] private float landingVelocityMagnitude = 4;
    [SerializeField] private float minDotVector = 4;

    private Rigidbody2D landerRigidbody2D;

    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            landerRigidbody2D.AddTorque(+turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            landerRigidbody2D.AddTorque(-turnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.relativeVelocity.magnitude > landingVelocityMagnitude)
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
    }
}
