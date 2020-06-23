using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 10.4f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;


    private void Start()
    {
        CreatePipe(50f, 0f, true);
        CreatePipe(50f, 0f, false);
        CreatePipe(40f, 20f, true);
        CreatePipe(30f, 40f, true);
        CreatePipe(20f, 60f, true);
    }

    void CreatePipe(float height, float xPosition, bool isBottom)
    {
        int multipler = isBottom ? 1 : -1;
        // Setup pipe Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        pipeHead.position = new Vector3(xPosition, multipler * (- CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f));

        // Setup pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        pipeBody.position = new Vector3(xPosition, multipler * -CAMERA_ORTHO_SIZE);
        pipeBody.localScale = new Vector3(1, multipler, 1);
        SpriteRenderer pipeBodySR = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySR.size = new Vector2(PIPE_WIDTH, height);
        BoxCollider2D pipeBodyBC2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBC2D.size = pipeBodySR.size;
        pipeBodyBC2D.offset = new Vector2(0, height * .5f);
    }
}
