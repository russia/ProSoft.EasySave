namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages.Client
{
    public record ClientHandshake : ISendable
    {
        public ClientHandshake(string instance)
        {
            Instance = instance;
        }

        public string Instance { get; set; }
    }
}