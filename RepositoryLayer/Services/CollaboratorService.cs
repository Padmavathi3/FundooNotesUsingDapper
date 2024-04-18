using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class CollaboratorService : ICollaborator
    {
        private readonly DapperContext _context;
        public CollaboratorService(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddCollaborator(Collaborator re_var)
        { 
            var checkEmailQuery = "SELECT COUNT(*) FROM Person WHERE EmailId = @EmailId";
            var insertCollaboratorQuery = "INSERT INTO Collaborators (CollaboratorId, NoteId, CollaboratorEmail) VALUES (@CollaboratorId, @NoteId, @CollaboratorEmail)";

            using (var connection = _context.CreateConnection())
            {
                int emailCount = await connection.ExecuteScalarAsync<int>(checkEmailQuery, new { EmailId = re_var.CollaboratorEmail });

                if (emailCount == 0)
                { 
                    throw new EmailNotFoundException($"Collaborator with email '{re_var.CollaboratorEmail}' is not a registered user. Please register first and try again.");
                }
                try
                {
                    // Add collaborator
                    var parameters = new DynamicParameters();
                    parameters.Add("@CollaboratorId", re_var.CollaboratorId, DbType.Int32);
                    parameters.Add("@NoteId", re_var.NoteId, DbType.Int32);
                    parameters.Add("@CollaboratorEmail", re_var.CollaboratorEmail, DbType.String);

                    await connection.ExecuteAsync(insertCollaboratorQuery, parameters);
                    return 1;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> DeleteCollaborator(int cid, int nid)
        {
            var check_collaborator_query = "SELECT COUNT(*) FROM collaborators WHERE NoteId = @NoteId AND CollaboratorId = @CollaboratorId";
            var delete_Collaborator_query = "DELETE FROM Collaborators WHERE NoteId = @NoteId AND CollaboratorId = @CollaboratorId";

            using (var connection = _context.CreateConnection())
            {
                // Check if the note exists for the given email
                int noteCount = await connection.ExecuteScalarAsync<int>(check_collaborator_query, new { NoteId = nid, CollaboratorId = cid });

                if (noteCount == 0)
                {
                    // If no note is found with the given id and email, throw custom exception
                    throw new IdNotFoundException("Id does not exist.");
                }
                else
                {
                    // Delete the note
                    int rowsAffected = await connection.ExecuteAsync(delete_Collaborator_query, new { NoteId = nid, CollaboratorId = cid });

                    if (rowsAffected == 0)
                    {
                        // If no rows are affected, the note was not deleted successfully
                        return 0;
                    }

                    return rowsAffected;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<Collaborator>> GetAllCollaborators(int nid)
        {
            var query = "SELECT * FROM collaborators where NoteId=@NoteId";

            using (var connection = _context.CreateConnection())
            {
                var collaborators = await connection.QueryAsync<Collaborator>(query, new { NoteId = nid });

                if (collaborators.Any())
                {
                    return collaborators;
                }
                else
                {
                    throw new EmptyListException($"Collaborator is not present with this {nid}.");
                }
            }
        }
    }
}
