# TwitchBot
TwitchBot is a .NET core console application written in C# to connect your own bot to your Twitch channel.

## Features
- Chat commands
- Playing sounds with a command

## Preparation
For using your own bot, you need to configure some properties.

In `appsettings.json` you can configure the name of your bot, the channel, the folder for your sounds, the access token and the welcome message, if your bot is joining the channel.
```html
{
  "bot_name": "YOUR BOTNAME",
  "bot_access_token": "YOUR TOKEN",
  "channel_name": "CHANNEL",
  "sound_path": "PATH TO YOUR SOUND FILES",
  "bot_welcome_msg": "Welcome, I'm your bot."
}
```

TwitchBot uses a Sqlite database to store the commands and sounds in a local database. 
The `TwitchBotDB.db` database file will be created automatically when the application starts.

To prepare the database with your own commands, add them to the Entity in the `ApplicationDbContext.cs` file.

We have the following database fields:
- `ID` Id to identify the record
- `CommandType` Enum to classify the command (text/sound)
- `CommandName` Name of the command
- `CommandOutput` Text output for the chat command
- `SoundFile` Name of the sound file for sound commands

Example for implementation in `ApplicationDbContext.cs`:
```html
builder.Entity<Command>()
       .HasData(
          // Insert your Commands here.
          new Command() { Id = 1, CommandType = CommandType.Text, CommandName = "!project", CommandOutput = "We're working today... on me! The Twitchbot. :-)", SoundFile = ""}
        );
```

You can also use `[UserName]`in your `CommandOutput` field to show the user name in the command:
```html
new Command() { Id = 4, Type = CommandType.Text, Name = "attention", Output = "[UserName] need to get attention by ...", SoundFile = "" }
```
