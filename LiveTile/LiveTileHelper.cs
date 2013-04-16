using System.Linq;
using Microsoft.Phone.Shell;

namespace Ayls.WP8Toolkit.LiveTile
{
    public class LiveTileHelper
    {
        public static void UpdatePrimaryTileBadge(int count)
        {
            var primaryTileData = new FlipTileData();
            primaryTileData.Count = count;
            var primaryTile = ShellTile.ActiveTiles.First();
            primaryTile.Update(primaryTileData);
        }
    }
}
