using MySql.Data.MySqlClient;
using System;
using System.Text.Json;

namespace PrimeNumbers.PrimesDB.Core
{
    public class PrimeCacheDb : IDisposable
    {
        private MySqlConnection _connection;

        internal PrimeCacheDb(MySqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Tries to find a prime record for the given number
        /// </summary>
        /// <returns>If a record was found or not</returns>
        public bool TryGetPrimeRecord(ulong number, out PrimeRecord record)
        {
            var command = new MySqlCommand("SELECT * FROM primefactoring WHERE number = @number LIMIT 1;", _connection);
            command.Parameters.Add("@number", MySqlDbType.UInt64);
            command.Parameters["@number"].Value = number;
            using MySqlDataReader reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                record = GetRecordFromReader(reader);
                return true;
            }
            else
            {
                record = null;
                return false;
            }

        }

        public void InsertPrimeRecord(PrimeRecord record)
        {
            var command = new MySqlCommand("INSERT IGNORE INTO primefactoring VALUES (@number, @isPrime, @primeFactors);", _connection);
            command.Parameters.Add("@number", MySqlDbType.UInt64);
            command.Parameters.Add("@isPrime", MySqlDbType.Bit);
            command.Parameters.Add("@primeFactors", MySqlDbType.JSON);

            command.Parameters["@number"].Value = record.Number;
            command.Parameters["@isPrime"].Value = record.IsPrime;
            command.Parameters["@primeFactors"].Value = JsonSerializer.Serialize(record.PrimeFactors);

            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }

            GC.SuppressFinalize(this);
        }

       
        private static PrimeRecord GetRecordFromReader(MySqlDataReader reader)
        {
            ulong number = reader.GetUInt64(0);
            bool isPrime = reader.GetBoolean(1);
            ulong[] primeFactors = JsonSerializer.Deserialize<ulong[]>(reader.GetString(2));
            return new PrimeRecord(number, isPrime, primeFactors);
        }
    }
}
