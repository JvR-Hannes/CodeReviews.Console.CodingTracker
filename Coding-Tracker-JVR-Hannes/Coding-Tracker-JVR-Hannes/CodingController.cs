using Dapper;
using Spectre.Console;
using System.Data.SQLite;
namespace Coding_Tracker_JVR_Hannes
{
    public class CodingController
    {
        private readonly string _connectionString;

        public CodingController(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<CodingSession> GetAllCodingSessions()
        {
            Console.Clear();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM coding_session";

                var sessions = connection.Query<CodingSession>(selectQuery).AsList();

                return sessions;
            }
        }
        public void InsertCodingSession(CodingSession session)
        {
            Console.Clear();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                session.CalculateDuration();

                string insertQuery = 
                    @"INSERT INTO coding_session (StartTime, EndTime, Duration)
                      VALUES (@StartTime, @EndTime, @Duration)";

                connection.Execute(insertQuery, session);

                Console.WriteLine("Coding session logged successfully!");
            }
        }
        public void DeleteCodingSession(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = 
                    @"DELETE FROM coding_session WHERE Id = @Id";
                var affectedRows = connection.Execute(deleteQuery, new {Id = id});

                if (affectedRows > 0)
                {
                    Console.WriteLine("Session deleted succesfully");
                }
                else
                {
                    Console.WriteLine("No session found with the provided ID.");
                }
            }
            Console.Clear();
        }
        public void ClearDatabase()
        {
            Console.Clear();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string clearDatabase = 
                    @"DELETE FROM coding_session";
                using (var command = new SQLiteCommand(clearDatabase, connection))
                {
                    command.ExecuteNonQuery();
                }

                AnsiConsole.Markup("[bold red]All records have been deleted.[/]");
                connection.Close();
            }
        }
        public void UpdateCodingSession(CodingSession session)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"UPDATE coding_session 
              SET StartTime = @startTime, EndTime = @endTime, Duration = @duration 
              WHERE Id = @id";

                tableCmd.Parameters.AddWithValue("@id", session.Id);
                tableCmd.Parameters.AddWithValue("@startTime", session.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                tableCmd.Parameters.AddWithValue("@endTime", session.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                tableCmd.Parameters.AddWithValue("@duration", (session.EndTime - session.StartTime).TotalHours);

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public CodingSession GetCodingSessionById(int id)
        {
            CodingSession codingSession = null;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"SELECT Id, StartTime, EndTime 
              FROM coding_session 
              WHERE Id = @id";

                tableCmd.Parameters.AddWithValue("@id", id);

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        codingSession = new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            StartTime = DateTime.Parse(reader.GetString(1)),
                            EndTime = DateTime.Parse(reader.GetString(2)),
                        };
                    }
                }
                connection.Close();
            }
            return codingSession;
        }
    }
}
