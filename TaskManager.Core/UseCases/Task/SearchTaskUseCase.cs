using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class SearchTaskUseCase : ISearchTaskUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ISearchTaskPort _searchTaskPort;
        public SearchTaskUseCase(ISearchTaskPort searchTaskPort, ICurrentUserPort currentUserPort)
        {
            _searchTaskPort = searchTaskPort;
            _currentUserPort = currentUserPort;
        }
    }
}
