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
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace RepositoryLayer.Services
{
    public class LabelService : ILabel
    {
        private readonly DapperContext _context;
        public LabelService(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddLabel(Label re_var)
        {
            var insertLabelQuery = "INSERT INTO Label (NoteId, LabelName, Email) VALUES (@NoteId, @LabelName, @Email)";

            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("NoteId", re_var.NoteId, DbType.Int32);
                parameters.Add("LabelName", re_var.LabelName, DbType.String);
                parameters.Add("Email", re_var.Email, DbType.String);

                int rowsAffected = await connection.ExecuteAsync(insertLabelQuery, parameters);
                return rowsAffected;
            }
        }



        public async Task<int> DeleteLabel(string name, string email)
        {
            var check_label_query = "SELECT COUNT(*) FROM Label WHERE LabelName=@LabelName and  Email= @Email";
            var delete_label_query = "DELETE FROM Label WHERE LabelName = @LabelName AND Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                
                int labelCount = await connection.ExecuteScalarAsync<int>(check_label_query, new { LabelName=name, Email = email});

                if (labelCount == 0)
                {
                    
                    throw new EmailNotFoundException("Email or name does not exist.");
                }
                else
                {
                    // Delete the label
                    int rowsAffected = await connection.ExecuteAsync(delete_label_query, new { LabelName = name, Email = email });

                    if (rowsAffected == 0)
                    {
                        
                        return 0;
                    }

                    return rowsAffected;
                }
            }
        }

        public async Task<IEnumerable<Label>> GetUserNoteLabels(string email)
        {
            var query = "SELECT * FROM Label WHERE Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                var labels = await connection.QueryAsync<Label>(query, new { Email = email });

                if (labels.Any())
                {
                    return labels;
                }
                else
                {
                    throw new EmptyListException("No labels found for the provided email.");
                }
            }
        }


        public async Task<int> UpdateName(string newLabelName,string oldLabelName,string email)
        {
            var check_label_query = "SELECT COUNT(*) FROM Label WHERE LabelName=@LabelName And Email= @Email";
            var updateQuery = "UPDATE Label SET LabelName = @NewLabelName WHERE LabelName = @OldLabelName AND Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                int labelCount = await connection.ExecuteScalarAsync<int>(check_label_query, new { LabelName = oldLabelName, Email = email });

                if (labelCount == 0)
                {
                    // If no note is found with the given id and email, throw custom exception
                    throw new EmailNotFoundException("Email or old label name does not exist.");
                }
                else
                {
                    int rowsAffected =await connection.ExecuteAsync(updateQuery, new { NewLabelName = newLabelName, OldLabelName = oldLabelName, Email = email });
                    if (rowsAffected == 0)
                    {

                        return 0;
                    }

                    return rowsAffected;
                }
            }
        }
    }
}
