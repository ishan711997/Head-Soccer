using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject lastCheckMark = null;

    private Sprite[] currentHeadArray;

    private Image
        lastPlayerTeam = null,
        lastAITeam = null;

    private bool
        playerTurnPick,
        readyForGame;

    public static UIManager Instance { get; private set; }

    [Header("UI Refrence")]
    [Space]
    public TeamSpritesHolder teamSpritesHolder;

    public GameObject opening;
    public GameObject
        teamsBoard,
        team,
        inGame,
        inGameButtons,
        blackImage,
        settingMenu,
        setting,
        confirm,
        controlTypeObj,
        chooseControl;

    public GameObject[] controlTypes;

    [Space]
    public Image[] musicAndSoundImg;

    [Space(25)]
    public Sprite musicSprite;
    public Sprite
        soundSprite,
        muteSprite;

    [Space(25)]
    [Header("Teams Board")]
    [Space]
    public Image playerLabel;
    public Image
        chosenPlayerTeam,
        AILabel,
        chosenAITeam,
        playBtn;

    [Space(25)]
    [Header("Team toggle button sprites")]
    [Space]
    public Sprite on;
    public Sprite off;

    [Header("Odd sprites")]
    [Space]
    public Sprite[] otherSprites;
    public Sprite
        teamEmpty,
        check,
        play;

    [Space(25)]
    [Header("Choose Characters In Team")]
    [Space]
    public Image teamEnsign;

    public List<Character> characters;
    public GameObject[] checkMarks;

    [Space(25)]
    [Header("In Game")]
    [Space]
    public Image leftImg;
    public Image
        rightImg,
        playerEnsign,
        AIEnsign;

    public Image[] controlTypesButtonImg;

    public Text
        leftLabel,
        rightLabel,
        timeText,
        matchScore;

    [Space(25)]
    [Header("GameOver")]
    [Space]
    public Image leftImgCup;
    public Image rightImgRight;

    public Sprite
        goldCupSprite,
        silverCupSprite;

    public Text
        restartTimeText,
        notify;

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
    }

    void Start()
    {
        CheckMusicAndSound();
        CheckControlTypes();

        playerTurnPick = true;

        playDelegate = PlayGame;
        backDelegate = ExitGame;
    }

    private void CheckMusicAndSound()
    {
        if (PlayerPrefs.GetInt(GameConstants.MUSIC, 1) == 1)
        {
            for (var i = 0; i < musicAndSoundImg.Length; i++)
            {
                musicAndSoundImg[i].sprite = musicSprite;
            }

            SoundManager.Instance.music.Play();
        }
        else
        {
            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                for (var i = 0; i < musicAndSoundImg.Length; i++)
                {
                    musicAndSoundImg[i].sprite = soundSprite;
                }
            }
            else
            {
                for (var i = 0; i < musicAndSoundImg.Length; i++)
                {
                    musicAndSoundImg[i].sprite = muteSprite;
                }
            }
        }
    }

    private void CheckControlTypes()
    {
        int type = PlayerPrefs.GetInt(GameConstants.CONTROL_TYPES, 1);

        for (var i = 1; i <= controlTypes.Length; i++)
        {
            if (i == type)
            {
                controlTypes[i - 1].SetActive(true);
                controlTypesButtonImg[i - 1].sprite = on;
            }
            else
            {
                controlTypes[i - 1].SetActive(false);
                controlTypesButtonImg[i - 1].sprite = off;
            }
        }
    }

    public delegate void PlayDelegate();
    public PlayDelegate playDelegate;
    public void Play()
    {
        playDelegate();

        // AdManager.instance.HideBanner();

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }
    }

    private void PlayGame()
    {
        opening.SetActive(false);
        teamsBoard.SetActive(true);

        playDelegate = ConfirmTeam;
        backDelegate = RemoveTeams;

        GameController.Instance.ChangeSkyColor();
    }

    private void ConfirmTeam()
    {
        if (lastPlayerTeam != null)
        {
            if (playerTurnPick)
            {
                playerTurnPick = false;
                playerLabel.sprite = otherSprites[0];
                AILabel.sprite = otherSprites[1];
            }
            else
            {
                if (lastAITeam != null)
                {
                    readyForGame = true;
                    playBtn.sprite = play;
                    playBtn.SetNativeSize();

                    playDelegate = ShowCharacter;

                    SetCharacterForAI();
                    ShowCharacters();
                }
            }
        }
    }

    private void SetCharacterForAI()
    {
        switch (lastAITeam.name)
        {
            case "T1": SetCharForAI("BRAZIL", "Brazil", 8, teamSpritesHolder.bra_arm, teamSpritesHolder.bra_body, teamSpritesHolder.bra_heads);
                break;
            case "T2": SetCharForAI("CROATIA", null, 3, teamSpritesHolder.cro_arm, teamSpritesHolder.cro_body, teamSpritesHolder.cro_heads);
                break;
            case "T3": SetCharForAI("MEXICO", null, 3, teamSpritesHolder.mex_arm, teamSpritesHolder.mex_body, teamSpritesHolder.mex_heads);
                break;
            case "T4": SetCharForAI("CAMEROON", null, 2, teamSpritesHolder.cam_arm, teamSpritesHolder.cam_body, teamSpritesHolder.cam_heads);
                break;
            case "T5": SetCharForAI("SPAIN", null, 7, teamSpritesHolder.spa_arm, teamSpritesHolder.spa_body, teamSpritesHolder.spa_heads);
                break;
            case "T6": SetCharForAI("NETHERLANDS", "Netherlands", 6, teamSpritesHolder.net_arm, teamSpritesHolder.net_body, teamSpritesHolder.net_heads);
                break;
            case "T7": SetCharForAI("CHILE", null, 2, teamSpritesHolder.chi_arm, teamSpritesHolder.chi_body, teamSpritesHolder.chi_heads);
                break;
            case "T8": SetCharForAI("AUSTRALIA", null, 3, teamSpritesHolder.aus_arm, teamSpritesHolder.aus_body, teamSpritesHolder.aus_heads);
                break;
            case "T9": SetCharForAI("COLOMBIA", null, 3, teamSpritesHolder.col_arm, teamSpritesHolder.col_body, teamSpritesHolder.col_heads);
                break;
            case "T10": SetCharForAI("GREECE", null, 3, teamSpritesHolder.gre_arm, teamSpritesHolder.gre_body, teamSpritesHolder.gre_heads);
                break;
            case "T11": SetCharForAI("IVORY COAST", null, 2, teamSpritesHolder.ivo_arm, teamSpritesHolder.ivo_body, teamSpritesHolder.ivo_heads);
                break;
            case "T12": SetCharForAI("JAPAN", null, 4, teamSpritesHolder.jap_arm, teamSpritesHolder.jap_body, teamSpritesHolder.jap_heads);
                break;
            case "T13": SetCharForAI("URUGUAY", null, 5, teamSpritesHolder.uru_arm, teamSpritesHolder.uru_body, teamSpritesHolder.uru_heads);
                break;
            case "T14": SetCharForAI("COSTA RICA", null, 3, teamSpritesHolder.cos_arm, teamSpritesHolder.cos_body, teamSpritesHolder.cos_heads);
                break;
            case "T15": SetCharForAI("ENGLAND", null, 5, teamSpritesHolder.eng_arm, teamSpritesHolder.eng_body, teamSpritesHolder.eng_heads);
                break;
            case "T16": SetCharForAI("ITALY", null, 4, teamSpritesHolder.ita_arm, teamSpritesHolder.ita_body, teamSpritesHolder.ita_heads);
                break;
            case "T17": SetCharForAI("SWITZERLAND", null, 4, teamSpritesHolder.swi_arm, teamSpritesHolder.swi_body, teamSpritesHolder.swi_heads);
                break;
            case "T18": SetCharForAI("ECUADOR", null, 2, teamSpritesHolder.ecu_arm, teamSpritesHolder.ecu_body, teamSpritesHolder.ecu_heads);
                break;
            case "T19": SetCharForAI("FRANCE", null, 4, teamSpritesHolder.fra_arm, teamSpritesHolder.fra_body, teamSpritesHolder.fra_heads);
                break;
            case "T20": SetCharForAI("HONDURAS", null, 2, teamSpritesHolder.hon_arm, teamSpritesHolder.hon_body, teamSpritesHolder.hon_heads);
                break;
            case "T21": SetCharForAI("ARGENTINA", null, 5, teamSpritesHolder.arg_arm, teamSpritesHolder.arg_body, teamSpritesHolder.arg_heads);
                break;
            case "T22": SetCharForAI("SNIA AND HERZEGOVINA", null, 2, teamSpritesHolder.sni_arm, teamSpritesHolder.sni_body, teamSpritesHolder.sni_heads);
                break;
            case "T23": SetCharForAI("IRAN", null, 2, teamSpritesHolder.iran_arm, teamSpritesHolder.iran_body, teamSpritesHolder.iran_heads);
                break;
            case "T24": SetCharForAI("NIGERIA", null, 2, teamSpritesHolder.nig_arm, teamSpritesHolder.nig_body, teamSpritesHolder.nig_heads);
                break;
            case "T25": SetCharForAI("GERMANY", null, 6, teamSpritesHolder.ger_arm, teamSpritesHolder.ger_body, teamSpritesHolder.ger_heads);
                break;
            case "T26": SetCharForAI("PORTUGAL", null, 4, teamSpritesHolder.por_arm, teamSpritesHolder.por_body, teamSpritesHolder.por_heads);
                break;
            case "T27": SetCharForAI("GHANA", null, 2, teamSpritesHolder.gha_arm, teamSpritesHolder.gha_body, teamSpritesHolder.gha_heads);
                break;
            case "T28": SetCharForAI("USA", null, 8, teamSpritesHolder.usa_arm, teamSpritesHolder.usa_body, teamSpritesHolder.usa_heads);
                break;
            case "T29": SetCharForAI("BELGIUM", null, 4, teamSpritesHolder.bel_arm, teamSpritesHolder.bel_body, teamSpritesHolder.bel_heads);
                break;
            case "T30": SetCharForAI("ALGERIA", null, 2, teamSpritesHolder.alg_arm, teamSpritesHolder.alg_body, teamSpritesHolder.alg_heads);
                break;
            case "T31": SetCharForAI("RUSSIA", null, 4, teamSpritesHolder.rus_arm, teamSpritesHolder.rus_body, teamSpritesHolder.rus_heads);
                break;
            case "T32": SetCharForAI("SOUTH KOREA", null, 3, teamSpritesHolder.sko_arm, teamSpritesHolder.sko_body, teamSpritesHolder.sko_heads);
                break;
        }
    }

    private void SetCharForAI(string teamName, string specialString, int CharacterNumber, Sprite arm, Sprite body, Sprite[] heads)
    {
        leftLabel.text = teamName;

        int randomIndex = Random.Range(0, CharacterNumber);

        if (specialString != null)
        {
            if (specialString == "Brazil")
            {
                if (randomIndex == 7)
                {
                    GameController.Instance.aiArm1.sprite = teamSpritesHolder.bra_arm2;
                    GameController.Instance.aiArm2.sprite = teamSpritesHolder.bra_arm2;
                    GameController.Instance.aiBody.sprite = teamSpritesHolder.bra_body2;
                }
                else
                {
                    GameController.Instance.aiArm1.sprite = arm;
                    GameController.Instance.aiArm2.sprite = arm;
                    GameController.Instance.aiBody.sprite = body;
                }
            }
            else if (specialString == "Netherlands")
            {
                if (randomIndex == 1)
                {
                    GameController.Instance.aiArm1.sprite = teamSpritesHolder.net_arm2;
                    GameController.Instance.aiArm2.sprite = teamSpritesHolder.net_arm2;
                    GameController.Instance.aiBody.sprite = body;
                }
                else
                {
                    GameController.Instance.aiArm1.sprite = arm;
                    GameController.Instance.aiArm2.sprite = arm;
                    GameController.Instance.aiBody.sprite = body;
                }
            }
        }
        else
        {
            GameController.Instance.aiArm1.sprite = arm;
            GameController.Instance.aiArm2.sprite = arm;
            GameController.Instance.aiBody.sprite = body;
        }
        GameController.Instance.aiHead.sprite = heads[randomIndex];
    }

    private void ShowCharacters()
    {
        switch (lastPlayerTeam.name)
        {
            case "T1": SetChar("BRAZIL", 8, teamSpritesHolder.bra_arm, teamSpritesHolder.bra_arm2, 7,
                teamSpritesHolder.bra_body, teamSpritesHolder.bra_body2, teamSpritesHolder.bra_heads);
                break;
            case "T2": SetChar("CROATIA", 3, teamSpritesHolder.cro_arm, null, 0, teamSpritesHolder.cro_body, null, teamSpritesHolder.cro_heads);
                break;
            case "T3": SetChar("MEXICO", 3, teamSpritesHolder.mex_arm, null, 0, teamSpritesHolder.mex_body, null, teamSpritesHolder.mex_heads);
                break;
            case "T4": SetChar("CAMEROON", 2, teamSpritesHolder.cam_arm, null, 0, teamSpritesHolder.cam_body, null, teamSpritesHolder.cam_heads);
                break;
            case "T5": SetChar("SPAIN", 7, teamSpritesHolder.spa_arm, null, 0, teamSpritesHolder.spa_body, null, teamSpritesHolder.spa_heads);
                break;
            case "T6": SetChar("NETHERLANDS", 6, teamSpritesHolder.net_arm, teamSpritesHolder.net_arm2, 1,
                teamSpritesHolder.net_body, null, teamSpritesHolder.net_heads);
                break;
            case "T7": SetChar("CHILE", 2, teamSpritesHolder.chi_arm, null, 0, teamSpritesHolder.chi_body, null, teamSpritesHolder.chi_heads);
                break;
            case "T8": SetChar("AUSTRALIA", 3, teamSpritesHolder.aus_arm, null, 0, teamSpritesHolder.aus_body, null, teamSpritesHolder.aus_heads);
                break;
            case "T9": SetChar("COLOMBIA", 3, teamSpritesHolder.col_arm, null, 0, teamSpritesHolder.col_body, null, teamSpritesHolder.col_heads);
                break;
            case "T10": SetChar("GREECE", 3, teamSpritesHolder.gre_arm, null, 0, teamSpritesHolder.gre_body, null, teamSpritesHolder.gre_heads);
                break;
            case "T11": SetChar("IVORY COAST", 2, teamSpritesHolder.ivo_arm, null, 0, teamSpritesHolder.ivo_body, null, teamSpritesHolder.ivo_heads);
                break;
            case "T12": SetChar("JAPAN", 4, teamSpritesHolder.jap_arm, null, 0, teamSpritesHolder.jap_body, null, teamSpritesHolder.jap_heads);
                break;
            case "T13": SetChar("URUGUAY", 5, teamSpritesHolder.uru_arm, null, 0, teamSpritesHolder.uru_body, null, teamSpritesHolder.uru_heads);
                break;
            case "T14": SetChar("COSTA RICA", 3, teamSpritesHolder.cos_arm, null, 0, teamSpritesHolder.cos_body, null, teamSpritesHolder.cos_heads);
                break;
            case "T15": SetChar("ENGLAND", 5, teamSpritesHolder.eng_arm, null, 0, teamSpritesHolder.eng_body, null, teamSpritesHolder.eng_heads);
                break;
            case "T16": SetChar("ITALY", 4, teamSpritesHolder.ita_arm, null, 0, teamSpritesHolder.ita_body, null, teamSpritesHolder.ita_heads);
                break;
            case "T17": SetChar("SWITZERLAND", 4, teamSpritesHolder.swi_arm, null, 0, teamSpritesHolder.swi_body, null, teamSpritesHolder.swi_heads);
                break;
            case "T18": SetChar("ECUADOR", 2, teamSpritesHolder.ecu_arm, null, 0, teamSpritesHolder.ecu_body, null, teamSpritesHolder.ecu_heads);
                break;
            case "T19": SetChar("FRANCE", 4, teamSpritesHolder.fra_arm, null, 0, teamSpritesHolder.fra_body, null, teamSpritesHolder.fra_heads);
                break;
            case "T20": SetChar("HONDURAS", 2, teamSpritesHolder.hon_arm, null, 0, teamSpritesHolder.hon_body, null, teamSpritesHolder.hon_heads);
                break;
            case "T21": SetChar("ARGENTINA", 5, teamSpritesHolder.arg_arm, null, 0, teamSpritesHolder.arg_body, null, teamSpritesHolder.arg_heads);
                break;
            case "T22": SetChar("SNIA AND HERZEGOVINA", 2, teamSpritesHolder.sni_arm, null, 0, teamSpritesHolder.sni_body, null, teamSpritesHolder.sni_heads);
                break;
            case "T23": SetChar("IRAN", 2, teamSpritesHolder.iran_arm, null, 0, teamSpritesHolder.iran_body, null, teamSpritesHolder.iran_heads);
                break;
            case "T24": SetChar("NIGERIA", 2, teamSpritesHolder.nig_arm, null, 0, teamSpritesHolder.nig_body, null, teamSpritesHolder.nig_heads);
                break;
            case "T25": SetChar("GERMANY", 6, teamSpritesHolder.ger_arm, null, 0, teamSpritesHolder.ger_body, null, teamSpritesHolder.ger_heads);
                break;
            case "T26": SetChar("PORTUGAL", 4, teamSpritesHolder.por_arm, null, 0, teamSpritesHolder.por_body, null, teamSpritesHolder.por_heads);
                break;
            case "T27": SetChar("GHANA", 2, teamSpritesHolder.gha_arm, null, 0, teamSpritesHolder.gha_body, null, teamSpritesHolder.gha_heads);
                break;
            case "T28": SetChar("USA", 8, teamSpritesHolder.usa_arm, null, 0, teamSpritesHolder.usa_body, null, teamSpritesHolder.usa_heads);
                break;
            case "T29": SetChar("BELGIUM", 4, teamSpritesHolder.bel_arm, null, 0, teamSpritesHolder.bel_body, null, teamSpritesHolder.bel_heads);
                break;
            case "T30": SetChar("ALGERIA", 2, teamSpritesHolder.alg_arm, null, 0, teamSpritesHolder.alg_body, null, teamSpritesHolder.alg_heads);
                break;
            case "T31": SetChar("RUSSIA", 4, teamSpritesHolder.rus_arm, null, 0, teamSpritesHolder.rus_body, null, teamSpritesHolder.rus_heads);
                break;
            case "T32": SetChar("SOUTH KOREA", 3, teamSpritesHolder.sko_arm, null, 0, teamSpritesHolder.sko_body, null, teamSpritesHolder.sko_heads);
                break;
        }
    }

    private void SetChar(string teamName, int CharacterNumber, Sprite arm1, Sprite arm2, int strangerIndex, Sprite body1, Sprite body2, Sprite[] heads)
    {
        rightLabel.text = teamName;

        for (var i = 0; i < CharacterNumber; i++)
        {
            if (arm2 != null && i == strangerIndex)
            {
                characters[i].arm1.sprite = arm2;
                characters[i].arm2.sprite = arm2;

                if (body2 != null)
                    characters[i].body.sprite = body2;
                else
                    characters[i].body.sprite = body1;
            }
            else
            {
                characters[i].arm1.sprite = arm1;
                characters[i].arm2.sprite = arm1;
                characters[i].body.sprite = body1;
            }

            characters[i].head.sprite = heads[i];
            characters[i].head.SetNativeSize();
            characters[i].body.transform.parent.gameObject.SetActive(true);
        }

        // Set body and arm for player in game
        GameController.Instance.pArm1.sprite = arm1;
        GameController.Instance.pArm2.sprite = arm1;
        GameController.Instance.pBody.sprite = body1;

        currentHeadArray = heads;

        int index = PlayerPrefs.GetInt(lastPlayerTeam.name, 0);

        SetCharacterForPlayer(index);
        lastCheckMark = checkMarks[index];
        lastCheckMark.SetActive(true);
    }

    private void SetCharacterForPlayer(int index)
    {
        if (lastPlayerTeam.name == "T1")
        {
            if (index == 7)
            {
                GameController.Instance.pArm1.sprite = teamSpritesHolder.bra_arm2;
                GameController.Instance.pArm2.sprite = teamSpritesHolder.bra_arm2;
                GameController.Instance.pBody.sprite = teamSpritesHolder.bra_body2;
            }
            else
            {
                GameController.Instance.pArm1.sprite = teamSpritesHolder.bra_arm;
                GameController.Instance.pArm2.sprite = teamSpritesHolder.bra_arm;
                GameController.Instance.pBody.sprite = teamSpritesHolder.bra_body;
            }
        }
        else if (lastPlayerTeam.name == "T6")
        {
            if (index == 1)
            {
                GameController.Instance.pArm1.sprite = teamSpritesHolder.net_arm2;
                GameController.Instance.pArm2.sprite = teamSpritesHolder.net_arm2;
            }
            else
            {
                GameController.Instance.pArm1.sprite = teamSpritesHolder.net_arm;
                GameController.Instance.pArm2.sprite = teamSpritesHolder.net_arm;
            }
        }

        GameController.Instance.pHead.sprite = currentHeadArray[index];
    }

    private void ShowCharacter()
    {
        teamsBoard.SetActive(false);
        team.SetActive(true);

        playDelegate = StartGame;
        backDelegate = ShowBoard;

        GameController.Instance.ChangeSkyColor();
    }

    private void StartGame()
    {
        team.SetActive(false);
        inGame.SetActive(true);
        blackImage.SetActive(true);

        playDelegate = ResumeGame;
        backDelegate = ShowBoard;

        GameController.Instance.ChangeSkyColor();
        GameController.Instance.StartGame();

        Time.timeScale = 0.0f;
    }

    private void ResumeGame()
    {
        blackImage.SetActive(false);
        Time.timeScale = 1.0f;

        playDelegate = RestartMatch;
    }

    private void RestartMatch()
    {
        GameController.Instance.RestartMatch();
        Time.timeScale = 0.0f;

        blackImage.SetActive(true);

        playDelegate = ResumeGame;
    }

    public delegate void BackDelegate();
    public BackDelegate backDelegate;
    public void GoBack()
    {
        backDelegate();

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void RemoveTeams()
    {
        playerTurnPick = true;
        readyForGame = false;

        if (lastPlayerTeam != null)
        {
            lastPlayerTeam.sprite = off;
            lastPlayerTeam = null;
        }

        if (lastAITeam != null)
        {
            lastAITeam.sprite = off;
            lastAITeam = null;
        }

        playerLabel.sprite = otherSprites[1];
        AILabel.sprite = otherSprites[0];
        chosenPlayerTeam.sprite = teamEmpty;
        chosenAITeam.sprite = teamEmpty;
        playBtn.sprite = check;
        playBtn.SetNativeSize();

        playDelegate = ConfirmTeam;

        // Deactivate characters
        for (var i = 7; i >= 0; i--)
        {
            characters[i].body.transform.parent.gameObject.SetActive(false);
        }

        if (lastCheckMark != null)
            lastCheckMark.SetActive(false);
    }

    private void Resume()
    {
        settingMenu.SetActive(false);
        Time.timeScale = 1.0f;

        backDelegate = ShowBoard;
    }

    private void ShowBoard()
    {
        team.SetActive(false);
        inGameButtons.SetActive(false);
        inGame.SetActive(false);
        teamsBoard.SetActive(true);

        GameController.Instance.SetObjectsInMatch(false);
        RemoveTeams();

        playDelegate = ConfirmTeam;
        backDelegate = RemoveTeams;

        GameController.Instance.ChangeSkyColor();
    }

    public void ChooseTeam(Image buttonImg)
    {
        if (!readyForGame)
        {
            buttonImg.sprite = on;
            Image ensignImg = buttonImg.GetComponentsInChildren<Image>()[1];

            if (playerTurnPick)
            {
                chosenPlayerTeam.sprite = ensignImg.sprite;
                rightImg.sprite = ensignImg.sprite;
                playerEnsign.sprite = ensignImg.sprite;

                teamEnsign.sprite = ensignImg.sprite;

                if (lastPlayerTeam != null)
                {
                    if (lastPlayerTeam != buttonImg)
                        lastPlayerTeam.sprite = off;
                }
                lastPlayerTeam = buttonImg;
            }
            else
            {
                if (buttonImg != lastPlayerTeam)
                {
                    chosenAITeam.sprite = ensignImg.sprite;
                    leftImg.sprite = ensignImg.sprite;
                    AIEnsign.sprite = ensignImg.sprite;

                    if (lastAITeam != null)
                    {
                        if (lastAITeam != buttonImg)
                            lastAITeam.sprite = off;
                    }
                    lastAITeam = buttonImg;
                }
            }

            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonClick.Play();
            }
        }
    }

    public void ChooseCharacter(int index)
    {
        if (lastCheckMark != null)
        {
            lastCheckMark.SetActive(false);
        }
        lastCheckMark = checkMarks[index];
        lastCheckMark.SetActive(true);

        SetCharacterForPlayer(index);

        PlayerPrefs.SetInt(lastPlayerTeam.name, index);

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.lockCharacter.Play();
        }
    }

    public void SetCup(Sprite cup1, Sprite cup2, string str)
    {
        leftImgCup.sprite = cup1;
        rightImgRight.sprite = cup2;
        notify.text = str;
    }

    // Sound btn is clicked
    public void MusicAndSound()
    {
        if (musicAndSoundImg[0].sprite == musicSprite)
        {
            for (var i = 0; i < musicAndSoundImg.Length; i++)
            {
                musicAndSoundImg[i].sprite = soundSprite;
            }

            PlayerPrefs.SetInt(GameConstants.MUSIC, 0);
            SoundManager.Instance.music.Stop();

            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonClick.Play();
            }
        }
        else if (musicAndSoundImg[0].sprite == soundSprite)
        {
            for (var i = 0; i < musicAndSoundImg.Length; i++)
            {
                musicAndSoundImg[i].sprite = muteSprite;
            }

            PlayerPrefs.SetInt(GameConstants.SOUND, 0);
        }
        else if (musicAndSoundImg[0].sprite == muteSprite)
        {
            for (var i = 0; i < musicAndSoundImg.Length; i++)
            {
                musicAndSoundImg[i].sprite = musicSprite;
            }

            PlayerPrefs.SetInt(GameConstants.MUSIC, 1);
            SoundManager.Instance.music.Play();
            PlayerPrefs.SetInt(GameConstants.SOUND, 1);
        }
    }

    // Setting btn is clicked
    public void Setting()
    {
        if (!GameController.Instance.EndMatch)
        {
            settingMenu.SetActive(true);
            Time.timeScale = 0.0f;

            backDelegate = Resume;

            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonClick.Play();
            }
        }
    }

    // Surrender btn is clicked
    public void Surrender()
    {
        
        setting.SetActive(false);
        confirm.SetActive(true);

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }
    }

    // Surrender
    public void NoBtn()
    {
        confirm.SetActive(false);
        setting.SetActive(true);

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }
    }

    // Surrender
    public void YesBtn()
    {

        Time.timeScale = 1.0f;

        confirm.SetActive(false);
        setting.SetActive(true);
        settingMenu.SetActive(false);
        GameController.Instance.Surrender();

        backDelegate = ShowBoard;

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }

        AdManager.instance.ShowFullScreenAd(); // Interstitial ad

    }

    public void ControlBtn()
    {
        setting.SetActive(false);
        chooseControl.SetActive(true);

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }
    }

    public void ChooseControl(int type)
    {
        for (var i = 1; i <= controlTypes.Length; i++)
        {
            if (i == type)
            {
                controlTypes[i - 1].SetActive(true);
                controlTypesButtonImg[i - 1].sprite = on;

                PlayerPrefs.SetInt(GameConstants.CONTROL_TYPES, i);
            }
            else
            {
                controlTypes[i - 1].SetActive(false);
                controlTypesButtonImg[i - 1].sprite = off;
            }
        }

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }
    }

    // Choose control
    public void Yes2Btn()
    {
        Time.timeScale = 1.0f;

        chooseControl.SetActive(false);
        setting.SetActive(true);
        settingMenu.SetActive(false);

        backDelegate = ShowBoard;

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.buttonClick.Play();
        }
    }

    [System.Serializable]
    public class Character
    {
        public Image
            arm1,
            arm2,
            body,
            head;
    }
}
