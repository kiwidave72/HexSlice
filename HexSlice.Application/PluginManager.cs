using HexSlice.Ports;

namespace HexSlice.Application
{
    // Manages plugins for the application
    public class PluginManager
    {
        private readonly Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();

        // Register a plugin
        public void RegisterPlugin(IPlugin plugin)
        {
            if (_plugins.ContainsKey(plugin.Id))
            {
                throw new InvalidOperationException($"A plugin with ID '{plugin.Id}' is already registered.");
            }

            _plugins[plugin.Id] = plugin;
            plugin.Initialize();
        }

        // Unregister a plugin
        public void UnregisterPlugin(string pluginId)
        {
            if (_plugins.TryGetValue(pluginId, out var plugin))
            {
                plugin.Shutdown();
                _plugins.Remove(pluginId);
            }
        }

        // Get a plugin by ID
        public IPlugin? GetPlugin(string pluginId)
        {
            return _plugins.TryGetValue(pluginId, out var plugin) ? plugin : null;
        }

        // Get all registered plugins
        public IEnumerable<IPlugin> GetAllPlugins()
        {
            return _plugins.Values;
        }

        // Initialize all plugins
        public void InitializeAllPlugins()
        {
            foreach (var plugin in _plugins.Values)
            {
                plugin.Initialize();
            }
        }

        // Shutdown all plugins
        public void ShutdownAllPlugins()
        {
            foreach (var plugin in _plugins.Values)
            {
                plugin.Shutdown();
            }
        }
    }
}
