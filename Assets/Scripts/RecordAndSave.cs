using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RecordAndSave : MonoBehaviour
{
    AudioClip myAudioClip;
    string soundPath;
    private AudioFileSpeechToText server;

    void Start()
    {
        server = GameObject.FindObjectOfType<AudioFileSpeechToText>();
        System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => {
            return true;
        };
    }
    public void startRecording()
    {
        myAudioClip = Microphone.Start(null, true, 5, 16000);
    }
    public void stopRecording()
    {
        Microphone.End(null);
        SavWav.Save("Saved Audio/myaudio", myAudioClip);
    }
    public void PlayAudioFile()
    {
        soundPath = Application.dataPath + "/Saved Audio/myaudio.wav";
        Debug.Log("PlayAudioFile");

        // Read the audio file bytes
        byte[] audioBytes = File.ReadAllBytes(soundPath);

        AudioClip clip = WaveFile.ParseWAV("myClip", audioBytes);
        server.PlayClip(clip);
    }
}
