using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.Entities;
using RepositoryLayer.CustomExceptions;
using System.Security.Cryptography;

namespace FundooNotesUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBl labelbl;
        private readonly ILogger<LabelController> logger;
        public LabelController(ILabelBl labelbl, ILogger<LabelController> logger)
        {
            this.labelbl = labelbl;
            this.logger = logger;
        }

        //----------------------------------------------------------------------------------------------
        [HttpPost("AddLabel")]
        public async Task<IActionResult> AddLabel(Label label)
        {
            try
            {
                int result = await labelbl.AddLabel(label);   

                if (result == 1)
                {
                    logger.LogInformation("label is added");
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "label added successfully",
                        Data = label
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Faild to add label"

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
        [HttpGet("GetLabel/{email}")]
        public async Task<IActionResult> GetUserNoteLabels(string email)
        {
            try
            {
                var labels = await labelbl.GetUserNoteLabels(email);

                if (labels.Any())
                {
                    return Ok(new ResponseModel<IEnumerable<Label>>
                    {
                        Success = true,
                        Message = "Labels retrieved successfully",
                        Data = labels
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "No labels found for the provided email"
                    });
                }
            }
            catch(EmptyListException ex)

            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while processing the request"
                });
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("updateName")]
        public async Task<IActionResult> UpdateName(string newLabelName, string oldLabelName, string email)
        {
            try
            {
                int rowsAffected = await labelbl.UpdateName(newLabelName, oldLabelName, email);

                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Label name updated successfully"
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "No label found with the provided name and email"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while processing the request",
                    Data = ex.Message
                });
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpDelete]
        public async Task<IActionResult> DeleteLabel(string name, string email)
        {
            try
            {
                int result = await labelbl.DeleteLabel(name, email);

                if (result > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Label deleted successfully"
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Label not found or not deleted"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest
                    (new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while processing the request",
                    Data = ex.Message
                });
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------

    }
}
