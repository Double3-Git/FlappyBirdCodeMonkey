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
    private const float BIRD_X_POSITION = -68f;

    private static Level instance;
    private List<Pipe> pipesList;
    private int pipesPassedCount;
    private int pipesSpawned;
    private float pipeSpawnerTimer;
    private float pipeSpawnerTimerMax;
    private float gapSize;
    private State state;

    public static Level GetInstance() => instance;
    public int GetPipesPassedCount() => pipesPassedCount;
    
    public enum Difficulty
    {
        easy,
        medium,
        hard,
        impossible,
    }

    private enum State
    {
        waitingToStart,
        playing,
        gameOver,
    }

    // Метод Awake вызывается во время загрузки экземпляра сценария
    private void Awake()
    {
        // Singleton 
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        pipesList = new List<Pipe>();
        pipeSpawnerTimerMax = 1.5f;
        SetDifficulty(Difficulty.easy);
        pipesPassedCount = 0;

        state = State.waitingToStart;
    }


    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnBirdDiedHandler;
        Bird.GetInstance().OnStart += Bird_OnStart;
    }

    private void Bird_OnStart(object sender, System.EventArgs e)
    {
        state = State.playing;
    }

    private void Bird_OnBirdDiedHandler(object sender, System.EventArgs e)
    {
        state = State.gameOver;
    }

    // Метод Update вызывается на каждом кадре, если класс MonoBehaviour включен
    private void Update()
    {
        if (state == State.playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
        }
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
            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (pipe.IsBottom() && isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION)
            {
                // Pipe passes the bird
                pipesPassedCount++;
                SoundManager.PlaySound(SoundManager.Sounds.Score);
            }
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

        Pipe pipe = new Pipe(pipeHead, pipeBody, isBottom);
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

    public int GetPipesSpawned() => pipesSpawned;

    /*
     * Represents a single pipe
     */
    class Pipe
    {
        Transform pipeHeadTransform;
        Transform pipeBodyTransform;
        bool isBottom;

        public bool IsBottom() => isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
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
