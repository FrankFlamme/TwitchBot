using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Data
{
    public enum CommandType
    {
        Text,
        Sound
    }

    public class Command
    {
        public int Id { get; set; }
        public CommandType Type { get; set; }
        public string Name { get; set; }
        public string Output { get; set; }
        public string SoundFile { get; set; }
    }
}
