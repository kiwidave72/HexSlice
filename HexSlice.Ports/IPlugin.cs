namespace HexSlice.Ports
{
    // Port for plugin functionality
    public interface IPlugin
    {
        // Unique identifier for the plugin
        string Id { get; }
        
        // Human-readable name of the plugin
        string Name { get; }
        
        // Plugin version
        string Version { get; }
        
        // Initialize the plugin
        void Initialize();
        
        // Shutdown the plugin
        void Shutdown();
    }
}
