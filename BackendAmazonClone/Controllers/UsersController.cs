using BackendAmazonClone.Repositories;
using BackendAmazonClone.Models;
using BackendAmazonClone.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using BackendAmazonClone.DTOs;

namespace BackendAmazonClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private UserHelper _userHelper;
        private readonly IConfiguration _configuration;
        public UsersController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userHelper = new UserHelper();
            _configuration = configuration;
        }

        //api/Users
        /*[HttpGet]
        public async Task<IEnumerable<Users>> GetUsers()
        {
            var getAllUsers = await _userRepository.Get();

            return getAllUsers;
        }*/

        //api/Users/5
        //used to get data
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUsers(long id, [FromBody] UserDTO user)
        {
            if (id != user.Id)
            {
                return Unauthorized(new { message = "You do not have access to this url" });
            }

            var returnedGet = await _userRepository.Get(id);

            UserDTO dataToReturn = _userHelper.ConstructUserDTO(returnedGet);

            return Ok(dataToReturn);
        }

        //api/Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser([FromBody] Users user)
        {
            bool emailValidated = _userHelper.ValidateEmail(user);
            bool emptyStrings = _userHelper.EmptyStrings(user);
            var emailEnumerator = _userRepository.SearchSpecificColumn("email").GetEnumerator();
            bool duplicateEmail = _userHelper.DuplicateEmail(user, emailEnumerator);

            if (emptyStrings)
            {
                return BadRequest(new { message = "Please fill in all fields" });
            }
            else if (emailValidated)
            {
                return BadRequest(new { message = "Invalid email address" });
            }
            else if (duplicateEmail)
            {
                return BadRequest(new { message = "This email address already exists" });
            }

            byte[] salt = _userHelper.GenerateSalt();
            string hashedPassword = _userHelper.HashPassword(user.Password, salt);

            Users userToCreate = new Users()
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Salt = salt,
                HashedPassword = hashedPassword,
                Cart = new Models.Cart
                {
                    UserId = user.Id,
                    User = user,
                    CartProduct = new List<Products>()
                },
                Product = new List<Products>()
            };

            var newUser = await _userRepository.Create(userToCreate);

            UserDTO dto = _userHelper.ConstructUserDTO(userToCreate);

            return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, userToCreate);
        }

        //api/Users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Users user)
        {
            bool missingLoginField = _userHelper.MissingLoginField(user.Email, user.Password);
            var loginEnumerable = _userRepository.Authenticate(user.Email).GetEnumerator();
            int accExists = _userHelper.CheckExistingAccount(user.Email, user.Password, loginEnumerable);

            if (missingLoginField)
            {
                return BadRequest(new { message = "Please fill in all the fields" });
            }
            else if (accExists == -1)
            {
                return BadRequest(new { message = "The credentials entered do not exist" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_configuration["JwtKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddYears(200),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var returnToken = tokenHandler.WriteToken(token);

            Users getUser = await _userRepository.Get(accExists);

            if (returnToken == null) return Unauthorized();
            else {
                return Ok(new { token = returnToken, status = "ok", id = getUser.Id, name = getUser.Name, email = getUser.Email, address = getUser.Address });
            }
        }

        //api/Users/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> PutUsers(long id, [FromBody] Users user)
        {
            if (id != user.Id)
            {
                return Unauthorized();
            }

            bool emptyStrings = _userHelper.EmptyStrings(user);

            if (emptyStrings)
            {
                return BadRequest(new { message = "You must fill in name and address" });
            }

            Users userToCreate = new Users()
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address
            };

            bool passwordNeedsUpdating = false;

            if (!String.IsNullOrEmpty(user.Password))
            {
                byte[] salt = _userHelper.GenerateSalt();
                string hashedPassword = _userHelper.HashPassword(user.Password, salt);

                userToCreate.Salt = salt;
                userToCreate.HashedPassword = hashedPassword;
                passwordNeedsUpdating = true;
            }

            await _userRepository.Update(userToCreate, id, passwordNeedsUpdating);

            return Ok(new { message = "Information has updated" });
        }

        //api/Users/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser (long id, [FromBody] Users user)
        {
            var userToDelete = await _userRepository.Get(id);
            if(userToDelete == null)
            {
                return NotFound();
            }

            if (id != user.Id)
            {
                return Unauthorized();
            }

            await _userRepository.Delete(userToDelete.Id);
            return Ok(new { message = "Account deleted" });
        }

        //api/Users
        /*[HttpDelete]
        public async Task<ActionResult> ClearAll ()
        {
            await _userRepository.ClearAll();
            return NoContent();
        }*/
    }
}
