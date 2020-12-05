namespace PrimeNumbers.PrimesDB.Core
{
    public class PrimeCacheDbFactory
    {
        private readonly ConnectionParameters _connectionParameters;

        public PrimeCacheDbFactory(ConnectionParameters connectionParameters)
        {
            _connectionParameters = connectionParameters;
        }

        public PrimeCacheDb CreateConnection()
        {
            var connection = MySqlConnector.ConnectToDb(_connectionParameters);
            return new PrimeCacheDb(connection);
        }
    }
}
