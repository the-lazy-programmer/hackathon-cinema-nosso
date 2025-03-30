using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OpcoesScript : MonoBehaviour
{
    //FAZER PLAYERFABS
    public AudioMixer AudioMixer;
    public AudioMixer SFXAudioMixer;
    private void Awake() {
        //Setar os valores pelos playesSttings
    }
    public void setVolume(float volume) {
        AudioMixer.SetFloat("Volume", volume);
        
    }public void setSFXVolume(float volume) {
        SFXAudioMixer.SetFloat("SFXVolume" , volume);
      
    }
    //Sound Master
    //SFX
    //fullScreen (Se sobrar tempo)
    //Mixer
}
