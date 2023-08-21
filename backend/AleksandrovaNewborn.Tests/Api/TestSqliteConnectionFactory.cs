using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace AleksandrovaNewborn.Tests.Api;

public class TestSqliteConnectionFactory
{
    private readonly ILogger<TestSqliteConnectionFactory> _logger;
    private readonly IDictionary<string, SqliteConnection> _connections;

    public TestSqliteConnectionFactory(ILogger<TestSqliteConnectionFactory> logger)
    {
        _logger = logger;
        _connections = new Dictionary<string, SqliteConnection>();
    }

    public SqliteConnection GetConnection(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (!_connections.ContainsKey(key))
        {
            var result = new SqliteConnection("DataSource=:memory:");
            result.Open();
            _logger.LogInformation("Created new Sqlite connection for {ConnectionKey}", key);
            _connections.Add(key, result);
        }
        else
        {
            _logger.LogInformation("Re-using existing Sqlite connection for {ConnectionKey}", key);
        }

        return _connections[key];
    }
}