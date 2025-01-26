using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Login;
using eInsuranceApp.RepositoryLayer.Implementation;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class LoginBL : ILoginBL
    {
        private readonly ILoginRL _loginRL;
        private readonly IConfiguration _configuration;
        public LoginBL(ILoginRL loginRL , IConfiguration configuration)
        {
            _loginRL = loginRL;
            _configuration = configuration;
        }

        public async Task<string> Login(string emailOrUsername, string password)
        {
            try
            {
                //var user = await _loginRL.Login(emailOrUsername, password);
                var user = await _loginRL.GetUserByCredentialsAsync(emailOrUsername, password);

                if (user == null)
                    throw new UnauthorizedAccessException("Invalid credentials.");

                if (user != null) { 
                    return GenerateToken(user);
                }
                else
                {
                    throw new Exception("Invalid email/username or password.");
                }
            }
            catch (Exception ex){
                throw new Exception("Couldn't login to your account", ex);
            }
        }

        private string GenerateToken(IUser user)
        {
            try
            {
                //var jwtKey = _configuration["JwtKey"];
                var jwtKey = _configuration["JwtSettings:JwtKey"];
                //var decodedKey = Convert.FromBase64String(jwtKey);
                var decodedKey = Encoding.ASCII.GetBytes(jwtKey);


                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new Exception("JWT key is missing in the configuration.");
                }

                //var key = Encoding.ASCII.GetBytes(jwtKey);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                //var key = new SymmetricSecurityKey(decodedKey);
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var tokenHandler = new JwtSecurityTokenHandler();
                
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("Username", user.Username),
                        new Claim("Password", user.Password),
                        new Claim("FullName", user.FullName)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    //SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)

                    //Issuer = issuer,
                    //Audience = audience
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the token.", ex);
            }
        }


        
    }
}
