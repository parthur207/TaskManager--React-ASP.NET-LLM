using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class GetUserSpacesDetailsAdapter : IGetUserSpacesDetailsPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        public GetUserSpacesDetailsAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<SpaceEntity>> ExecuteAsync(Guid userId, Guid spaceId)
        {
            var Response = new ResponseModel<SpaceEntity>();
          
            try
            {
                if (!await _context.Space
                .AnyAsync(x => x.Id == spaceId))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Espaço não encontrado.";
                    return Response;
                }

                if (!await _context.SpaceMember.AnyAsync(x => x.UserId == userId))
                {
                    Response.Message = "Nenhum espaço foi encontrado.";
                    Response.Status = Core.Enums.ResponseStatusEnum.NotFound;
                    return Response;
                }

                var responseCache = await _cachingPort.GetAsync<SpaceEntity>($"spaceUser_{spaceId}_{userId}");

                if (responseCache != null)
                {
                    Response.Content = responseCache;
                    Response.Status = ResponseStatusEnum.Success;
                    return Response;
                }

                var space= await _context.Space
                    .Include(x => x.Tasks)
                    .Include(x => x.Members)
                    .Include(x => x.TaskCategories)
                    .Where(x => x.Id == spaceId)
                    .FirstOrDefaultAsync();

                await _cachingPort.SetAsync($"spaceUser_{spaceId}_{userId}", space, TimeSpan.FromMinutes(5));

                Response.Content = space;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("Ocorreu um erro inesperado ao tentar obter os detalhes dos espaços do usuário.");
            }
        }
    }
}
