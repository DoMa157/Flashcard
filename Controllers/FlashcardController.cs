using FlashCards.Models;
using Microsoft.Data.SqlClient;

namespace FlashCards.Controllers
{
    internal class FlashcardController
    {
        private string _connectionString;

        public FlashcardController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddFlashcardIntoStack(FlashcardDTO flashcard, FlashcardStackDTO stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("INSERT INTO flashcard(Front, Back, StackId) VALUES (@Front, @Back, @StackId)", connection);
                sqlCommand.Parameters.AddWithValue("@Front", flashcard.Front);
                sqlCommand.Parameters.AddWithValue("@Back", flashcard.Back);
                sqlCommand.Parameters.AddWithValue("StackId", stack.StackId);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }
        public bool DeleteFlashcard(FlashcardDTO flashcard)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("DELETE flashcard WHERE CardId = @Id", connection);
                sqlCommand.Parameters.AddWithValue("@Id", flashcard.Id);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }

        public bool ModifyFlashCard(FlashcardDTO flashcard)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("UPDATE flashcard SET Front = @Front, Back = @Back WHERE CardId = @Id", connection);
                sqlCommand.Parameters.AddWithValue("@Front", flashcard.Front);
                sqlCommand.Parameters.AddWithValue("@Back", flashcard.Back);
                sqlCommand.Parameters.AddWithValue("@Id", flashcard.Id);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }

        public List<FlashcardDTO> GetAllFlashcards()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                List<FlashcardDTO> flashcardDTOs = new List<FlashcardDTO>();
                var sqlCommand = new SqlCommand("SELECT CardId, Front, Back, Name FROM flashcard as fc INNER JOIN stack as st ON fc.StackId = st.StackId", connection);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flashcardDTOs.Add(new FlashcardDTO() { Id = reader.GetInt32(0), Front = reader.GetString(1), Back = reader.GetString(2), StackName = reader.GetString(3) });
                    }
                    return flashcardDTOs;
                }
            }
        }
        public bool CheckForFlashcard(FlashcardDTO flashcardDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("SELECT 1 FROM flashcard WHERE CardId = @Id", connection);
                sqlCommand.Parameters.AddWithValue("@Id", flashcardDTO.Id);
                var execute = sqlCommand.ExecuteScalar();
                connection.Close();
                return execute != null;
            }
        }
    }
}
