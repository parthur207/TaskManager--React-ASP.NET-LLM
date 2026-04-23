using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Security;

namespace TaskManager.Adapters.Security
{
    public class HttpCurrentUserAdapter : ICurrentUserPort
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpCurrentUserAdapter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public Guid UserId =>
            Guid.Parse(GetClaim(ClaimTypes.NameIdentifier));

        public string Email =>
            GetClaim(ClaimTypes.Email);

        public RoleUserEnum Role =>
            Enum.Parse<RoleUserEnum>(GetClaim(ClaimTypes.Role));

        private string GetClaim(string type)
        {
            var value = User?.FindFirst(type)?.Value;

            if (string.IsNullOrEmpty(value))
                throw new Exception($"Claim '{type}' não encontrada.");

            return value;
        }
    }

}

