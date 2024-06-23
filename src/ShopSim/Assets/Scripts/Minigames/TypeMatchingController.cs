using Auxiliars;
using TMPro;
using UnityEngine;

//TODO: Make an interface from this
[RequireComponent(typeof(PlayerInteractable))]
public class TypeMatchingController : MonoBehaviour
{
    private const string WORD_LIST_PATH = "RandomWords/WordList1";
    //Make a timer so that they have to input the word before it runs out

    private string[] m_loadedWords;

    public bool IsRunning => this.m_isRunning;

    [SerializeField]
    private TextMeshPro m_displayWordText;

    private bool m_isRunning;

    private IInteractable m_interactionHandler;

    private string m_loadedWord;

    SpartanTimer m_wordInputTimer;

    [SerializeField]
    private float m_maxWordInputTime;

    [SerializeField]
    private SpriteRenderer m_radialSpriteRenderer;

    [SerializeField]
    private int m_attempts;

    private void Start()
    {
        //Reuse this with the next word list IF by any chance 300 words is not enough (I don't think so)
        this.m_loadedWords = this.LoadRandomWords();
        this.m_isRunning = false;
    }

    private void Update()
    {
        if (!this.m_isRunning)
        {
            return;
        }
        this.HandleLostAttemptFx();
        this.MiniGameLoop();
    }

    private void MiniGameLoop()
    {
        float elapsed = this.m_wordInputTimer.GetCurrentTime(TimeScaleMode.Seconds);
        if (elapsed >= this.m_maxWordInputTime)
        {
            this.m_wordInputTimer.Reset();
            this.m_maxWordInputTime -= Random.Range(0.1f, 0.5f);
            SpartanMath.Clamp(ref this.m_maxWordInputTime, 1f, float.MaxValue);
        }
        this.m_displayWordText.text = this.m_loadedWord;

        //Display the time left
        this.TimerFx(elapsed);
    }

    private void TimerFx(float elapsed)
    {
        const string shaderRadialPropertyName = "_Arc1";
        const float radialMaxAmount = 360f;
        float fillAmount = (elapsed / this.m_maxWordInputTime) * radialMaxAmount;
        this.m_radialSpriteRenderer.material.SetFloat(shaderRadialPropertyName, fillAmount);
    }

    public void StartGame()
    {
        this.m_isRunning = true;
        this.m_wordInputTimer.Start();
        this.m_loadedWord = this.FetchRandomWord();
    }

    public void GameOver()
    {
        //Complete interaction
        this.m_interactionHandler.CompleteInteraction();
    }

    private void HandleLostAttemptFx()
    {
        //Send camera shake and a small audio cue
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
