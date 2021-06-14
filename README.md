# SpeechToText-IBM-NonStreaming
## Introduction

This Unity project converts speech stored as an audio file in WAV format. The conversion is performed using IBM Watson Speech-to-text. The program should be easy to modify to convert other types of audio files that are compatible with Watson speech-to-text.

## Updates
The program still transcribes the entire audio file at once as was previously done. However, it now stores the timestamps for each of the words. After converting speech to text, it then plays the audio file and displays each word of text at the correct time based on the timestamp.


## Setting up

1) You will need an IBM cloud account. You can just go to the speech-to-text page and you will have links to create an account (https://www.ibm.com/cloud/watson-speech-to-text).



2) Create a speech-to-text resource and get the url and API key.



3) Get your speech-to-text API key and the service URL. Copy and paste these into the Unity project in the appropriate fields in the inspector in STT Manager gameobject. 



4) If you run the program, in the editor or compile it, you will see 1 input field. The input field is for the path to your WAV file. It will start with a default path to an example file. You can change it to a file path of your choosing. The speech conversion can return multiple results. If the speech has pauses in it, as it does in the example file, each section of speech between pauses will be converted and stored as a different item in an array of results. The program loops through each of the results and prints out each of them.

