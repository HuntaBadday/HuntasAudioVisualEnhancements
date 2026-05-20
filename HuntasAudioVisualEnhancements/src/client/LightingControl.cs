using JimmysUnityUtilities;
using LaserWireAndLightControl.Client.Adapters;
using LICC;
using LogicAPI.Data;
using LogicSettings;
using UnityEngine;

namespace HuntasAudioVisualEnhancements.Lighting
{
	public static class LightingControl
	{
		[Setting_Toggle("HuntasAudioVisualEnhancements.Lighting.Enable")]
		public static bool Enabled {
			get => _enabled;
			set {
				_enabled = value;
				updateEnabled = true;
			}
		}
		
		[Setting_SliderFloat("HuntasAudioVisualEnhancements.Lighting.LightSun")]
		public static float LightSun {
			get => _lightSun;
			set {
				_lightSun = value;
				updateSun = true;
			}
		}
		
		
		[Setting_SliderFloat("HuntasAudioVisualEnhancements.Lighting.LightAmbient")]
		public static float LightAmbient {
			get => _lightAmbient;
			set {
				_lightAmbient = value;
				updateAmbient = true;
			}
		}
		
		private static bool _enabled = false;
		private static float _lightSun = 1.0f;
		private static float _lightAmbient = 1.0f;
		
		private static bool updateEnabled;
		private static bool updateSun;
		private static bool updateAmbient;
		
		public static void applySettings() {
			//LConsole.WriteLine($"Applying: enable:{updateEnabled} on:{updateOn} off:{updateOff}");
			if (!(updateEnabled || updateSun || updateAmbient)) {
				return; //No changes occurred, nothing to update.
			}
			
			if (Enabled) {
				RenderSettings.sun.color = new Color(LightSun, LightSun, LightSun);
				RenderSettings.ambientSkyColor = new Color(LightAmbient, LightAmbient, LightAmbient);
			} else {
				RenderSettings.sun.color = new Color(1.0f, 1.0f, 1.0f);
				RenderSettings.ambientSkyColor = new Color(1.0f, 1.0f, 1.0f);
			}
			
			//Done, clear flags:
			updateEnabled = false;
			updateSun = false;
			updateAmbient = false;
		}
	}
}
