using InventoryClientAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryClientAPI.Utils
{
    public class Tools
    {
        public static bool ValidateStatusEnum(List<Product> products)
        {
            foreach (var product in products)
            {
                // Validar si el estado del producto es válido
                if (!Enum.IsDefined(typeof(ProductStatus), product.ProductStatus))
                {
                    return false;
                }
            }
            return true;
        }

        public static string GenerateTokenBearer(AppSettings settings)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "Admin")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Token));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(60000), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
