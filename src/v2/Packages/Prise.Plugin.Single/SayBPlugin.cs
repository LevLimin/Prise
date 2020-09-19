﻿using System;
using System.Threading.Tasks;
using Prise.Console.Contract;

namespace Prise.Plugin.Single
{
    [Plugin(PluginType = typeof(IPlugin))]
    public class SayBPlugin : IPlugin
    {
        public async Task<PluginObject> GetData(PluginObject input)
        {
            // await Task.Delay(4000);
            return new PluginObject
            {
                Number = 4000,
                Text = input.Text + "B"
            };
        }
    }
}