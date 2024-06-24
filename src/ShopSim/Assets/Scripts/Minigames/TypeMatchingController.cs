using Auxiliars;
using TMPro;
using UnityEngine;

//TODO: Make an interface from this and refactor
[RequireComponent(typeof(PlayerInteractable))]
public class TypeMatchingController : MonoBehaviour
{
    private const string WORD_LIST_PATH = "RandomWords/WordList";
    private const string SHADER_RADIAL_PROP_NAME = "_Arc1";
    //Make a timer so that they have to input the word before it runs out

    private string[] m_loadedWords;

    public bool IsRunning => this.m_isRunning;

    [SerializeField]
    private TextMeshPro m_displayWordText;
    [SerializeField]
    private TextMeshPro m_matchingWordText;

    private bool m_isRunning;

    private IInteractable m_interactionHandler;

    private string m_loadedWord;

    SpartanTimer m_wordInputTimer;

    SpartanTimer m_forgivenessTimer; //Used to wait a bit on each error and at the beginning

    [SerializeField]
    private float m_maxWordInputTime;

    [SerializeField]
    private SpriteRenderer m_radialSpriteRenderer;

    [SerializeField]
    private int m_attempts;

    private int m_initialAttempts;
    private float m_initialMaxTime;

    private void Start()
    {
        this.m_interactionHandler = GetComponent<IInteractable>();
        //Reuse this with the next word list IF by any chance 300 words is not enough (I don't think so)
        this.m_loadedWords = this.LoadRandomWords();
        this.m_isRunning = false;
        this.m_radialSpriteRenderer.gameObject.SetActive(false);
        this.m_displayWordText.gameObject.SetActive(false);
        this.m_matchingWordText.gameObject.SetActive(false);
        this.m_initialAttempts = this.m_attempts;
        this.m_initialMaxTime = this.m_maxWordInputTime;
    }

    private void Update()
    {
        GameManager.s_IsInMiniGame = this.m_isRunning;
        if (!this.m_isRunning)
        {
            return;
        }

        this.m_displayWordText.text = "Match the words appearing on screen...\nGet Ready!";
        this.m_displayWordText.color = Color.yellow;

        if (this.m_forgivenessTimer.Started && this.m_forgivenessTimer.CurrentTimeMS < 2500f)
        {
            this.m_wordInputTimer.Stop();
            return;
        }

        if (!this.m_wordInputTimer.Started)
        {
            this.m_maxWordInputTime = this.m_initialMaxTime;
            this.m_wordInputTimer.Reset();
        }

        this.m_displayWordText.color = Color.white;
        this.m_displayWordText.outlineColor = Color.black;
        this.m_displayWordText.text = this.m_loadedWord;
        this.MiniGameLoop();
    }

    private void MiniGameLoop()
    {
        this.m_forgivenessTimer.Stop();
        MatchInputResult matchResult = this.HandleTypingInput();
        float elapsed = this.m_wordInputTimer.GetCurrentTime(TimeScaleMode.Seconds);

        if (matchResult == MatchInputResult.Failed || elapsed >= this.m_maxWordInputTime)
        {
            this.HandleLostAttemptFx();
            this.MoveToNextWord();
        }
        else if (matchResult == MatchInputResult.Matched)
        {
            this.HandleSuccesfulAttempt();
            this.MoveToNextWord();
        }

        //Display the time left
        this.TimerFx(elapsed);
    }

    private MatchInputResult HandleTypingInput()
    {
        if (!Input.anyKeyDown) return MatchInputResult.None;
        this.m_matchingWordText.text += Input.inputString;
        //We can either go through each string or just trim the expected
        //I decided to go for trimming bc it looks cleaner
        if (this.m_loadedWord == this.m_matchingWordText.text)
        {
            return MatchInputResult.Matched;
        }
        if (this.m_matchingWordText.text.Length > this.m_loadedWord.Length)
        {
            return MatchInputResult.Failed;
        }
        int trimIndex = Mathf.Clamp(this.m_matchingWordText.text.Length, 0, this.m_loadedWord.Length - 1);
        string trimmed = this.m_loadedWord.Remove(trimIndex);
        if (this.m_matchingWordText.text != trimmed)
        {
            return MatchInputResult.Failed;
        }
        return MatchInputResult.None;
    }

    private void TimerFx(float elapsed)
    {
        
        const float radialMaxAmount = 360f;
        float fillAmount = (elapsed / this.m_maxWordInputTime) * radialMaxAmount;
        this.m_radialSpriteRenderer.material.SetFloat(SHADER_RADIAL_PROP_NAME, fillAmount);
    }

    public void StartGame()
    {
        this.m_forgivenessTimer.Reset();
        this.m_loadedWord = this.FetchRandomWord();
        this.m_radialSpriteRenderer.gameObject.SetActive(true);
        this.m_displayWordText.gameObject.SetActive(true);
        this.m_matchingWordText.gameObject.SetActive(true);
        this.m_matchingWordText.text = string.Empty;
        this.m_attempts = this.m_initialAttempts;
        this.m_radialSpriteRenderer.material.SetFloat(SHADER_RADIAL_PROP_NAME, 0f);
        this.m_isRunning = true;
    }

    public void GameOver()
    {
        //Complete interaction
        this.m_isRunning = false;
        this.m_wordInputTimer.Stop();
        this.m_interactionHandler.CompleteInteraction();
        this.m_radialSpriteRenderer.gameObject.SetActive(false);
        this.m_displayWordText.gameObject.SetActive(false);
        this.m_matchingWordText.gameObject.SetActive(false);
    }

    private void MoveToNextWord()
    {
        this.m_loadedWord = this.FetchRandomWord();
        this.m_wordInputTimer.Reset();
        this.m_matchingWordText.text = string.Empty;
        this.m_maxWordInputTime -= Random.Range(0.1f, 0.5f);
        SpartanMath.Clamp(ref this.m_maxWordInputTime, 1f, float.MaxValue);
    }

    private void HandleLostAttemptFx()
    {
        //Send camera shake and a small audio cue
        EntityFetcher.s_CameraActions.SendCameraShake(0.1f, 1f);
        EntityFetcher.s_PlayerExpressions.TryEnqueueExpression(FacialExpression.Sad);
        this.m_attempts--;
        if (this.m_attempts <= 0)
        {
            this.GameOver();
        }
    }

    private void HandleSuccesfulAttempt()
    {
        EntityFetcher.s_PlayerExpressions.TryEnqueueExpression(FacialExpression.Happy);
        //Score
        ScoringManager.s_Money += Random.Range(10, 26);
    }

    private string FetchRandomWord()
    {
        int index = Random.Range(0, this.m_loadedWords.Length);
        return this.m_loadedWords[index];
    }

    private string[] LoadRandomWords()
    {
        //Optimization trick: Pray this gets stack allocated
        var textAsset = Resources.Load<TextAsset>(WORD_LIST_PATH);
        string[] result = textAsset.text.Split('\n');
        return result;
    }

}
