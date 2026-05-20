using JimmysUnityUtilities;
using LogicWorld.Audio;
using LogicWorld.ClientCode;
using LogicWorld.Rendering.Components;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using EccsLogicWorldAPI.Shared.AccessHelper;
using LogicAPI.Data;
using LogicWorld.Interfaces;
using LogicWorld.Rendering.DisplayConfigurations;
using LogicWorld.SharedCode;

namespace HuntasAudioVisualEnhancements;

public class PanelDisplay : LogicWorld.ClientCode.PanelDisplay {
    private static readonly SoundEffect lightOn = SoundEffectDatabase.GetSoundEffectByTextID("HuntasAudioVisualEnhancements.lightOn");
    private static readonly SoundEffect lightOff = SoundEffectDatabase.GetSoundEffectByTextID("HuntasAudioVisualEnhancements.lightOff");
    
    private float currentR = 0.0f;
    private float currentG = 0.0f;
    private float currentB = 0.0f;
    
    private bool[] lastStates;
    
    private CoilBuzz buzz;
    
    private MethodInfo GetCurrentColor;
    
    protected override void Initialize() {
        base.Initialize();
        //buzz = new CoilBuzz(this);
        
        Type typeGenericDisplay = typeof(GenericDisplay<IPanelDisplayData>);
        GetCurrentColor = Methods.getPrivate(typeGenericDisplay, "<FrameUpdate>g__GetCurrentColor|2_0");
        
        lastStates = new bool[InputCount];
    }
    
    protected override void FrameUpdate() {
        base.FrameUpdate();
        
        //Logger.Info(ModClass.RampOnSpeed.ToString());
        
        if (ModClass.Enabled) {
            float displayIntensity = ModClass.DisplayIntensity;
            
            GpuColor blockColor = (GpuColor)GetCurrentColor.Invoke(this, null);
            float blockR = blockColor.r;
            float blockG = blockColor.g;
            float blockB = blockColor.b;
            
            currentR = calcNewValue(currentR, blockR);
            currentG = calcNewValue(currentG, blockG);
            currentB = calcNewValue(currentB, blockB);
            if (currentR != blockR || currentG != blockG || currentB != blockB) {
                ContinueUpdatingForAnotherFrame();
            }
            
            SetBlockColor(new GpuColor(currentR*displayIntensity, currentG*displayIntensity, currentB*displayIntensity));
        }
        
        if (lastStates.Length != InputCount) {
            lastStates = new bool[InputCount];
            ContinueUpdatingForAnotherFrame();
        }
        
        if (ModClass.SoundEnabled) {
            for (int i = 0; i < InputCount; i++) {
                bool state = GetInputState(i);
                if (state && !lastStates[i]) {
                    SoundPlayer.PlaySoundAt(lightOn, Address);
                } else if (!state && lastStates[i]) {
                    SoundPlayer.PlaySoundAt(lightOff, Address);
                }
                lastStates[i] = state;
            }
        }
    }
    
    protected override void OnComponentDestroyed() {
        base.OnComponentDestroyed();
        //buzz.Stop();
    }
    
    public void RestartBuzz() {
        //buzz.Stop();
        //buzz.Play();
    }
    
    private static float calcNewValue(float current, float setpoint) {
        if (Math.Abs(setpoint - current) < 0.01f) {
            return setpoint;
        }
        
        if (current > setpoint) {
            return Math.Clamp(current + (setpoint - current) * ModClass.RampOffSpeed/100.0f, 0.0f, 1.0f);
        } else {
            return Math.Clamp(current + (setpoint - current) * ModClass.RampOnSpeed/100.0f, 0.0f, 1.0f);
        }
    }
}

public class StandingDisplay : LogicWorld.ClientCode.StandingDisplay {
    /*private static readonly SoundEffect lightOn = SoundEffectDatabase.GetSoundEffectByTextID("HuntasAudioVisualEnhancements.lightOn");
    private static readonly SoundEffect lightOff = SoundEffectDatabase.GetSoundEffectByTextID("HuntasAudioVisualEnhancements.lightOff");
    
    private float currentR = 0.0f;
    private float currentG = 0.0f;
    private float currentB = 0.0f;
    
    private bool lastState = false;
    
    private CoilBuzz buzz;
    
    private MethodInfo GetCurrentColor;
    
    protected override void Initialize() {
        base.Initialize();
        buzz = new CoilBuzz(this);
        
        Type typeGenericDisplay = typeof(GenericDisplay<IPanelDisplayData>);
        GetCurrentColor = Methods.getPrivate(typeGenericDisplay, "<FrameUpdate>g__GetCurrentColor|2_0");
    }
    
    protected override void FrameUpdate() {
        base.FrameUpdate();
        bool state = GetInputState(0);
        
        //Logger.Info(ModClass.RampOnSpeed.ToString());
        
        float displayIntensity = ModClass.DisplayIntensity;
        
        GpuColor blockColor = (GpuColor)GetCurrentColor.Invoke(this, null);
        float blockR = blockColor.r;
        float blockG = blockColor.g;
        float blockB = blockColor.b;
        
        currentR = calcNewValue(currentR, blockR);
        currentG = calcNewValue(currentG, blockG);
        currentB = calcNewValue(currentB, blockB);
        if (currentR != blockR || currentG != blockG || currentB != blockB) {
            ContinueUpdatingForAnotherFrame();
        }
        
        SetBlockColor(new GpuColor(currentR*displayIntensity, currentG*displayIntensity, currentB*displayIntensity));
        
        if (state && !lastState) {
            buzz.Play();
            SoundPlayer.PlaySoundAt(lightOn, Address);
        } else if (!state && lastState) {
            buzz.Stop();
            SoundPlayer.PlaySoundAt(lightOff, Address);
        }
        
        lastState = state;
    }
    
    protected override void OnComponentDestroyed() {
        base.OnComponentDestroyed();
        buzz.Stop();
    }
    
    public void RestartBuzz() {
        buzz.Stop();
        buzz.Play();
    }
    
    private static float calcNewValue(float current, float setpoint) {
        if (Math.Abs(setpoint - current) < 0.005f) {
            return Math.Clamp(setpoint, 0.0f, 1.0f);
        }
        
        if (current > setpoint) {
            return Math.Clamp(current + (setpoint - current) * ModClass.RampOffSpeed/100.0f, 0.0f, 1.0f);
        } else {
            return Math.Clamp(current + (setpoint - current) * ModClass.RampOnSpeed/100.0f, 0.0f, 1.0f);
        }
    }*/
}