using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 10.4f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 3f;

    private List<Transform> pipesList;

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        pipesList = new List<Transform>();
    }


    private void Start()
    {
        //CreatePipe(40f, 0f, true);
        //CreatePipe(50f, 0f, false);
        CreateGapPipes(50f, 20f, 20f);
        CreateGapPipes(70f, 10f, 40f);
    }

    // Метод Update вызывается на каждом кадре, если класс MonoBehaviour включен
    private void Update()
    {
        HandlePipeMovement();
    }

    void HandlePipeMovement()
    {
        foreach (Transform pipeTransform in pipesList)
        {
            pipeTransform.position += Vector3.left * PIPE_MOVE_SPEED * Time.deltaTime;
        }
    }

    void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
    }
    void CreatePipe(float height, float xPosition, bool isBottom)
    {
        int multipler = isBottom ? 1 : -1;
        // Setup pipe Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        pipeHead.position = new Vector3(xPosition, multipler * (- CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f));
        pipesList.Add(pipeHead);

        // Setup pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        pipeBody.position = new Vector3(xPosition, multipler * -CAMERA_ORTHO_SIZE);
        pipeBody.localScale = new Vector3(1, multipler, 1);
        SpriteRenderer pipeBodySR = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySR.size = new Vector2(PIPE_WIDTH, height);
        BoxCollider2D pipeBodyBC2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBC2D.size = pipeBodySR.size;
        pipeBodyBC2D.offset = new Vector2(0, height * .5f);
        pipesList.Add(pipeBody);
    }
}
