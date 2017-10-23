using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Services.Geolocator
{
    public interface IGeolocatorService
    {

        //
        // Summary:
        //     Gets if the plugin is supported on the current platform.
        bool GetIsSupported();

        //
        // Summary:
        //     Current plugin implementation to use
        IGeolocator GetCurrent();
    }
}
