using LogicWorld.Audio;
using LogicWorld.Rendering.Components;
using UnityEngine;
using System.IO;
using System;

namespace HuntasAudioVisualEnhancements;

public class CoilBuzz {
    private GameObject gameObject;
    private AudioSource audioSource;
    private static AudioClip audioClip;
    
    private static readonly SoundEffect buzz = SoundEffectDatabase.GetSoundEffectByTextID("HuntasAudioVisualEnhancements.buzz");
    
    public CoilBuzz(ComponentClientCode component) {
        gameObject = new GameObject();
        gameObject.transform.position = component.Component.WorldPosition;
        audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.loop = true;
        audioSource.outputAudioMixerGroup = buzz.Output;
        audioSource.spatialBlend = 1.0f;
        audioSource.clip = audioClip;
    }
    
    public static void InitSound() {
        byte[] soundData = File.ReadAllBytes("GameData/HuntasAudioVisualEnhancements/pcm/buzz.raw");
        float[] pcmData = new float[soundData.Length / 4];
        for (int i = 0; i < soundData.Length / 4; i++) {
            pcmData[i] = BitConverter.ToSingle(soundData, i * 4);
        }
        
        audioClip = AudioClip.Create("", pcmData.Length, 1, 48000, false);
        audioClip.SetData(pcmData, 0);
    }
    
    public void Play() {
        if (ModClass.BuzzVolume <= 0.1f) {
            return;
        }
        audioSource.volume = ModClass.BuzzVolume/100.0f;
        audioSource.Play();
    }
    
    public void Stop() {
        audioSource.Stop();
    }
}