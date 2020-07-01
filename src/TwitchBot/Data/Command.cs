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