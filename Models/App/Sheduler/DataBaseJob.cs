using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Sheduler
{
    public class DataBaseJob: IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DataBaseJob(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        } // DataBaseJob

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dataBaseService = scope.ServiceProvider.GetService<IDataBaseService>();

                // Выполнение задач сервиса
                await dataBaseService.CheckImages();
                await dataBaseService.CheckProposals();
                await dataBaseService.CheckRequisitions();
                await dataBaseService.CheckMessages();
            }
        } // Execute
    }
}
