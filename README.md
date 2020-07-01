# TwitchBot
TwitchBot is a .NET core console application written in C# to connect your own bot to your Twitch channel.

## Features
- Chat commands
- Playing sounds with a command

## Preparation
For using your own bot, you need to configure some properties.

### Create a Twitch account and an access token
1. Register a new [Twitch](https://www.twitch.tv) account
2. Log-in to your newly created Twitch account
3. Open https://twitchtokengenerator.com, select the scopes you want and click on `Generate Token!`

### Update the settings
In `appsettings.json` you can configure the name of your bot, the channel, the folder for your sounds, the access token and the welcome message, if your bot is joining the channel.
```json
{
  "bot_name": "YOUR BOTNAME",
  "bot_access_token": "YOUR TOKEN",
  "channel_name": "CHANNEL",
  "sound_path": "PATH TO YOUR SOUND FILES",
  "bot_welcome_msg": "Welcome, I'm your bot."
}
```

TwitchBot uses a SQLite database to store the commands and sounds in a local database. 
The `TwitchBotDB.db` database file will be created automatically when the application starts.

To prepare the database with your own commands, add them to the entity in the `ApplicationDbContext.cs` file.

A command consists of the fields:
- `ID` Id to identify the record
- `Type` Enum to classify the command (text/sound)
- `Name` Name of the command
- `Output` Text output for the chat command
- `SoundFile` Name of the sound file for sound commands

Example for implementation in `ApplicationDbContext.cs`:
```csharp
builder.Entity<Command>()
       .HasData(
          // Insert your Commands here.
          new Command() { Id = 1, Type = CommandType.Text, Name = "project", Output = "We're working today... on me! The Twitchbot. :-)", SoundFile = ""}
        );
```

You can also use the placeholder `[UserName]` in the `Output` field to show the user name in the command:
```csharp
new Command() { Id = 4, Type = CommandType.Text, Name = "attention", Output = "[UserName] need to get attention by ...", SoundFile = "" }
```
