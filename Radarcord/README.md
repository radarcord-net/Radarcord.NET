# Radarcord.NET

The best way to interact with the Radarcord API with DSharpPlus and .NET!

## Note

This package is used with DSharpPlus, not Discord.Net, if you want an integration with that, I'll be publishing one soon.

## Compatibility

This package only supports .NET 7 at this point in time.

Please note that this package is only compatible with the specified version, and other versions of .NET may not work/have issues.

## Features

- Easy to use, less pressure on working with HTTP.
- Autoposting support, keep getting your stats!
- Webhook support soon!
- Constantly maintained to keep up with the thriving .NET ecosystem!

## Installation

Installation is super easy, just run the following command:

### Package Manager Console (Visual Studio)

```powershell
Install-Package Radarcord.NET
```

### .NET CLI

```bash
dotnet add package Radarcord.NET
```

## Basic Usage

```csharp
using DSharpPlus;
using DSharpPlus.EventArgs;
using Radarcord;

namespace MyBot
{
    public class Program
    {
        private RadarcordClient? radar;

        public static async Task Main()
        {
            var program = new Program();
            await program.StartBot();
        }

        private async Task StartBot()
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = "YOUR_TOKEN_HERE",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });
            radar = new RadarcordClient(discord, "YOUR_RADARCORD_API_TOKEN_HERE");

            discord.Ready += OnClientReady;

            await discord.ConnectAsync();

            // Keep the program running until the user presses the keyboard interrupt keybind.
            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine("The client is ready!");

            if (radar is null) return Task.CompletedTask;

            // Post bot stats to Radarcord
            radar.PostStatsAsync();

            return Task.CompletedTask;
        }
    }
}
```

## Documentation

Documentation will be coming pretty soon.

## Copyright

Copyright (c) 2023 - present Yoshiboi18303.

**Licensed under the MIT License**
