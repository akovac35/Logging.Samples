// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Shared.Blogs
{
    public class BlogServiceFactory
    {
        public BlogServiceFactory(ILoggerFactory loggerFactory = null)
        {
            _logger = (loggerFactory ?? LoggerFactoryProvider.LoggerFactory).CreateLogger<BlogServiceFactory>();
        }

        private ILogger _logger;

        public BlogService CreateInstance(Action<DbContextOptionsBuilder<BlogContext>> configureContext, ILoggerFactory loggerFactory = null)
        {
            _logger.Here(l => l.EnteringSimpleFormat(configureContext, loggerFactory));

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<BlogContext>();
            dbContextOptionsBuilder.UseLoggerFactory(loggerFactory);
            // Should provide connection, e.g. dbContextOptionsBuilder.UseSqlite(connection)
            configureContext(dbContextOptionsBuilder);

            var options = dbContextOptionsBuilder.Options;
            var context = new BlogContext(options);
            var blogService = new BlogService(context);

            // In-memory database exists only for the duration of an open connection
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            _logger.Here(l => l.ExitingSimpleFormat(blogService));
            return blogService;
        }
    }
}
