using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 input;

    private SpriteRenderer sprite;


    private void OnEnable()
    {
        // TODO: Set position from save file.


    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }


    private void Update()
    {
        input = new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
        );

        if (input.sqrMagnitude > 1f)
            input = input.normalized;

        if (input.sqrMagnitude > 0f)
            sprite.flipX = input.x < 0f;
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }
}