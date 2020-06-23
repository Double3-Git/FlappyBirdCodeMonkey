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
        CreatePipe(50f, 0f);
        CreatePipe(40f, 20f);
        CreatePipe(30f, 40f);
        CreatePipe(20f, 60f);
    }

    void CreatePipe(float height, float xPosition)
    {
        // Setup pipe Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        pipeHead.position = new Vector3(xPosition, -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f);

        // Setup pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        pipeBody.position = new Vector3(xPosition, -CAMERA_ORTHO_SIZE);
        SpriteRenderer pipeBodySR = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySR.size = new Vector2(PIPE_WIDTH, height);
        BoxCollider2D pipeBodyBC2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBC2D.size = pipeBodySR.size;
        pipeBodyBC2D.offset = new Vector2(0, height * .5f);
    }
}
