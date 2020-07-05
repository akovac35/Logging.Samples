// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace Shared.Blogs
{
    public class BlogServiceFactory
    {
        public BlogServiceFactory(ILogger<BlogServiceFactory> logger = null)
        {
            if (logger != null) _logger = logger;
        }

        private ILogger _logger = NullLogger.Instance;

        public BlogService CreateInstance(Action<DbContextOptionsBuilder<BlogContext>> configureContext, ILoggerFactory loggerFactory = null)
        {
            _logger.Here(l => l.EnteringSimpleFormat(configureContext, loggerFactory));

            var lf = loggerFactory ?? NullLoggerFactory.Instance;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<BlogContext>();
            dbContextOptionsBuilder.UseLoggerFactory(lf);
            // Should provide connection, e.g. dbContextOptionsBuilder.UseSqlite(connection)
            configureContext(dbContextOptionsBuilder);

            var options = dbContextOptionsBuilder.Options;
            var context = new BlogContext(options);
            var blogService = new BlogService(context, lf.CreateLogger<BlogService>());

            // In-memory database exists only for the duration of an open connection
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            _logger.Here(l => l.ExitingSimpleFormat(blogService));
            return blogService;
        }
    }
}
