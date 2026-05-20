using LICC;
using UnityEngine;

namespace HuntasAudioVisualEnhancements.Lighting
{
	public class LightingCommands {
		[Command(name: "LightAmbient", Description = "Sets the color of the ambient light, with equal distribution over RGB channel.")]
		public static void lightAmbient(float intensity)
		{
			LightingControl.LightAmbient = intensity;
		}
		
		[Command(name: "LightSun", Description = "Sets the color of the sun light, with equal distribution over RGB channel.")]
		public static void lightSun(float intensity)
		{
			LightingControl.LightSun = intensity;
		}
	}
}
