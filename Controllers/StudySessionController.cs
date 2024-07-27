using FlashCards.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Controllers
{
    internal class StudySessionController
    {
        private string _connectionString;

        public StudySessionController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<StudySessionDTO> GetSessions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("SELECT SessionId, Name, Date, Score  FROM studyarea as sa INNER JOIN stack as st on sa.StackId = st.StackId", connection);
                List<StudySessionDTO> sessions = new();
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessions.Add(new StudySessionDTO { SessionId = reader.GetInt32(0), StackName = reader.GetString(1), Duration = reader.GetTimeSpan(2), Score = reader.GetInt32(3) });
                    }
                }
                return sessions;
            }
        }

        public bool AddSession(StudySession session)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("INSERT INTO studyarea(StackId, Date, Score) VALUES (@StackId, @Date, @Score)", connection);
                sqlCommand.Parameters.AddWithValue("@StackId", session.StackId);
                sqlCommand.Parameters.AddWithValue("@Date", session.Duration);
                sqlCommand.Parameters.AddWithValue("Score", session.Score);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }

        public bool DeleteSession(StudySessionDTO session)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("DELETE studyarea WHERE SessionId = @SessionId", connection);
                sqlCommand.Parameters.AddWithValue("SessionId", session.SessionId);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }
        public bool CheckSession(StudySessionDTO session)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("SELECT 1 FROM studyarea WHERE SessionId = @SessionId", connection);
                sqlCommand.Parameters.AddWithValue("@SessionId", session.SessionId);
                var execute = sqlCommand.ExecuteScalar();
                connection.Close();
                return execute != null;
            }
        }
    }
}
