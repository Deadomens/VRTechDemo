using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionEngine : MonoBehaviour
{

    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    public float speed = 1;
   
    
    public Text results;
    public GameObject Cube;
    private Material cubeMaterial;


    public ParticleSystem ps;
    public Light PointLight;
    private float lightstep = 10;

    protected GrammarRecognizer grammarRecognizer;
    //protected PhraseRecognizer keywordRecognizer;
    protected string word = "";

    [System.Obsolete]
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        cubeMaterial = Cube.GetComponent<Renderer>().material;
        

        PhraseRecognitionSystem.OnError += PhraseRecognitionSystem_OnError;

        if (grammarRecognizer != null && grammarRecognizer.IsRunning)
        {
            Debug.Log("grammarRecognizer already exists");
            grammarRecognizer.OnPhraseRecognized -= GrammarRecognizer_OnPhraseRecognized;
            grammarRecognizer.Stop();
            grammarRecognizer.Dispose();
        }

        grammarRecognizer = new GrammarRecognizer(Application.streamingAssetsPath + "/SRGS/GrammarBasic.xml", confidence);
        grammarRecognizer.OnPhraseRecognized += GrammarRecognizer_OnPhraseRecognized;
        grammarRecognizer.Start();

        if (grammarRecognizer.IsRunning)
            Debug.Log("Start - grammarRecognizer is running from file: " + grammarRecognizer.GrammarFilePath);
    }

    private void PhraseRecognitionSystem_OnError(SpeechError errorCode)
    {
        Debug.Log("errorCode =" + errorCode.ToString());
    }

    private void GrammarRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        SemanticMeaning[] meanings = args.semanticMeanings;

        Debug.Log("GrammarRecognizer_OnPhraseRecognized: " + word);
        Debug.Log("GrammarRecognizer_OnPhraseRecognized - meanings: " + meanings.Length);

        results.text = "You said: <b>" + word + "</b>";
    }



    [System.Obsolete]
    private void Update()
    {
        switch (word)
        {
            case "levitate":
                Debug.Log("You used levitate");
                break;
            case "lumos":
                PointLight.enabled = true;
                
                OnLight();

                Debug.Log("You used Red");
                break;
            case "green":
                cubeMaterial.color = Color.green;
                Debug.Log("You used Green");
                break;
            case "yellow":
                cubeMaterial.color = Color.yellow;
                Debug.Log("You used Yellow");
                break;
            default:
                break;
        }    
        void OnLight()
    {
        ps.Play();
        PointLight.enabled = true;
    }
    void OffLight()
    {
        PointLight.enabled = false;
        ps.Stop();
    }
    }

    private void OnApplicationQuit()
    {

        if (grammarRecognizer != null && grammarRecognizer.IsRunning)
        {
            grammarRecognizer.OnPhraseRecognized -= GrammarRecognizer_OnPhraseRecognized;
            grammarRecognizer.Stop();
        }
    }

}

