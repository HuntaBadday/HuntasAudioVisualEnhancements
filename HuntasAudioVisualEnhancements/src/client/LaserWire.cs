using JimmysUnityUtilities;
using LaserWireAndLightControl.Client.Adapters;
using LICC;
using LogicAPI.Data;
using LogicSettings;

namespace LaserWireAndLightControl.client
{
	public static class LaserWire
	{
		private static bool _enabled;
		[Setting_Toggle("HuntasAudioVisualEnhancements.LaserWire.Enable")]
		public static bool enabled
		{
			get => _enabled;
			set
			{
				_enabled = value;
				updateEnabled = true;
			}
		}
		
		private static Color24 _colorOn = ConductorColors.originalConductorColorOn;
		[Setting_ColorPicker("HuntasAudioVisualEnhancements.LaserWire.On.Color")]
		public static Color24 colorOn
		{
			get => _colorOn;
			set
			{
				_colorOn = value;
				updateOn = true;
			}
		}
		
		private static float _glownessOn = 1f;
		[Setting_SliderFloat("HuntasAudioVisualEnhancements.LaserWire.On.Glowness")]
		public static float glownessOn
		{
			get => _glownessOn;
			set
			{
				_glownessOn = value;
				updateOn = true;
			}
		}
		
		private static Color24 _colorOff = ConductorColors.originalConductorColorOff;
		[Setting_ColorPicker("HuntasAudioVisualEnhancements.LaserWire.Off.Color")]
		public static Color24 colorOff
		{
			get => _colorOff;
			set
			{
				_colorOff = value;
				updateOff = true;
			}
		}
		
		private static float _glownessOff = 1f;
		[Setting_SliderFloat("HuntasAudioVisualEnhancements.LaserWire.Off.Glowness")]
		public static float glownessOff
		{
			get => _glownessOff;
			set
			{
				_glownessOff = value;
				updateOff = true;
			}
		}
		
		// Update logic:
		
		private static bool updateEnabled;
		private static bool updateOn;
		private static bool updateOff;
		
		public static void applySettings()
		{
			LConsole.WriteLine($"Applying: enable:{updateEnabled} on:{updateOn} off:{updateOff}");
			if (!(updateEnabled || updateOn || updateOff))
			{
				return; //No changes occurred, nothing to update.
			}
			
			//TODO: Optimize what to update. For now update everything:
			GpuColor gpuColorOn;
			GpuColor gpuColorOff;
			if (!enabled)
			{
				// LConsole.WriteLine("RESETTING!");
				gpuColorOn = ConductorColors.setConductorColorOn(ConductorColors.originalConductorColorOn);
				gpuColorOff = ConductorColors.setConductorColorOff(ConductorColors.originalConductorColorOff);
			}
			else
			{
				// LConsole.WriteLine("APPLYING!");
				gpuColorOn = ConductorColors.setConductorColorOn(colorOn, glownessOn);
				gpuColorOff = ConductorColors.setConductorColorOff(colorOff, glownessOff);
			}
			//ThumbnailUpdater.triggerThumbnailUpdateForStartOnComponents();
			WorldConductorUpdater.updateWorldConductors(gpuColorOn, gpuColorOff);
			
			//Done, clear flags:
			updateEnabled = false;
			updateOn = false;
			updateOff = false;
		}
	}
}
