using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using IBM.Watson.SpeechToText.V1;
using IBM.Watson.SpeechToText.V1.Model;


using System.IO;

using UnityEngine.UI;

public class AudioFileSpeechToText : MonoBehaviour
{

    // Place you API key and service URL in the Unity inspector.
    [SerializeField]
    private string myApiKey; // API key for your IBM text to speech

    [SerializeField]
    private string myServiceUrl; // Servuce URL for your IBM text to speech

    [SerializeField]
    private InputField audioFilePathInputField;

    public Text transcribedSpeech;
    public Button playButton;
    public Button convertAudioToTextButton;
    public Button s1button;

    private SpeechToTextService speechToText;

    //Path to wav sound file
    private string soundPath;


    private AudioSource audioSource;
    byte[] audioBytes;

    List<string> words; // List of words from the transcribed text
    List<float> times; // List of start times for each word
    AudioClip speechClip; // The AudioClip of the speech
    bool showTimedText; // Flag to show the the timed text
    float startTime; // Starting time which is reset right before the AudioClip is played
    int wordCounter; // An index into the list of words

    void Start()
    {
        showTimedText = false;
        wordCounter = 0;

        if (string.IsNullOrEmpty(myApiKey))
        {
            myApiKey = "PLACE YOUR API KEY HERE";
        }
        if (string.IsNullOrEmpty(myServiceUrl))
        {
            myServiceUrl = "https://PLACE.YOUR.SERVICE.URL.HERE";
        }

        StartCoroutine(CreateService());

        soundPath = Application.dataPath + "/Saved Audio/jork3.wav";
        //soundPath = Application.streamingAssetsPath + "/jork2.wav";

        audioFilePathInputField.text = soundPath;
      
        // Start the conversion of the audio file to text
        //StartCoroutine(ConvertAudioFileToText());

        playButton.onClick.AddListener(delegate { PlayAudioFile(audioFilePathInputField.text); });

        // Add listerner for button to covert audio file to text
        convertAudioToTextButton.onClick.AddListener(delegate { StartCoroutine(ConvertAudioFileToText(audioFilePathInputField.text)); });
        s1button.onClick.AddListener(delegate { StartCoroutine(ConvertAudioFileToText(Application.dataPath + "/Saved Audio/myaudio.wav")); });


    }

    void Update()
    {
        // Show each word of the transcribed speech at the correct time on the basis of its timestamp.
        if (showTimedText)
        {

            float deltaTime = Time.realtimeSinceStartup - startTime; // time from start of playing the audio clip

            // If the time since start of playing is at the start time of the timestamp of the current word as indexed by wordCounter, then
            // print out the word
            if (deltaTime >= times[wordCounter])
            {
                Debug.Log(words[wordCounter]);
                transcribedSpeech.text += words[wordCounter] + " ";
                wordCounter++;

                // After the last word is shown, stop trying to show more words
                if (wordCounter >= words.Count)
                {
                    showTimedText = false;
                }
            }

        }

    }

    private IEnumerator CreateService()
    {
        var authenticator = new IamAuthenticator(
            apikey: myApiKey
        );

        while (!authenticator.CanAuthenticate())
            yield return null;

        speechToText = new SpeechToTextService(authenticator);
        speechToText.SetServiceUrl(myServiceUrl);
    }

    private IEnumerator WaitForService()
    {
        yield return StartCoroutine(CreateService());
    }

    public IEnumerator ConvertAudioFileToText(string audioFilePath)
    {
        Debug.Log("ConvertAudioFileToText");

        transcribedSpeech.text = "";

        // If the speech-to-text service has not been created yet, start creating it and wait for it
        if (speechToText == null)
        {
            yield return StartCoroutine(CreateService());
        }

        // Read the audio file bytes
        byte[] audioBytes = File.ReadAllBytes(audioFilePath);
        speechClip = WaveFile.ParseWAV("myClip", audioBytes);

        SpeechRecognitionResults recognizeResponse = null;
        speechToText.Recognize(
            callback: (DetailedResponse<SpeechRecognitionResults> response, IBMError error) =>
            {
                Debug.Log("Converting speech to text...");
                Log.Debug("SpeechToTextServiceV1", "Recognize result: {0}", response.Response);
                Debug.Log("The response is: " + response.Response);
                recognizeResponse = response.Result;
            },
            audio: audioBytes,
            contentType: "audio/wav", // Other audio file formats can be used
            timestamps: true // The default is to now show the timestamp for each word, so this must be set to true to get timestamps.
        );

        while (recognizeResponse == null)
        {
            yield return null;
        }

        int resultNumber = 1;
        words = new List<string>();
        times = new List<float>();

        foreach (var res in recognizeResponse.Results)
        {
            foreach (var alt in res.Alternatives)
            {
                Debug.Log(alt.Transcript);
                transcribedSpeech.text += resultNumber.ToString() + ") " + alt.Transcript + "\n";
                resultNumber++;
                foreach (var ts in alt.Timestamps)
                {
                    Debug.Log(ts[0] + ", " + ts[1]);
                    words.Add(ts[0]);
                    times.Add(float.Parse(ts[1]));
                }
            }
        }
        transcribedSpeech.text += "\n";
        PlayClipWithText(speechClip);

        //        Debug.Log("Result is: " + recognizeResponse.Results[0].Alternatives[0].Transcript);
    }

    private void PlayAudioFile(string audioFilePath)
    {
        Debug.Log("PlayAudioFile");

        // Read the audio file bytes
        byte[] audioBytes = File.ReadAllBytes(audioFilePath);

        AudioClip clip = WaveFile.ParseWAV("myClip", audioBytes);
        PlayClip(clip);

    }

    public void PlayClip(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (clip != null)
        {
            audioSource.spatialBlend = 0.0f;
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void PlayClipWithText(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (clip != null)
        {
            audioSource.spatialBlend = 0.0f;
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.Play();
            startTime = Time.realtimeSinceStartup;
            showTimedText = true;
        }
    }

}
