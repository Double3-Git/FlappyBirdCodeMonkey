using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 10.4f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED =30f;
    private const float PIPE_DESTROY_X_POSITION = -110f;
    private const float PIPE_SPAWN_X_POSITION = 110f;

    private List<Pipe> pipesList;
    private int pipesSpawned;
    private float pipeSpawnerTimer;
    private float pipeSpawnerTimerMax;
    private float gapSize;

    public enum Difficulty
    {
        easy,
        medium,
        hard,
        impossible,
    }

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        pipesList = new List<Pipe>();
        pipeSpawnerTimerMax = 1.5f;
        SetDifficulty(Difficulty.easy);
    }


    private void Start()
    {
        //CreatePipe(40f, 0f, true);
        //CreatePipe(50f, 0f, false);
        CreateGapPipes(50f, gapSize, 20f);
    }

    // Метод Update вызывается на каждом кадре, если класс MonoBehaviour включен
    private void Update()
    {
        HandlePipeMovement();
        HandlePipeSpawning();
    }
    
    private void HandlePipeSpawning()
    {
        pipeSpawnerTimer -= Time.deltaTime;
        if (pipeSpawnerTimer < 0)
        {
            pipeSpawnerTimer += pipeSpawnerTimerMax;

            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }

    void HandlePipeMovement()
    {

        for(int i = 0; i < pipesList.Count; i++)
        {
            Pipe pipe = pipesList[i];
            pipe.Move();
            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                pipe.DestroySelf();
                pipesList.Remove(pipe);
                i--;
            }
        }
    }

    void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    void CreatePipe(float height, float xPosition, bool isBottom)
    {
        int multipler = isBottom ? 1 : -1;
        // Setup pipe Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        pipeHead.position = new Vector3(xPosition, multipler * (-CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f));
        //pipesList.Add(pipeHead);

        // Setup pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        pipeBody.position = new Vector3(xPosition, multipler * -CAMERA_ORTHO_SIZE);
        pipeBody.localScale = new Vector3(1, multipler, 1);
        SpriteRenderer pipeBodySR = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySR.size = new Vector2(PIPE_WIDTH, height);
        BoxCollider2D pipeBodyBC2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBC2D.size = pipeBodySR.size;
        pipeBodyBC2D.offset = new Vector2(0, height * .5f);
        //pipesList.Add(pipeBody);

        Pipe pipe = new Pipe(pipeHead, pipeBody);
        pipesList.Add(pipe);
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.easy:
                gapSize = 45f;
                pipeSpawnerTimerMax = 1.5f;
                break;
            case Difficulty.medium:
                gapSize = 35f;
                pipeSpawnerTimerMax = 1.2f;
                break;
            case Difficulty.hard:
                gapSize = 25f;
                pipeSpawnerTimerMax = .9f;
                break;
            case Difficulty.impossible:
                gapSize = 15f;
                pipeSpawnerTimerMax = .7f;
                break;
            default:
                break;
        }
    }

    public Difficulty GetDifficulty()
    {
        if (pipesSpawned > 30) return Difficulty.impossible;
        else if (pipesSpawned > 20) return Difficulty.hard;
        else if (pipesSpawned > 10) return Difficulty.medium;
        else return Difficulty.easy;
    }

    /*
     * Represents a single pipe
     */
    class Pipe
    {
        Transform pipeHeadTransform;
        Transform pipeBodyTransform;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
        }

        public void Move()
        {
            pipeHeadTransform.position += Vector3.left * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += Vector3.left * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeBodyTransform.transform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeBodyTransform.gameObject);
            Destroy(pipeHeadTransform.gameObject);
        }
    }

}
