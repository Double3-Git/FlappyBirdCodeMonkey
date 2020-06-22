using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{
    private const float JUMP_AMOUNT = 100f;

    private Rigidbody2D rb2d;

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        bool jumpButtionPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        if (jumpButtionPressed) Jump();
    }

    private void Jump()
    {
        rb2d.velocity = Vector2.up * JUMP_AMOUNT;
    }
}
