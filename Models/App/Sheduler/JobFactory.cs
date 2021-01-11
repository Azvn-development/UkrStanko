using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Sheduler
{
    public class JobFactory: IJobFactory
    {
        protected readonly IServiceScopeFactory serviceScopeFactory;
        public JobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        } // JobFactory

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            } // using
        } // NewJob

        public void ReturnJob(IJob job) { }
    }
}
