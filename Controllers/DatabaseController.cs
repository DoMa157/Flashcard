using FlashCards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashCards.Controllers
{
    internal class DatabaseController
    {
        private string _connectionString;
        public StackController StackController { get; set; }
        public FlashcardController FlashcardController { get; set; }

        public StudySessionController StudySessionController { get; set; }

        public DatabaseController(string connectionString) 
        {
            _connectionString = connectionString;
            StackController = new StackController(connectionString);
            FlashcardController = new FlashcardController(connectionString);
            StudySessionController = new StudySessionController(connectionString);
            InitTables();
        }

        internal void InitTables()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = new SqlCommand("IF OBJECT_ID(N'stack', N'U') IS NULL CREATE TABLE stack (StackId int IDENTITY(1,1) PRIMARY KEY, Name varchar(30) UNIQUE)", connection);
                query.ExecuteNonQuery();
                connection.Close();
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlCommand("IF OBJECT_ID(N'flashcard', N'U') IS NULL CREATE TABLE  flashcard (CardId int IDENTITY(1,1) PRIMARY KEY, Front varchar(30), Back varchar(30), StackId int FOREIGN KEY REFERENCES stack(StackId))", connection);
                connection.Open();
                query.ExecuteNonQuery();
                connection.Close();
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = new SqlCommand("IF OBJECT_ID(N'studyarea', N'U') IS NULL CREATE TABLE studyarea (SessionId int IDENTITY(1,1) PRIMARY KEY, StackId int FOREIGN KEY REFERENCES stack(StackId), Date time, Score int)", connection);
                connection.Open();
                query.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
