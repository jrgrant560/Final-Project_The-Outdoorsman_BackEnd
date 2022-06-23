using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OutdoorsmanBackend.Models;

namespace OutdoorsmanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        //variable that points to the appsettings.json file
        private IConfiguration _config;
        //variable that points to the User Database
        private readonly UserContext _userContext;

        //variable that points to the appsettings.json file (For loaded constructor)
        public LoginController(IConfiguration config, UserContext userContext)
        {
            _config = config;
            _userContext = userContext;
        }

        //api/Login
        //Attribute that allows for an unauthenticated user to access the method
        [AllowAnonymous]
        //HTTP POST method that receives data to log in using a "User" model, generate a new token, and then return the token to the front-end
        [HttpPost]
        public IActionResult Login([FromBody] User login)
        {
            //by default, the response action for a user that fails authentication, which returns a 401 UnAuthorizedAccess HTTP Code
            IActionResult response = Unauthorized();
            //variable that is the result of the AuthenticateUser() method performed on the "login" parameter
            var user = AuthenticateUser(login);

            //if the user is not null (aka a valid user), perform this code
            if (user != null)
            {
                //variable for a newly generated JWT token string
                var tokenString = GenerateJSONWebToken(user);
                //variable that is a 'Microsoft.AspNetCore.Http.StatusCodes.Status200OK' response containing a new token (based on tokenString variable)
                response = Ok(new { token = tokenString , user});
            }

            //returns the final response; could be the default Unauthorized response, or, if the user is authenticated, the response will contain a new token for the front-end to use
            return response;
        }

        //method that creates new JWT tokens, taking in a submitted user's data
        private string GenerateJSONWebToken(User userInfo)
        {
            //variable that creates a new Symetric security key object
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            //variable that creates a new credentials object
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //variable that creates a new token object
            var token = new JwtSecurityToken
            (
                //issuer
                _config["Jwt:Issuer"],
                //audience
                _config["Jwt:Audience"],
                //claims
                null,
                //notBefore
                //

                //expires: parameter that controls the timer before a token expires
                expires: DateTime.Now.AddMinutes(120),
                //signingCredentials
                signingCredentials: credentials
            );

            //returns the new token, based on the token object
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //method that authenticates a user trying to log in
        private User AuthenticateUser(User login)
        {
            //by default, the user is null
            User validUser = null;

            //IQueryable<object> variable that gets any users from the database that match the "login" user's email address
            List<User>? checkUserQuery = _userContext.Users.Where(e => e.emailAddress == login.emailAddress).ToList();

            if (checkUserQuery == null)
            {
                return null;
            }

            //variable that converts checkUserQuery to Class[]
            User[] checkUserObjectArray = checkUserQuery.ToArray<User>();
            //variable that converts checkUserObjectArray to "User" object
            User checkUser = checkUserObjectArray[0];

            //variable that pulls checkUser's email address
            var checkEmailAddress = checkUser.emailAddress;
            //variable that pulls checkUser's password
            var checkPassword = checkUser.password;

            //if statement that validates the User Credentials
            //NOTE FOR IMPROVEMENT: Need to program this If statement to have a password dependent on the related email address, not just any password in the database
            if (login.emailAddress == checkEmailAddress && login.password == checkPassword)
            {
                //creates a new User object based on checkUser's properties
                validUser = new User {
                    id = checkUser.id,
                    firstName = checkUser.firstName,
                    lastName = checkUser.lastName,
                    emailAddress = checkUser.emailAddress,
                    streetAddress = checkUser.streetAddress,
                    city = checkUser.city,
                    state = checkUser.state,
                    zipCode = checkUser.zipCode,
                    phoneNumber = checkUser.phoneNumber
                };
            }

            //returns the final response; could be the default null response, or, if the user is authenticated, the response contains the validated user
            return validUser;

        }
    }
}