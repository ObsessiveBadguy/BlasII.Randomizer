using Il2CppTGK.Game;
using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    public class ItemHandler
    {
        private Dictionary<string, string> _mappedItems = new();

        private readonly ItemShuffler _shuffler = new();

        public Dictionary<string, string> MappedItems
        {
            get => _mappedItems;
            set => _mappedItems = value ?? new Dictionary<string, string>();
        }

        public Item GetItemAtLocation(string locationId)
        {
            if (_mappedItems.TryGetValue(locationId, out string item))
            {
                return Main.Randomizer.Data.GetItem(item);
            }
            else
            {
                Main.Randomizer.LogError(locationId + " does not have a mapped item!");
                return null;
            }
        }

        public void GiveItemAtLocation(string locationId)
        {
            Main.Randomizer.LogWarning("Giving item at location: " +  locationId);
            Item item = GetItemAtLocation(locationId);

            if (item == null)
                return;

            // Check for and set location id flag

            item.GiveReward();
            DisplayItem(item);
        }

        public void DisplayItem(Item item)
        {
            CoreCache.UINavigationHelper.ShowItemPopup("Obtained", item.name, item.Image);
        }

        public bool IsLocationRandomized(string locationId)
        {
            return _mappedItems.ContainsKey(locationId);
        }

        public void FakeShuffle(uint seed, TempConfig config)
        {
            if (_shuffler.Shuffle(seed, config, _mappedItems))
            {
                Main.Randomizer.Log($"Shuffled {_mappedItems.Count} items!");
            }
            else
            {
                Main.Randomizer.LogError("Failed to shuffle items!");
                _mappedItems.Clear();
            }
        }
    }
}