using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class UpdateTaskDetailsUseCase : IUpdateTaskDetailsUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;

        public UpdateTaskDetailsUseCase(ICurrentUserPort currentUserPort)
        {
            _currentUserPort = currentUserPort;
        }
    }
}
