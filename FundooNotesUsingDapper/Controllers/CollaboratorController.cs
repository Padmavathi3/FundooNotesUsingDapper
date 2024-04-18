using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entities;
using ModelLayer;
using RepositoryLayer.CustomExceptions;

namespace FundooNotesUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBl collaboratorbl;
        public CollaboratorController(ICollaboratorBl collaboratorbl)
        {
            this.collaboratorbl = collaboratorbl;
        }

        //----------------------------------------------------------------------------------------------
        [HttpPost("AddCollaborator")]
        public async Task<IActionResult> AddCollaborator(Collaborator addcollab)
        {
            try
            {
                int result = await collaboratorbl.AddCollaborator(addcollab);

                if (result == 1)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "collaborator added successfully",
                        Data=addcollab
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Faild to add collaborator"

                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message

                }); ;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
        [HttpDelete("DeleteCollaborator/{cid}/{nid}")]
        public async Task<IActionResult> DeleteCollaborator(int cid, int nid)
        {
            try
            {
                int result = await collaboratorbl.DeleteCollaborator(cid, nid);

                if (result > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Collaborator deleted successfullyy"

                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Collaborator with the given id and note id does not exist."

                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message

                }); ;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("GetAllCollaboratorsById/{nid}")]
        public async Task<IActionResult> GetAllCollaborators(int nid)
        {
            try
            {
                var collaborators = await collaboratorbl.GetAllCollaborators(nid);

                if (collaborators != null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Collaborator Retrieved successfullyy",
                        Data = collaborators

                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "No one collaborator is present with this note id"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
