namespace Ayls.WP8Toolkit.LiveTile
{
    public interface ILiveTileSettings
    {
        string LiveTileAgentName { get; }
        string LiveTileAgentDescription { get; }
        bool IsLiveTileEnabled { get; set; }
    }
}
