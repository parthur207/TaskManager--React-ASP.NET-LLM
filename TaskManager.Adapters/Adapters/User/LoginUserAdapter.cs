using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.User;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.User
{
    public class LoginUserAdapter : ILoginUserPort
    {
        private readonly DbContextTaskManager _context;
        private 
        public LoginUserAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public Task<ResponseModel<string>> ExecuteAsync(LoginRequestModel model)
        {
            var Response= new ResponseModel<string>();
            try
            {

            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
