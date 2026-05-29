using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.Space.Interfaces
{
    public interface IGetSpaceDetailsByIdUseCase
    {
        Task<ResponseModel<SpaceDTO>> GetSpaceDetailsById(int spaceId);
    }
}
