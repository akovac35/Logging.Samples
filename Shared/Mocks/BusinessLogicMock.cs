// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Shared.Mocks
{
    public class BusinessLogicMock<T>
    {
        public BusinessLogicMock(ILoggerFactory loggerFactory = null)
        {
            _logger = (loggerFactory ?? LoggerFactoryProvider.LoggerFactory).CreateLogger<BusinessLogicMock<T>>();

            _logger.Here(l => l.Entering());

            _logger.Here(l => l.Exiting());
        }

        private ILogger _logger;

        public static Task<int> GetTaskInstance()
        {
            var tmp = new BusinessLogicMock<T>();
            return tmp.FirstLevelAsync((new Random()).Next(0, 3000));
        }

        public int FirstLevel(int delay = 3000)
        {
            _logger.Here(l => l.Entering(delay));

            SecondLevel(delay);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public int SecondLevel(int delay)
        {
            _logger.Here(l => l.Entering(delay));

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public async Task<int> FirstLevelAsync(int delay = 3000)
        {
            _logger.Here(l => l.Entering(delay));

            await SecondLevelAsync(delay).ConfigureAwait(false);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public async Task<int> SecondLevelAsync(int delay)
        {
            _logger.Here(l => l.Entering(delay));

            await ThirdLevelAsync(delay).ConfigureAwait(false);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }

        public async Task<int> ThirdLevelAsync(int delay)
        {
            _logger.Here(l => l.Entering(delay));

            await Task.Delay(delay).ConfigureAwait(false);

            _logger.Here(l => l.Exiting(delay));
            return delay;
        }
    }
}
