using DailyRoutines.Managers;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace WhatIHaveInFFXIVCN10thAnniv;

public class Service
{
    public static void Init(DalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;
        pluginInterface.Create<Service>();

        Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Config.Init();

        WindowManager = new();
        WindowManager.Init();
    }

    public static void Uninit()
    {
        WindowManager.Uninit();
        Config.Uninit();
    }
    
    [PluginService] public static IDataManager           Data            { get; private set; } = null!;
    [PluginService] public static ITextureProvider       Texture         { get; private set; } = null!;
    public static                 DalamudPluginInterface PluginInterface { get; private set; } = null!;
    public static                 Configuration          Config          { get; private set; } = null!;
    public static                 WindowManager?         WindowManager   { get; private set; }
}
