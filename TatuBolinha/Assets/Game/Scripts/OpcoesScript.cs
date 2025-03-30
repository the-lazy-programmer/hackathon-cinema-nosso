using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class OpcoesScript : MonoBehaviour
{
    //FAZER PLAYERFABS
    public AudioMixer AudioMixer;
    public AudioMixer SFXAudioMixer;

    private float OVolume; 
    private void Awake() {
        //Tentar colocar e pegar no ggetgameObject
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            setVolume(PlayerPrefs.GetFloat("musicVolume"));
        }
    }
    private void Update() {
        PlayerPrefs.SetFloat("musicVolume" , OVolume);
    }
    public void setVolume(float volume) {
        AudioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("musicVolume" , volume);
        OVolume = volume;
        
        
    }public void setSFXVolume(float volume) {
        SFXAudioMixer.SetFloat("SFXVolume" , volume);
      
    }
    //Sound Master
    //SFX
    //fullScreen (Se sobrar tempo)
    //Mixer
}
