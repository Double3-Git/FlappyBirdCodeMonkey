using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using CodeMonkey;
using UnityEditorInternal;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{
    private const float JUMP_AMOUNT = 100f;
    private static Bird instance;
    private Rigidbody2D rb2d;
    private State state;

    public event EventHandler OnDied;
    public event EventHandler OnStart;

    public static Bird GetInstance() => instance;
    
    private enum State
    {
        waitingToJump,
        Flying,
        Dead,
    }

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);


        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Static;
        state = State.waitingToJump;
    }



    // Update is called once per frame
    void Update()
    {
        bool jumpButtionPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        switch (state)
        {
            case State.waitingToJump:
                if (jumpButtionPressed)
                {
                    state = State.Flying;
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    OnStart?.Invoke(this, EventArgs.Empty);
                    Jump();
                }
                break;
            case State.Flying:
                if (jumpButtionPressed) Jump();
                break;
            case State.Dead:
                break;
            default:
                break;
        }

        
        
    }

    private void Jump()
    {
        rb2d.velocity = Vector2.up * JUMP_AMOUNT;
    }

    // OnTriggerEnter2D вызывается, когда Collider2D входит в триггер (только двухмерная физика)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CMDebug.TextPopupMouse("Dead!");
        rb2d.bodyType = RigidbodyType2D.Static;
        // Simplyfied
        // if (OnDied != null) OnDied(this, EventArgs.Empty);
        OnDied?.Invoke(this, EventArgs.Empty);
    }


}
