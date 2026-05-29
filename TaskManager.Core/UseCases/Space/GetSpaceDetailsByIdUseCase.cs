using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class GetSpaceDetailsByIdUseCase : IGetSpaceDetailsByIdUseCase
    {

        public async Task<ResponseModel<SpaceDTO>> GetSpaceDetailsById(int spaceId)
        {
            throw new NotImplementedException();
        }
    }
}
