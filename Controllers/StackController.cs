using FlashCards.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Controllers
{
    internal class StackController
    {
        private string _connectionString;
        public StackController(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool AddStack(FlashcardStackDTO stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("INSERT INTO stack (Name) VALUES(@Name)", connection);
                sqlCommand.Parameters.AddWithValue("@Name", stack.StackName);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }

        public bool DeleteStack(FlashcardStackDTO stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("DELETE stack WHERE Name = @Name", connection);
                sqlCommand.Parameters.AddWithValue("@Name", stack.StackName);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }
        public List<FlashcardStackDTO> GetStacks()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("SELECT * FROM stack", connection);
                List<FlashcardStackDTO> flashcardStackDTOs = new();
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flashcardStackDTOs.Add(new FlashcardStackDTO { StackId = reader.GetInt32(0), StackName = reader.GetString(1) });
                    }
                }
                connection.Close();
                return flashcardStackDTOs;
            }
        }
        public List<FlashcardDTO> GetFlashcards(FlashcardStackDTO stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("SELECT CardId, Front, Back FROM stack as st INNER JOIN flashcard as fc ON st.StackId = fc.StackId WHERE fc.stackId = @stackId", connection);
                sqlCommand.Parameters.AddWithValue("@stackId", stack.StackId);
                List<FlashcardDTO> flashcardDTOs = new();
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flashcardDTOs.Add(new FlashcardDTO { Id = reader.GetInt32(0), Back = reader.GetString(1), Front = reader.GetString(2) });
                    }
                }
                connection.Close();
                return flashcardDTOs;
            }
        }

        public bool ModifyStackName(FlashcardStackDTO oldStack, FlashcardStackDTO newStack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("UPDATE stack SET Name = @NewName WHERE Name = @OldName", connection);
                sqlCommand.Parameters.AddWithValue("@OldName", oldStack.StackName);
                sqlCommand.Parameters.AddWithValue("@NewName", newStack.StackName);
                var execute = sqlCommand.ExecuteNonQuery();
                connection.Close();
                return execute > 0;
            }
        }

        public bool CheckForStack(FlashcardStackDTO stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("SELECT 1 FROM stack WHERE Name = @Name", connection);
                sqlCommand.Parameters.AddWithValue("@Name", stack.StackName);
                var execute = sqlCommand.ExecuteScalar();
                connection.Close();
                return execute != null;
            }
        }

        public int GetStackId(FlashcardStackDTO stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand("SELECT StackId FROM STACK WHERE Name = @Name", connection);
                sqlCommand.Parameters.AddWithValue("@Name", stack.StackName);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    return -1;
                }
            }
        }
    }
}
