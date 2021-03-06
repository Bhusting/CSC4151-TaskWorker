using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CSC1451_TaskWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly EventLog _eventLog;

        public Worker(ILogger<Worker> logger, EventLog eventLog)
        {
            _logger = logger;
            _eventLog = eventLog;
        }
        
        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Version 1.0");
            await _eventLog.AddEvent(new Domain.Task() { TaskId = Guid.Empty, Channel = Guid.Empty, TaskName = "Test", EndTime = "22:51" });

            while (!stoppingToken.IsCancellationRequested)
            {
                if (await _eventLog.CheckTime())
                {
                    await _eventLog.TriggerEvent();
                }

                await System.Threading.Tasks.Task.Delay(500, stoppingToken);
            }
        }
    }
}
