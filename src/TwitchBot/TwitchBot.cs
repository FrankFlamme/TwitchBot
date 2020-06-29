using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.FileExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TwitchBot.Data;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using WMPLib;
using System.IO;

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
                var outputCommand = _db.Command.FirstOrDefault(c => "!" + c.Name == e.ChatMessage.Message)?.Output;                
                var isSound = _db.Command.FirstOrDefault(st => "!" + st.Name == e.ChatMessage.Message)?.Type;
                var output = outputCommand.Replace("[UserName]", e.ChatMessage.Username);

                if (outputCommand != null && isSound != CommandType.Sound)
                {
                    _client.SendMessage(e.ChatMessage.Channel, output);
                }
                else if (outputCommand != null && isSound == CommandType.Sound)
                {
                    var soundFile = _db.Command.FirstOrDefault(s => "!" + s.Name == e.ChatMessage.Message)?.SoundFile;
                    WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer
                    {
                        URL = $"{_soundPath}{soundFile}"
                    };

                    mediaPlayer.controls.play();
                    _client.SendMessage(e.ChatMessage.Channel, output);
                }
            }
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }
    }
}
