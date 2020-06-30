using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using TwitchBot.Data;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using System.IO;
using NAudio.Wave;
using System.Threading;
using System.Text.RegularExpressions;

namespace TwitchBot
{
    public class TwitchBot
    {
        private readonly ApplicationDbContext _db;
        private readonly TwitchClient _client;
        private readonly string _soundPath;
        private readonly string _botWelcomeMsg;

        public TwitchBot(ApplicationDbContext db)
        {
            _db = db;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _soundPath = config["sound_path"];
            _botWelcomeMsg = config["bot_welcome_msg"];

            ConnectionCredentials credentials = new ConnectionCredentials(config["bot_name"], config["bot_access_token"]);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient wsClient = new WebSocketClient(clientOptions);
            // TODO can be moved to an own Client class
            _client = new TwitchClient(wsClient);
            _client.Initialize(credentials, config["channel_name"]);

            _client.OnLog += Client_OnLog;
            _client.OnJoinedChannel += Client_OnJoinedChannel;
            _client.OnMessageReceived += Client_OnMessageReceived;
            _client.OnConnected += Client_OnConnected;

            _client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine($"Channel {e.Channel} joined.");
            _client.SendMessage(e.Channel, _botWelcomeMsg);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith('!'))
            {
                var command = _db.Command.FirstOrDefault<Command>(c => "!" + c.Name == e.ChatMessage.Message);

                if (command != null)
                {
                    var commandOutput = ConsiderCommandPlaceholders(command, e.ChatMessage);

                    SendMessage(e.ChatMessage.Channel, commandOutput);

                    // Optionally play a sound.
                    if (IsValidFileName(command.SoundFile))
                    {
                        PlaySound(command.SoundFile);
                    }
                }
            }
        }

        private string ConsiderCommandPlaceholders(Command command, ChatMessage chatMessage) => command.Output.Replace("[UserName]", chatMessage.Username);

        private void PlaySound(string fileName)
        {
            using (var audioFile = new AudioFileReader(GetSoundFilePath(fileName)))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();

                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void SendMessage(string channel, string message)
        {
            _client.SendMessage(channel, message);
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private string GetProjectPath() => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private string GetSoundFilePath(string soundFile) => GetProjectPath() + @"\Sounds\" + soundFile;

        // TODO can be moved to an own validation class
        private bool IsValidFileName(string? fileName) => fileName != null && Regex.IsMatch(fileName, @"^[\w\-. ]+$");
    }
}