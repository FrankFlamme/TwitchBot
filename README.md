# TwitchBot
TwitchBot is a .NET core console application written in C# to connect your own bot to your Twitch channel.

## Features
- Chat commands
- Playing sounds with a command

## Preparation
For using your own bot, you need to configure some properties.

In `appsettings.json` you can configure the name of your bot, the channel, the folder for your sounds and the access token.
```html
{
  "bot_name": "YOUR BOTNAME",
  "bot_access_token": "YOUR TOKEN",
  "channel_name": "CHANNEL",
  "sound_path": "PATH TO YOUR SOUND FILES"
}
```

TwitchBot uses a Sqlite database to store the commands and sounds in a local database. 

