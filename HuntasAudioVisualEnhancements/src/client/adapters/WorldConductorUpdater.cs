using LogicAPI.Data;
using LogicWorld.Interfaces;

namespace LaserWireAndLightControl.Client.Adapters
{
	public static class WorldConductorUpdater
	{
		public static void updateWorldConductors(GpuColor? colorOn = null, GpuColor? colorOff = null)
		{
			var mainWorld = Instances.MainWorld;
			if (mainWorld == null)
			{
				return; //Nothing to do
			}
			var circuitStates = mainWorld.CircuitStates;
			var entityTracker = mainWorld.Renderer.Entities;
			
			//Wires:
			foreach(var (address, wire) in mainWorld.Data.AllWires)
			{
				var state = circuitStates.GetStateAt(wire.StateID);
				if(state && colorOn.HasValue)
				{
					var entity = entityTracker.GetWireEntity(address);
					entity.SetColor(colorOn.Value);
				}
				else if(!state && colorOff.HasValue)
				{
					var entity = entityTracker.GetWireEntity(address);
					entity.SetColor(colorOff.Value);
				}
			}
			
			//Components:
			foreach(var (address, componentDataManager) in mainWorld.Data.AllComponents)
			{
				int index = 0;
				foreach(var inputInfo in componentDataManager.Data.InputInfos)
				{
					var state = circuitStates.GetStateAt(inputInfo.StateID);
					if(state && colorOn.HasValue)
					{
						var entity = entityTracker.GetPegEntity(new PegAddress(address, index, PegType.Input));
						entity.SetColor(colorOn.Value);
					}
					else if(!state && colorOff.HasValue)
					{
						var entity = entityTracker.GetPegEntity(new PegAddress(address, index, PegType.Input));
						entity.SetColor(colorOff.Value);
					}
					index += 1;
				}
				index = 0;
				foreach(var outputInfo in componentDataManager.Data.OutputInfos)
				{
					var state = circuitStates.GetStateAt(outputInfo.StateID);
					if(state && colorOn.HasValue)
					{
						var entity = entityTracker.GetPegEntity(new PegAddress(address, index, PegType.Output));
						entity.SetColor(colorOn.Value);
					}
					else if(!state && colorOff.HasValue)
					{
						var entity = entityTracker.GetPegEntity(new PegAddress(address, index, PegType.Output));
						entity.SetColor(colorOff.Value);
					}
					index += 1;
				}
			}
		}
	}
}
