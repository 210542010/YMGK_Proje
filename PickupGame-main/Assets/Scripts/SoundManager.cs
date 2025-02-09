using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{ 
    void Start()
    {
        Dictionary<string, AudioClip> sounds = GetSounds();

        string soundName = "1";
        if (sounds.ContainsKey(soundName))
        {
            AudioClip sound = sounds[soundName];
            GetComponent<AudioSource>().PlayOneShot(sound);
        }
    }

    Dictionary<string, AudioClip> GetSounds()
    {
        Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

        AudioClip[] soundFiles = Resources.LoadAll<AudioClip>("sounds");

        string prefix = "sepet_";
        string postfix = "_var";

        foreach (AudioClip soundFile in soundFiles)
        {
            string fileName = soundFile.name;

            string newFileName = prefix + fileName + postfix;

            sounds.Add(fileName, soundFile);
        }

        return sounds;
    }
}
