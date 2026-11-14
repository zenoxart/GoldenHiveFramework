using System.Data.Common;
using Npgsql;

namespace GoldenHive.NET.WinUI.Framework.Backend.Core.Data;

/// <summary>The only place that knows the database driver: creates the PostgreSQL data source.</summary>
public static class PostgresDataSource
{
    public static DbDataSource Create(string connectionString) => NpgsqlDataSource.Create(connectionString);
}
