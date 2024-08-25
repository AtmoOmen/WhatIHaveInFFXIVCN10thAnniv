using System;
using Dalamud.Configuration;

namespace WhatIHaveInFFXIVCN10thAnniv;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;


    public void Init()
    {
    }

    public void Save()
    {
        Service.PluginInterface.SavePluginConfig(this);
    }

    public void Uninit()
    {

    }
}
