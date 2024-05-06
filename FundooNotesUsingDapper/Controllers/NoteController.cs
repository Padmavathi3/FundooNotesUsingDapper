using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.Entities;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Services;

namespace FundooNotesUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBl notebl;
        private readonly ILogger<NoteController> logger;    
        public NoteController(INoteBl notebl, ILogger<NoteController> logger)
        {
            this.notebl = notebl;
            this.logger = logger;
        }

        [HttpPost("AddNote")]
        public async Task<IActionResult> CreateNote(Note updateDto1)
        {
            try
            {
                int rowsAffected = await notebl.CreateNote(updateDto1);

                if (rowsAffected > 0)
                {
                    logger.LogInformation("Notes created successfully");
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Note created successfully", // Changed message to reflect note creation
                        Data = updateDto1
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to create note", // Changed message to reflect note creation failure
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("GetNote/{id}")]
        public async Task<IActionResult> GetNotesById(int id)
        {
            try
            {
                var notes = await notebl.GetNotesById(id);

                if (notes.Any())
                {
                    logger.LogInformation("notes are retrived for particular user based on email");
                    var response = new ResponseModel<IEnumerable<Note>> // Assuming the type here is Note
                    {
                        Success = true,
                        Message = "Notes retrieved successfully",
                        Data = notes
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<IEnumerable<Note>> // Assuming the type here is Note
                    {
                        Success = false,
                        Message = "Note with the given ID not found",
                        Data = null
                    };

                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<Note>> // Assuming the type here is Note
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

                return BadRequest(response); // Changed to BadRequest since it's an exception in processing the request
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("GetAllNotes")]
        [Authorize]
        public async Task<IActionResult> GetAllNotes()
        {
            try
            {
                var notes = await notebl.GetAllNotes();

                if (notes.Any())
                {
                    logger.LogInformation("noted are retrieved successfully");
                    var response = new ResponseModel<IEnumerable<Note>> // Assuming the type here is Note
                    {
                        Success = true,
                        Message = "Notes retrieved successfully",
                        Data = notes
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<IEnumerable<Note>> // Assuming the type here is Note
                    {
                        Success = false,
                        Message = "Notes list is empty",
                        Data = null
                    };

                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<Note>> // Assuming the type here is Note
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

                return BadRequest(response); // Changed to BadRequest since it's an exception in processing the request
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------
        [HttpPut("UpdateNote/{id}")]
        public async Task<IActionResult> UpdateNote(int id, Note re_var)
        {
            try
            {
                int rowsAffected = await notebl.UpdateNote(id, re_var);

                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Note updated successfully",
                        Data = re_var
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to update note",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------
        [HttpDelete]
        [Route("DeleteNote/{id}/{email}")]
        public async Task<IActionResult> DeleteNote(int id, string email)
        {
            try
            {
                int rowsAffected = await notebl.DeleteNote(id, email);
                if (rowsAffected > 0)
                {
                    var responseModel = new ResponseModel<object>
                    {
                        Success = true,
                        Message = "note deleted succesdsfully",
                        Data = null
                    };
                    return Ok(responseModel);
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to update note",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
<<<<<<< Updated upstream
        [HttpPost("ArchiveNote")]
        public async Task<IActionResult> ArchiveNote(int id, string email)
=======
        [HttpPatch("ArchiveNote/{id}")]
        [Authorize]
        public async Task<IActionResult> ArchiveNote(int id)
>>>>>>> Stashed changes
        {
            try
            {
                int rowsAffected = await notebl.ArchiveNote(id, email);

                if (rowsAffected > 0)
                {
                    logger.LogInformation("note is archieved");
                    var responseModel = new ResponseModel<object>
                    {
                        Success = true,
                        Message = "note archived status is changed",

                    };
                    return Ok(responseModel);
                }
                else
                {
                    var responseModel = new ResponseModel<object>
                    {
                        Success = false,
                        Message = "note archived status is not toggled",

                    };
                    return Ok(responseModel);
                }
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(responseModel);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [HttpPost("PinnNote")]
        public async Task<IActionResult> PinnNote(int id, string email)
        {
            try
            {
                int rowsAffected = await notebl.PinnNote(id, email);

                if (rowsAffected > 0)
                {
                    
                    var responseModel = new ResponseModel<object>
                    {
                        Success = true,
                        Message = "note pinned status is changed",

                    };
                    return Ok(responseModel);
                }
                else
                {
                    var responseModel = new ResponseModel<object>
                    {
                        Success = false,
                        Message = "note pinn status is not toggled",

                    };
                    return Ok(responseModel);
                }
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(responseModel);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
<<<<<<< Updated upstream
        [HttpPost("TrashNote")]
        public async Task<IActionResult> TrashNote(int id, string email)
=======
        [HttpPatch("TrashNote/{id}")]
        [Authorize]
        public async Task<IActionResult> TrashNote(int id)
>>>>>>> Stashed changes
        {
            try
            {
                int rowsAffected = await notebl.TrashNote(id, email);

                if (rowsAffected > 0)
                {
                    logger.LogInformation("note is trasched");
                    var responseModel = new ResponseModel<object>
                    {
                        Success = true,
                        Message = "note trash status is changed",

                    };
                    return Ok(responseModel);
                }
                else
                {
                    var responseModel = new ResponseModel<object>
                    {
                        Success = false,
                        Message = "note trash status is not toggled",

                    };
                    return Ok(responseModel);
                }
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(responseModel);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------
        [HttpPut("UpdateColour/{id}/{updatedColour}")]
        [Authorize]
        public async Task<IActionResult> UpdateColour(int id, string updatedColour)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            try
            {
                int rowsAffected = await notebl.UpdateColour(id, updatedColour);

                if (rowsAffected > 0)
                {
                    logger.LogInformation("note is colour is updated");
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "colour updated successfully",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to update colour",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
