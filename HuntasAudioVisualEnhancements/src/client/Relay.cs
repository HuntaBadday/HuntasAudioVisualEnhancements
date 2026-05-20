using JimmysUnityUtilities;
using LogicWorld.Audio;
using LogicWorld.ClientCode;
using LogicWorld.Rendering.Components;
using UnityEngine;
using System.IO;
using System;

namespace HuntasAudioVisualEnhancements;

public class Relay : ComponentClientCode {
    private static readonly SoundEffect lightOn = SoundEffectDatabase.GetSoundEffectByTextID("HuntasAudioVisualEnhancements.lightOn");
    private static readonly SoundEffect lightOff = SoundEffectDatabase.GetSoundEffectByTextID("HuntasAudioVisualEnhancements.lightOff");
    
    private bool lastState = false;
    
    protected override void FrameUpdate() {
        bool state = GetInputState(0);
        if (state && !lastState) {
            //PlaySound();
            //SoundPlayer.PlaySoundAt(lightOn, Address);
        } else if (!state && lastState) {
            //audioSource.Stop();
            //SoundPlayer.PlaySoundAt(lightOff, Address);
        }
        
        lastState = state;
    }
    
    protected override void OnComponentDestroyed() {
        
    }
    
    protected override void Initialize() {
        
    }
}