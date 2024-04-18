using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using ModelLayer.Entities;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundooNotesUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBl userdl;
        private readonly IConfiguration configuration;
        public UserController(IUserBl userdl, IConfiguration configuration)
        {
            this.userdl = userdl;
            this.configuration = configuration;
        }

        //---------------------------------------------------------------------------------------------------------

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignnUp(User updateDto)
        {
            try
            {
                // Inserting user details into the database
                int rowsAffected = await userdl.Insertion(updateDto.FirstName, updateDto.LastName, updateDto.EmailId, updateDto.Password);

                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "User registered successfully",
                        Data = updateDto
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to register user",
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
        //---------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                var users = await userdl.GetUsers();

                if (users.Any())
                {
                    var response = new ResponseModel<IEnumerable<User>>
                    {
                        Success = true,
                        Message = "Users retrieved successfully",
                        Data = users
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<IEnumerable<User>>
                    {
                        Success = false,
                        Message = "No users found",
                        Data = null
                    };

                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<User>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

                return NotFound(response);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("GetbyEmail/{email}")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            try
            {
                var users = await userdl.GetUsersByEmail(email);

                if (users.Any())
                {
                    var response = new ResponseModel<IEnumerable<User>>
                    {
                        Success = true,
                        Message = "Users retrieved successfully",
                        Data = users
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<IEnumerable<User>>
                    {
                        Success = false,
                        Message = "No users found with the provided email",
                        Data = null
                    };

                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<User>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

                return NotFound(response);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpDelete("DeleteUser/{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            try
            {
                int rowsAffected = await userdl.DeleteUserByEmail(email);

                if (rowsAffected > 0)
                {
                    var responseModel = new ResponseModel<object>
                    {
                        Success = true,
                        Message = "user deleted successfully",
                    };

                    return Ok(responseModel);
                }
                else
                {
                    var responseModel = new ResponseModel<object>
                    {
                        Success = false,
                        Message = "user not deleted successfully",
                    };

                    return Ok(responseModel);
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
        //-----------------------------------------------------------------------------------------------------------------------

        [HttpPut("ForgotPassword/{Email}")]

        public async Task<IActionResult> ChangePasswordRequest(string Email)
        {
            try
            {
                var result=await userdl.ChangePasswordRequest(Email);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "mail send successfully",
                    Data = result
                };

                return Ok(response);
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



        [HttpPut("ChangePassword/{otp}/{password}")]

        public async Task<IActionResult> ChangePassword(string otp, string password)
        {
            try
            {
                var res = await userdl.ChangePassword(otp, password);

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = res,
                    Data = res
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("Login")]

        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var values = await userdl.Login(email, password);

                string token = TokenGeneration(email);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "login successfully",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "login not done successfully",
                    Data = ex.Message
                });
            }

        }
        //----------------------------------------------------------------------------------------

        private string TokenGeneration(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Email, email),
                 // Add additional claims if needed
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
