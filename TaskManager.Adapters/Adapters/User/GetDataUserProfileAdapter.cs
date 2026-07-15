using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Caching;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.ValueObjects;

namespace TaskManager.Adapters.Adapters.User
{
    public class GetDataUserProfileAdapter : IGetDataUserProfilePort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;

        public GetDataUserProfileAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<UserEntity>> ExecuteAsync(Guid userId)
        {
            var Response= new ResponseModel<UserEntity>();
            try
            {
                if (!await _context.User.AnyAsync(x=>x.Id==userId))
                {
                    Response.Message= "Usuário não encontrado";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var responseCache = await _cachingPort.GetAsync<UserEntity>($"userProfile_{userId}");

                if (responseCache != null)
                {
                    Response.Status = ResponseStatusEnum.Success;
                    Response.Content = responseCache;
                    return Response;
                }

                var user = await _context
                    .User
                    .Where(x => x.Id == userId)
                    .Include(x => x.Spaces)
                        .ThenInclude(x=>x.Space)
                    .FirstOrDefaultAsync();

                await _cachingPort
                    .SetAsync($"userProfile_{userId}", user, TimeSpan.FromMinutes(5));

                Response.Content = user;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false,"Erro: " +ex.Message);
                throw new Exception("Ocorreu um erro inesperado");
            }
        }
    }
}
