using System.Reflection;
using HarmonyLib;
using LogicAPI.Client;
using System;
using JECS.Abstractions;
using LogicAPI;
using LogicAPI.Data;
using LogicLog;
using LogicWorld;
using LogicWorld.Audio;
using LogicWorld.ClientCode;
using LogicWorld.ClientWorldStuff;
using LogicWorld.Interfaces;
using LogicWorld.SharedCode.Components;
using LogicSettings;
using LogicUI.MenuTypes;
using LogicWorld.UI.SharedStuff;

namespace HuntasAudioVisualEnhancements;

public class ModClass : ClientMod {
    public static ILogicLogger logger = null;
    protected override void Initialize() {
        logger = Logger;
        
        var harmony = new Harmony("HuntasAudioVisualEnhancements");
        harmony.PatchAll();
        CoilBuzz.InitSound();
        
        InitLaserWireAndLightControl();
        
        Logger.Info("HuntasAudioVisualEnhancements - Loaded");
    }
    
    private static void InitLaserWireAndLightControl() {
        LaserWireAndLightControl.Client.Adapters.ConductorColors.init();
        LaserWireAndLightControl.Client.Adapters.ThumbnailUpdater.init();
        
        ToggleableSingletonMenu<SettingsMenuPage>.OnMenuHidden += LaserWireAndLightControl.client.LaserWire.applySettings;
        ToggleableSingletonMenu<SettingsMenuPage>.OnMenuHidden += HuntasAudioVisualEnhancements.Lighting.LightingControl.applySettings;
        
        // CircuitColor.setCircuitColor(new Color24(255, 140, 10)); // Orange circuits
        // CircuitColor.setCircuitColor(new Color24(10, 150, 10)); // Green circuits (Clashes with wire outlines)
    }
    
    [Setting_SliderFloat("HuntasAudioVisualEnhancements.DisplayEffect.DisplayIntensity")]
    public static float DisplayIntensity {
        get => _displayIntensity;
        set {
            _displayIntensity = value;
            updateAllIntensities();
        }
    }
    [Setting_SliderFloat("HuntasAudioVisualEnhancements.DisplayEffect.RampOnSpeed")]
    public static float RampOnSpeed {
        get => _rampOnSpeed;
        set => _rampOnSpeed = value;
    }
    [Setting_SliderFloat("HuntasAudioVisualEnhancements.DisplayEffect.RampOffSpeed")]
    public static float RampOffSpeed {
        get => _rampOffSpeed;
        set => _rampOffSpeed = value;
    }
    
    /*
    [Setting_SliderFloat("HuntasAudioVisualEnhancements.Volume.BuzzVolume")]
    public static float BuzzVolume {
        get => _buzzVolume;
        set {
            _buzzVolume = value;
            updateAllBuzzVolumes();
        }
    }
    */
    
    private static float _displayIntensity = 2;
    private static float _rampOnSpeed = 20;
    private static float _rampOffSpeed = 15;
    private static float _buzzVolume = 5;
    
    private static void updateAllIntensities(){
        var mainWorld = Instances.MainWorld;
        if (mainWorld == null)
        {
            return;
        }
        var display = mainWorld.ComponentTypes.GetComponentType("MHG.StandingDisplay");
        var panel = mainWorld.ComponentTypes.GetComponentType("MHG.PanelDisplay");
        foreach(var kvp in mainWorld.Data.AllComponents)
        {
            var (address, data) = kvp;
            if (data.Data.Type == display || data.Data.Type == panel)
            {
                mainWorld.Renderer.Entities.GetClientCode(address).QueueFrameUpdate();
            }
        }
    }
    
    private static void updateAllBuzzVolumes(){
        var mainWorld = Instances.MainWorld;
        if (mainWorld == null)
        {
            return;
        }
        var display = mainWorld.ComponentTypes.GetComponentType("MHG.StandingDisplay");
        var panel = mainWorld.ComponentTypes.GetComponentType("MHG.PanelDisplay");
        foreach(var kvp in mainWorld.Data.AllComponents)
        {
            var (address, data) = kvp;
            if (data.Data.Type == display || data.Data.Type == panel) {
                //((PanelDisplay)mainWorld.Renderer.Entities.GetClientCode(address)).RestartBuzz();
                //((StandingDisplay)mainWorld.Renderer.Entities.GetClientCode(address)).RestartBuzz();
            }
        }
    }
}



[HarmonyPatch(typeof(SceneAndNetworkManager))]
[HarmonyPatch("HandleWorldInitializationPacket")]
public class worldLoadPatch() {
    public static void Postfix() {
        ModClass.logger.Info("Patch Attempt");
        try {
            typeof(ComponentInfo).GetProperty("ClientCodeType").SetValue(GetCompInfo("MHG.Relay"), typeof(Relay));
            typeof(ComponentInfo).GetProperty("ClientCodeType").SetValue(GetCompInfo("MHG.PanelDisplay"), typeof(PanelDisplay));
            typeof(ComponentInfo).GetProperty("ClientCodeType").SetValue(GetCompInfo("MHG.StandingDisplay"), typeof(StandingDisplay));
            ModClass.logger.Info("Patch Possible Success");
        } catch (Exception e) {
            ModClass.logger.Info("Patch Fail");
            ModClass.logger.Error(e.ToString());
        }
    }
    
    private static ComponentInfo GetCompInfo(string textID) {
        ComponentType compType = Instances.MainWorld.ComponentTypes.GetComponentType(textID);
        ComponentInfo compInfo = Instances.MainWorld.ComponentTypes.GetComponentInfo(compType);
        return compInfo;
    }
}