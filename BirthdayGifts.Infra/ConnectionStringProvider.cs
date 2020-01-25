namespace BirthdayGifts.Infra
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public ConnectionStringProvider()
        {
            ConnectionString = "Server=DESKTOP-2OONPBE\\SQLEXPRESS;Database=BirthdayGifts;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public string ConnectionString { get; private set; }
    }
}
