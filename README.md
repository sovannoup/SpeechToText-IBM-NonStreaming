# Unity-Watson-audio-file-speech-to-text



Scott Hwang

snhwang@alum.mit.edu

LinkedIn: https://www.linkedin.com/in/snhwang

v2 10/31/2020
v1 10/5/2020



## Introduction

This Unity project converts speech stored as an audio file in WAV format. The conversion is performed using IBM Watson Speech-to-text. The program should be easy to modify to convert other types of audio files that are compatible with Watson speech-to-text.

## Updates
The program still transcribes the entire audio file at once as was previously done. However, it now stores the timestamps for each of the words. After converting speech to text, it then plays the audio file and displays each word of text at the correct time based on the timestamp.



## Implementation

The project was created with Unity version 2019.4.11f1. The version of the IBM SDKs are unity-sdk-4.8.0 (https://github.com/watson-developer-cloud/unity-sdk) and unity-sdk-core-1.2.1 ( https://github.com/IBM/unity-sdk-core/).



## Setting up

1) You will need an IBM cloud account. You can just go to the speech-to-text page and you will have links to create an account (https://www.ibm.com/cloud/watson-speech-to-text).



2) Create a speech-to-text resource



3) Get your speech-to-text API key and the service URL. Cut and paste these into the Unity project in the appropriate fields in the inspector. You will find these in the GameObject called SpeechToText in the hierarchy of the Unity scene. The scene is call SpeechAudioFileToText.



4) If you run the program, in the editor or compile it, you will see 1 input field and 2 buttons. The input field is for the path to your WAV file. It will start with a default path to an example file. You can change it to a file path of your choosing. One button will play the WAV file. The other button will convert it to speech and print the results on the screen. The speech conversion can return multiple results. If the speech has pauses in it, as it does in the example file, each section of speech between pauses will be converted and stored as a different item in an array of results. The program loops through each of the results and prints out each of them.

