using Exiled.API.Interfaces;

namespace SCPTeleporter.Configs;

internal class Config : IConfig
{
    public bool IsEnabled { get; set; }
    public bool Debug { get; set; }
}
