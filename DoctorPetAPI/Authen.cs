using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace DoctorPetAPI
{
    public class Authen
    {
        public Authen()
        {
            
        }

        public string GetIdFromToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (token.StartsWith("Bearer "))
                {
                    token = token.Replace("Bearer ", "");

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);

                    var userIdClaim = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "UserID");

                    if (userIdClaim != null)
                    {
                        return userIdClaim.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
