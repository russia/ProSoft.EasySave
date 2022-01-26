using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Application.Interfaces.Services
{
    public interface IJobFactoryService
    {
        void LoadConfiguration();

        void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);

        Task<IReadOnlyCollection<JobResult>> StartJobsAsync(int count, ExecutionType executionType);
    }
}