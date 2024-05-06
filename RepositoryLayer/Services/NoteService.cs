using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteService:INote
    {
        private readonly DapperContext _context;
        public NoteService(DapperContext context)
        {
            _context = context;
        }

        //Logic for inserting records
        public async Task<int> CreateNote(Note re_var)
        {
            var query = "INSERT INTO UserNote (Title, Description, reminder, isArchive, isPinned, isTrash, EmailId,IsColour) " +
                        "VALUES (@Title, @Description, @Reminder, @IsArchive, @IsPinned, @IsTrash, @EmailId,@IsColour)";

            var parameters = new DynamicParameters();
           
            parameters.Add("@Title", re_var.Title, DbType.String);
            parameters.Add("@Description", re_var.Description, DbType.String);
            parameters.Add("@Reminder", re_var.Reminder, DbType.DateTime);
            parameters.Add("@IsArchive", re_var.IsArchive, DbType.Boolean);
            parameters.Add("@IsPinned", re_var.IsPinned, DbType.Boolean);
            parameters.Add("@IsTrash", re_var.IsTrash, DbType.Boolean);
            parameters.Add("@EmailId", re_var.EmailId, DbType.String);
            parameters.Add("@IsColour", re_var.IsColour, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------
        //logic for Display the all notes

        public async Task<IEnumerable<Note>> GetNotesByEmail(string email)
        {
            var query = "SELECT * FROM USerNote where EmailId=@EmailId";

            using (var connection = _context.CreateConnection())
            {
                var notes = await connection.QueryAsync<Note>(query, new{ EmailId=email});

                if (notes.Any())
                {
                    return notes;
                }
                else
                {
                    throw new EmptyListException("Note is not  present in the table.");
                }
            }
        }

        //==================================================================================================================================================================
        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            var query = "SELECT * FROM UserNote";

            using (var connection = _context.CreateConnection())
            {
                var notes = await connection.QueryAsync<Note>(query);

                if (notes.Any())
                {
                    return notes;
                }
                else
                {
                    throw new EmptyListException("Notes are not present");
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<int> UpdateNote(int id, Note re_var)
            {
                var query = @"UPDATE UserNote SET 
                  Title = @Title, 
                  Description = @Description, 
                  Reminder = @Reminder, 
                  IsArchive = @IsArchive, 
                  IsPinned = @IsPinned, 
                  IsTrash = @IsTrash,
                  IsColour = @IsColour
                  WHERE NoteId = @NoteId";

                var parameters = new DynamicParameters();
                parameters.Add("@NoteId", id, DbType.Int32);
                parameters.Add("@Title", re_var.Title, DbType.String);
                parameters.Add("@Description", re_var.Description, DbType.String);
                parameters.Add("@Reminder", re_var.Reminder, DbType.DateTime);
                parameters.Add("@IsArchive", re_var.IsArchive, DbType.Boolean);
                parameters.Add("@IsPinned", re_var.IsPinned, DbType.Boolean);
                parameters.Add("@IsTrash", re_var.IsTrash, DbType.Boolean);
                parameters.Add("@IsColour", re_var.IsColour, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    return await connection.ExecuteAsync(query, parameters);
                }
            }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<int> DeleteNote(int id, string email)
        {
            var check_note_query = "SELECT COUNT(*) FROM UserNote WHERE NoteId = @NoteId AND EmailId = @EmailId";
            var delete_note_query = "DELETE FROM UserNote WHERE NoteId = @NoteId AND EmailId = @EmailId";

            using (var connection = _context.CreateConnection())
            {
                // Check if the note exists for the given email
                int noteCount = await connection.ExecuteScalarAsync<int>(check_note_query, new { EmailId = email, NoteId = id });

                if (noteCount == 0)
                {
                    // If no note is found with the given id and email, throw custom exception
                    throw new IdNotFoundException("NoteId does not exist.");
                }
                else
                {
                    // Delete the note
                    int rowsAffected = await connection.ExecuteAsync(delete_note_query, new { NoteId = id, EmailId = email });

                    if (rowsAffected == 0)
                    {
                        // If no rows are affected, the note was not deleted successfully
                        return 0;
                    }

                    return rowsAffected;
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
        public async Task<int> ArchiveNote(int id, string email)
        {
            var select_query = "SELECT IsArchive FROM UserNote WHERE NoteId = @NoteId AND EmailId = @EmailId";
            var update_query = "UPDATE UserNote SET IsArchive = @IsArchive WHERE NoteId = @NoteId AND EmailId = @EmailId";

            using (var connection = _context.CreateConnection())
            {
                // Check the current state of isarchive
                bool isArchive = await connection.ExecuteScalarAsync<bool>(select_query, new { NoteId = id, EmailId = email });

                // Toggle the value of isarchive
                isArchive = !isArchive;

                // Update the database with the new value of isarchive
                int rowsAffected = await connection.ExecuteAsync(update_query, new { IsArchive = isArchive, NoteId = id, EmailId = email });

                if (rowsAffected == 0)
                {
                    // If no rows are affected, the update was not successful
                    return 0;
                }

                return rowsAffected;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> PinnNote(int id, string email)
        {
            var select_query = "SELECT IsPinned FROM UserNote WHERE NoteId = @NoteId AND EmailId = @EmailId";
            var update_query = "UPDATE UserNote SET IsPinned = @IsPinned WHERE NoteId = @NoteId AND EmailId = @EmailId";

            using (var connection = _context.CreateConnection())
            {
                // Check the current state of isarchive
                bool isPinned = await connection.ExecuteScalarAsync<bool>(select_query, new { NoteId = id, EmailId = email });

                // Toggle the value of isarchive
                isPinned = !isPinned;

                // Update the database with the new value of isarchive
                int rowsAffected = await connection.ExecuteAsync(update_query, new { IsPinned = isPinned, NoteId = id, EmailId = email });

                if (rowsAffected == 0)
                {
                    // If no rows are affected, the update was not successful
                    return 0;
                }

                return rowsAffected;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<int> TrashNote(int id, string email) 
        {
            var select_query = "SELECT IsTrash FROM UserNote WHERE NoteId = @NoteId AND EmailId = @EmailId";
            var update_query = "UPDATE UserNote SET IsTrash = @IsTrash WHERE NoteId = @NoteId AND EmailId = @EmailId";

            using (var connection = _context.CreateConnection())
            {
                // Check the current state of isarchive
                bool isTrash = await connection.ExecuteScalarAsync<bool>(select_query, new { NoteId = id, EmailId = email });

                // Toggle the value of isarchive
                isTrash = !isTrash;

                // Update the database with the new value of isarchive
                int rowsAffected = await connection.ExecuteAsync(update_query, new { IsTrash = isTrash, NoteId = id, EmailId = email });

                if (rowsAffected == 0)
                {
                    // If no rows are affected, the update was not successful
                    return 0;
                }

                return rowsAffected;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateColour(int id, string colour)
        {
            var query = @"UPDATE UserNote SET
                  IsColour = @IsColour
                  WHERE NoteId = @NoteId";

            var parameters = new DynamicParameters();
            parameters.Add("@IsColour", colour, DbType.String);
            parameters.Add("@NoteId", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

    }



}
