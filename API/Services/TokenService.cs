using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API.Services

{   

    public class TokenService : ITokenService //implementation class as the service
    {
     private readonly SymmetricSecurityKey _key;//server encrypts and decrypts the token key
    
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));//firma cifrada o the token key is created and encoded. The secret key is stored in the configuration 
        }
        public string CreateToken(AppUser user)
        {
           var claims = new List<Claim>{ 
             
               new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
           };

           var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);//signing in the token key, encriptacion de la firma cifrada
           
           var tokenDescriptor = new SecurityTokenDescriptor{//payload,  describes the token
           
             Subject = new ClaimsIdentity(claims),
             Expires = DateTime.Now.AddDays(7),
             SigningCredentials = creds
           };

           JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

           var token = tokenHandler.CreateToken(tokenDescriptor);//creates a JSON Web Token
           
           return tokenHandler.WriteToken(token);
        }
    }
}
