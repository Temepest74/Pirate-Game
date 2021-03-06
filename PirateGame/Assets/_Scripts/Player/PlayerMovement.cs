using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float speed;
    public float rotationSpeed;
    public float brake;

    float horizontal;
    float vertical;

    Rigidbody2D rb2D;

    void Start ()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D> ();
    }

    void Update ()
    {
        speed = GetComponent<IEntityData>().GetEntityData().speed;
        horizontal = Input.GetAxisRaw ("Horizontal");
        vertical = Input.GetAxisRaw ("Vertical");
    }

    void FixedUpdate ()
    {
        transform.Translate (Vector3.up * speed * Time.deltaTime * vertical, Space.Self); //change it when ballancing the game
        transform.Rotate (0.0f, 0.0f, -horizontal * rotationSpeed);
        if (vertical == 0.0f)
        {
            float brakeOnX = Mathf.MoveTowards (rb2D.velocity.x, 0.0f, brake);
            float brakeOnY = Mathf.MoveTowards (rb2D.velocity.y, 0.0f, brake);
            rb2D.velocity = new Vector2 (brakeOnX, brakeOnY);
        }
    }
}