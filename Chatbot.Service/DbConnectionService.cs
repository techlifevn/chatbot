using Chatbot.Common;
using DemoApi.Data.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Chatbot.Service
{
    public interface IDbConnectionService
    {
        Task<bool> IsConnected(string connectionName = null);

        Task<bool> CheckConnection(string connectionString);

        string GetConnectionString(string connectionName = null);

        Task<SqlConnection> GetConnectionAsync(string connectionName = null);

        Task<SqlConnection> GetConnectionByConnectionStringAsync(string connectionString);

        Task<List<T>> ExecuteToListAsync<T>(SqlConnection connection, string query, Dictionary<string, object> parameters = null);

        Task<T> ExecuteToFirstRowAsync<T>(SqlConnection connection, string query, Dictionary<string, object> parameters = null);

        Task<T> ExecuteScalarAsync<T>(SqlConnection connection, string query, Dictionary<string, object> parameters = null);

        Task<List<T>> ExecuteStoredProcedureAsync<T>(SqlConnection connection, string storedProcedure, Dictionary<string, object> parameters = null);

        Task<bool> ExecuteCommandAsync(SqlConnection connection, string command, Dictionary<string, object> parameters = null);
    }
    public class DbConnectionService : IDbConnectionService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbConnectionService> _logger;

        public DbConnectionService(IConfiguration configuration, ILogger<DbConnectionService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> CheckConnection(string connectionString)
        {
            bool isValid = false;

            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                SqlConnection connection = null;
                try
                {
                    connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();
                    isValid = true;
                }
                catch
                {
                    isValid = false;
                }
                finally
                {
                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                }
            }

            return isValid;
        }

        public async Task<bool> ExecuteCommandAsync(SqlConnection connection, string command, Dictionary<string, object> parameters = null)
        {
            if (connection.State != ConnectionState.Open)
                connection = await GetConnectionAsync(connection.ConnectionString);

            using var cmd = new SqlCommand(command, connection);

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue($"{item.Key}", item.Value ?? DBNull.Value);
                }
            }

            try
            {
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch
            {
                _logger.LogInformation("Query: {0}, Params: {1}", command, string.Join(",", parameters.Select(kv => kv.Key + "=" + kv.Value).ToArray()));

                throw;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(SqlConnection connection, string query, Dictionary<string, object> parameters = null)
        {
            if (connection.State != ConnectionState.Open)
                connection = await GetConnectionAsync(connection.ConnectionString);

            using var cmd = new SqlCommand(query, connection);

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue($"{item.Key}", item.Value ?? DBNull.Value);
                }
            }

            try
            {
                var data = await cmd.ExecuteScalarAsync();

                return (T)data;
            }
            catch
            {
                _logger.LogInformation("Query: {0}, Params: {1}", query, string.Join(",", parameters.Select(kv => kv.Key + "=" + kv.Value).ToArray()));

                throw;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(SqlConnection connection, string storedProcedure, Dictionary<string, object> parameters = null)
        {
            if (connection.State != ConnectionState.Open)
                connection = await GetConnectionAsync(connection.ConnectionString);

            using var cmd = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue($"{item.Key}", item.Value ?? DBNull.Value);
                }
            }

            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                return reader.GetListData<T>();
            }
            catch
            {
                _logger.LogInformation("Query: {0}, Params: {1}", storedProcedure, string.Join(",", parameters.Select(kv => kv.Key + "=" + kv.Value).ToArray()));

                throw;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public async Task<T> ExecuteToFirstRowAsync<T>(SqlConnection connection, string query, Dictionary<string, object> parameters = null)
        {
            if (connection.State != ConnectionState.Open)
                connection = await GetConnectionAsync(connection.ConnectionString);

            using var cmd = new SqlCommand(query, connection);

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue($"{item.Key}", item.Value ?? DBNull.Value);
                }
            }

            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                return reader.GetFirstData<T>();
            }
            catch
            {
                _logger.LogInformation("Query: {0}, Params: {1}", query, string.Join(",", parameters.Select(kv => kv.Key + "=" + kv.Value).ToArray()));
                throw;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public async Task<List<T>> ExecuteToListAsync<T>(SqlConnection connection, string query, Dictionary<string, object> parameters = null)
        {
            if (connection.State != ConnectionState.Open)
                connection = await GetConnectionAsync(connection.ConnectionString);

            using var cmd = new SqlCommand(query, connection);

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue($"{item.Key}", item.Value ?? DBNull.Value);
                }
            }

            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                return reader.GetListData<T>();
            }
            catch
            {
                _logger.LogInformation("Query: {0}, Params: {1}", query, string.Join(",", parameters.Select(kv => kv.Key + "=" + kv.Value).ToArray()));
                throw;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public async Task<SqlConnection> GetConnectionAsync(string connectionName = null)
        {
            var connectionString = GetConnectionString(connectionName);
            var connection = new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
            return connection;
        }

        public async Task<SqlConnection> GetConnectionByConnectionStringAsync(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            var connection = new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
            return connection;
        }

        public string GetConnectionString(string connectionName = null)
        {
            return !String.IsNullOrWhiteSpace(connectionName)
                ? _configuration.GetConnectionString(connectionName)
                : _configuration.GetConnectionString(SystemConstants.ConnectionSqlServer);
        }

        public async Task<bool> IsConnected(string connectionName = null)
        {
            var connectionString = GetConnectionString(connectionName);

            return await CheckConnection(connectionString);
        }
    }
}
