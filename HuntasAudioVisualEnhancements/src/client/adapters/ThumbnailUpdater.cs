using System.Collections.Generic;
using EccsLogicWorldAPI.Shared.AccessHelper;
using LogicAPI.Data;
using LogicWorld.Interfaces;
using LogicWorld.SharedCode.Components;
using LogicWorld.UI.Thumbnails;

namespace LaserWireAndLightControl.Client.Adapters
{
	public static class ThumbnailUpdater
	{
		public static void init()
		{
		}
		
		public static void triggerThumbnailUpdateForStartOnComponents()
		{
			Methods.getPrivateStatic(typeof(ItemThumbnails), "RegenerateCacheCommand").Invoke(null, null);
		}
		
		public static void a()
		{
			// Update component thumbnails:
			var rendererInstance = Types.checkType<ItemThumbnails>(Fields.getNonNull(Fields.getPrivateStatic(typeof(ItemThumbnails), "Instance")));
			var method = Methods.getPrivate(typeof(ItemThumbnails), "QueueRender");
			
			var affectedComponentTypes = new HashSet<string>();
			foreach(var type in ComponentRegistry.GetAllTextIDs())
			{
				var componentInfo = ComponentRegistry.GetComponentInfoByTextID(type);
				
				if (!componentInfo.PrefabIsDynamic)
				{
					Prefab staticPrefab = componentInfo.StaticPrefab;
					foreach(var outputs in staticPrefab.Outputs)
					{
						if(outputs.StartOn)
						{
							affectedComponentTypes.Add(type);
							
							var hotbarItem = new BasicHotbarItemData(type);
							var texture = ItemThumbnails.GetThumbnailFor(hotbarItem);
							method.Invoke(rendererInstance, new object[] {hotbarItem, texture});
							
							goto end_of_loop;
						}
					}
				}
				end_of_loop: {}
			}
			
			if (Instances.MainWorld == null)
			{
				return; //No point in updating any hotbar right now - not joined.
			}
			
			// Update thumbnails in hotbar...
			var hotbar = Instances.Hotbar;
			for(int i = 0; i < Instances.Hotbar.HotbarItemsCount; i++)
			{
				var hotbarItem = hotbar.HotbarItemInfo(i) as DetailedHotbarItemData;
				if(hotbarItem == null)
				{
					continue; // Wrong hotbar item type.
				}
				if(!affectedComponentTypes.Contains(hotbarItem.TextID))
				{
					continue; // No startOn peg.
				}
				// Also re-render that:
				var texture = ItemThumbnails.GetThumbnailFor(hotbarItem);
				method.Invoke(rendererInstance, new object[] {hotbarItem, texture});
			}
		}
	}
}
