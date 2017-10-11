using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MachinesMonitor.Infrastructure
{
    /// <summary>
    /// In order to avoid typical for web apps issues with long running background tasks, this implementation of 
    /// IHostedService was created. Its not full and anough implementation. Its main purpose is to demonstrate
    /// approach of proper implementation of background processing in web app.
    /// </summary>
    public class PeriodicBackgroundRunner : IHostedService
    {
        private readonly TimeSpan _repeatInterval = TimeSpan.FromMinutes(1);

        private readonly Action _startAction;

        private readonly Action _stopAction;

        public PeriodicBackgroundRunner(Action startAction, Action stopAction)
        {
            startAction = startAction ?? throw new ArgumentNullException($"{nameof(startAction)} should not be null.");
            stopAction = stopAction ?? throw new ArgumentNullException($"{nameof(stopAction)} should not be null.");

            //_repeatInterval = repeatInterval;
            _startAction = startAction;
            _stopAction = stopAction;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(_repeatInterval);
                _startAction();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _stopAction();
        }
    }
}
