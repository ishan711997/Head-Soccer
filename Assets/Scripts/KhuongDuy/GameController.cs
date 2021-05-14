using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Vector3
        playerComputerOriginalPos,
        playerOriginalPos;

    private int
        goals,
        goalsConceded;

    public static GameController Instance { get; private set; }

    public Color[] skyColors;

    public GameObject
        targetLeft,
        targetRight,
        ball,
        playerComputer,
        player;

    [Space]
    public Character2DController playerController;
    public AIPlayer aiController;

    [Space]
    [Header("Starting position of the ball")]
    [Space]
    public Vector3 posLeft;
    public Vector3
        posCenter,
        posRight;

    [Space]
    [Header("Player Character Reference")]
    [Space]
    public SpriteRenderer pHead;
    public SpriteRenderer
        pBody,
        pArm1,
        pArm2;

    [Space]
    [Header("AI Character Reference")]
    [Space]
    public SpriteRenderer aiHead;
    public SpriteRenderer
        aiBody,
        aiArm1,
        aiArm2;

    public bool Scored { get; private set; }
    public bool EndMatch { get; private set; }

	public float matchTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }
		Application.targetFrameRate = 60;
    }

    void Start()
    {
        playerComputerOriginalPos = playerComputer.transform.position;
        playerOriginalPos = player.transform.position;
    }

    public void StartGame()
    {
        SetObjectsInMatch(true);
        Reset();

        StartCoroutine("RunMatchTime");
    }

    private void Reset()
    {
        EndMatch = Scored = false;

        goals = goalsConceded = 0;
        UIManager.Instance.matchScore.text = "0 : 0";

        playerComputer.transform.position = playerComputerOriginalPos;
        player.transform.position = playerOriginalPos;

        ball.transform.position = posCenter;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        UIManager.Instance.controlTypeObj.SetActive(true);

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.referee.Play();
        }
    }

    public void SetObjectsInMatch(bool value)
    {
        if (!value)
        {
            StopCoroutine("CountDownTimeToRestart");
        }

        targetLeft.SetActive(value);
        targetRight.SetActive(value);
        ball.SetActive(value);
        playerComputer.SetActive(value);
        player.SetActive(value);
    }

    private IEnumerator RunMatchTime()
    {
		var time = matchTime;
        UIManager.Instance.timeText.text = time + "";
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            time--;
            UIManager.Instance.timeText.text = time + "";

            if (time == 0) { break; }
        }

        if (goals < goalsConceded)
        {
            playerController.Anim.SetBool(playerController.CryHash, true);
            aiController.anim.SetBool(aiController.CelebrateHash, true);

            UIManager.Instance.SetCup(UIManager.Instance.goldCupSprite, UIManager.Instance.silverCupSprite, "YOU LOSE !");

            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.matchLost.Play();
            }
        }
        else if (goals == goalsConceded)
        {
            playerController.Anim.SetBool(playerController.CelebrateHash, true);
            aiController.anim.SetBool(aiController.CelebrateHash, true);

            UIManager.Instance.SetCup(UIManager.Instance.goldCupSprite, UIManager.Instance.goldCupSprite, "DRAW !");

            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.matchWon.Play();
            }
        }
        else
        {
            playerController.Anim.SetBool(playerController.CelebrateHash, true);
            aiController.anim.SetBool(aiController.CryHash, true);

            UIManager.Instance.SetCup(UIManager.Instance.silverCupSprite, UIManager.Instance.goldCupSprite, "YOU WIN !");

            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.matchWon.Play();
            }
        }

        EndMatch = true;

        UIManager.Instance.controlTypeObj.SetActive(false);
        UIManager.Instance.inGameButtons.SetActive(true);

        StartCoroutine("CountDownTimeToRestart");
    }

    private IEnumerator CountDownTimeToRestart()
    {
		AdManager.instance.ShowFullScreenAd();     // Interstitial Ad
        
        var time = 8.0f;
        UIManager.Instance.restartTimeText.text = "Restart in " + time;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            time--;
            UIManager.Instance.restartTimeText.text = "Restart in " + time;

            if (time == 0) { break; }
        }

        RestartMatch();
    }

    public void ScoredAgainst(bool netOfAIPlayer)
    {
        if (!EndMatch)
        {
            Scored = true;

            if (netOfAIPlayer)
            {
                goals++;
                aiController.anim.SetBool(aiController.CryHash, true);
            }
            else
            {
                goalsConceded++;
                playerController.Anim.SetBool(playerController.CryHash, true);
            }

            UIManager.Instance.matchScore.text = goalsConceded + " : " + goals;

            StartCoroutine(ContinueMatch(netOfAIPlayer));
        }
    }

    private IEnumerator ContinueMatch(bool netOfAIPlayer)
    {
        yield return new WaitForSeconds(2.0f);

        if (!EndMatch)
        {
            playerComputer.transform.position = playerComputerOriginalPos;
            aiController.anim.SetBool(aiController.CryHash, false);

            player.transform.position = playerOriginalPos;
            playerController.Anim.SetBool(playerController.CryHash, false);

            if (netOfAIPlayer)
            {
                ball.transform.position = posLeft;
            }
            else
            {
                ball.transform.position = posRight;
            }
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            Scored = false;

            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.referee.Play();
            }
        }
    }

    public void RestartMatch()
    {
        StopCoroutine("CountDownTimeToRestart");

        UIManager.Instance.inGameButtons.SetActive(false);

        Reset();
        playerController.StopCry();
        playerController.Anim.SetBool(playerController.CryHash, false);
        playerController.Anim.SetBool(playerController.CelebrateHash, false);

        aiController.StopCry();
        aiController.anim.SetBool(aiController.CryHash, false);
        aiController.anim.SetBool(aiController.CelebrateHash, false);

        ChangeSkyColor();

        StartCoroutine("RunMatchTime");
    }

    public void Surrender()
    {
        StopCoroutine("RunMatchTime");

        playerController.Anim.SetBool(playerController.CryHash, true);
        aiController.anim.SetBool(aiController.CelebrateHash, true);

        UIManager.Instance.SetCup(UIManager.Instance.goldCupSprite, UIManager.Instance.silverCupSprite, "YOU LOSE !");

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.matchLost.Play();
        }

        Scored = false;
        EndMatch = true;

        UIManager.Instance.inGameButtons.SetActive(true);
        UIManager.Instance.controlTypeObj.SetActive(false);

        StartCoroutine("CountDownTimeToRestart");
    }

    public void ChangeSkyColor()
    {
        int index = Random.Range(0, 5);
        Camera.main.backgroundColor = skyColors[index];
    }
}
