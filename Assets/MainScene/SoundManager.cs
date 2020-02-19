using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : MonoBehaviour { 

  private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>(); 
  private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

  public void Add(int chain, int x, int y, int index, int loopNumber, string soundName) {
    string key = chain.ToString() + x.ToString() + y.ToString() + index.ToString();
    
    audioSources.Add(key, gameObject.AddComponent<AudioSource>() as AudioSource);
    audioSources[key].Stop();

    StartCoroutine(GetAudioClip(key, soundName, loopNumber));
  } 

  public void Play(int chain, int x, int y, int index) 
  { 
    string key = chain.ToString() + x.ToString() + y.ToString() + index.ToString();

    if (audioSources.ContainsKey(key) == true && audioClips.ContainsKey(key) == true)
        audioSources[key].Play();
  }

  IEnumerator GetAudioClip(string key, string soundName, int loopNumber) {
     WWW www = new WWW("file:///" + soundName);

    while (www.progress < 0.1)
    {
      yield return new WaitForSeconds(0.01f);
    }
    audioClips.Add(key, www.GetAudioClip(false));
    audioSources[key].clip = audioClips[key];
    audioSources[key].loop = false;
    audioSources[key].playOnAwake = false; 
  }
} 
