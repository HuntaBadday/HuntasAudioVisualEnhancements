using System.Reflection;
using EccsLogicWorldAPI.Shared.AccessHelper;
using JimmysUnityUtilities;
using LICC;
using LogicAPI.Data;
using LogicWorld.SharedCode;

namespace LaserWireAndLightControl.Client.Adapters
{
	public static class ConductorColors
	{
		public static readonly Color24 originalConductorColorOn = Colors.CircuitOn24;
		public static readonly Color24 originalConductorColorOff = Colors.CircuitOff24;
		
		private static FieldInfo fieldConductorOn24;
		private static FieldInfo fieldConductorOff24;
		private static FieldInfo fieldConductorOn;
		private static FieldInfo fieldConductorOff;
		
		public static void init()
		{
			LConsole.WriteLine("INIT!!!");
			fieldConductorOn24 = Fields.getPublicStatic(typeof(Colors), nameof(Colors.CircuitOn24));
			fieldConductorOff24 = Fields.getPublicStatic(typeof(Colors), nameof(Colors.CircuitOff24));
			fieldConductorOn = Fields.getPublicStatic(typeof(Colors), nameof(Colors.CircuitOn));
			fieldConductorOff = Fields.getPublicStatic(typeof(Colors), nameof(Colors.CircuitOff));
		}
		
		public static GpuColor setConductorColorOff(Color24 color, float glowness = 1.0f)
		{
			fieldConductorOff24.SetValue(null, color);
			var gpuColor = applyGlowness(color, glowness);
			fieldConductorOff.SetValue(null, gpuColor);
			return gpuColor;
		}
		
		public static GpuColor setConductorColorOn(Color24 color, float glowness = 1.0f)
		{
			fieldConductorOn24.SetValue(null, color);
			var gpuColor = applyGlowness(color, glowness);
			fieldConductorOn.SetValue(null, gpuColor);
			return gpuColor;
		}
		
		private static GpuColor applyGlowness(Color24 color, float glowness)
			=> new GpuColor(
				color.r / (float) byte.MaxValue * glowness,
				color.g / (float) byte.MaxValue * glowness,
				color.b / (float) byte.MaxValue * glowness
			);
	}
}
