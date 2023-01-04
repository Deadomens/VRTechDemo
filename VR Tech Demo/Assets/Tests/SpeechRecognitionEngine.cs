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

    public GameObject wand;
    public Transform FireballSpawner;
    public GameObject Fireball;
    public GameObject Bang;
    public ParticleSystem ps;
    public Light PointLight;
    private float lightstep = 10;
    public AudioSource LumosAudio;

    protected GrammarRecognizer grammarRecognizer;
    //protected PhraseRecognizer keywordRecognizer;
    protected string word = "";

    [System.Obsolete]
    private void Start()
    {
        
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

                if (PointLight.enabled == true)
                {
                    OffLight();
                }
                else
                {
                    OnLight();
                }

                word = ""; // stop this triggering again on the next frame
                Debug.Log("You used Lumos");
                break;
            case "fireball":

                MakeFireball();

                word = ""; // stop this triggering again on the next frame
                break;
            case "bang":

                MakeBang();

                word = ""; // stop this triggering again on the next frame
                break;
            default:
                break;
        }    
        void OnLight()
    {
        ps.Play();
            LumosAudio.Play();
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

    private void MakeFireball()
    {

        GameObject FB = Instantiate(Fireball, wand.transform.position + (wand.transform.right * 0.1f), Quaternion.identity);
        Rigidbody RB = FB.GetComponent<Rigidbody>();
        RB.AddForce(wand.transform.right * -10f);

    }

    private void MakeBang()
    {

        GameObject BG = Instantiate(Bang, wand.transform.position + (wand.transform.right * -0.3f), Quaternion.identity);
        Rigidbody RB = BG.GetComponent<Rigidbody>();

    }

}

