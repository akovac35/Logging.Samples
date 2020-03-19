// Author: Aleksander Kovač

using com.github.akovac35.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Blogs
{
    public class BlogService : IDisposable
    {
        public BlogService(BlogContext context, ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory == null)
            {
                _logger = LoggerFactoryProvider.LoggerFactory.CreateLogger<BlogService>();
            }
            else
            {
                _logger = loggerFactory.CreateLogger<BlogService>();
            }

            _logger.Here(l => l.EnteringSimpleFormat(context));

            Context = context;

            _logger.Here(l => l.Exiting());
        }

        private ILogger _logger;

        public BlogContext Context { get; set; }

        public int Count
        {
            get
            {
                _logger.Here(l => l.Entering());

                var count = Context.Blogs.Count();

                _logger.Here(l => l.Exiting(count));
                return count;
            }
        }

        public void Add(string url)
        {
            _logger.Here(l => l.Entering(url));

            var blog = new Blog { Url = url };
            Context.Blogs.Add(blog);
            Context.SaveChanges();

            _logger.Here(l => l.Exiting());
        }

        public IEnumerable<Blog> Find(string term)
        {
            _logger.Here(l => l.Entering(term));

            var tmp = Context.Blogs
                .Where(b => b.Url.Contains(term))
                .OrderBy(b => b.Url)
                .ToList();

            _logger.Here(l => l.Exiting(tmp));
            return tmp;
        }

        private bool disposedValue = false;

        public void Dispose()
        {
            _logger.Here(l => l.Entering());

            _logger.Here(l => l.LogDebug("{@disposedValue}", disposedValue));

            if (!disposedValue)
            {
                disposedValue = true;

                // We should explicitely manage our explicitly provided connection
                Context?.Database.CloseConnection();
                Context?.Database.GetDbConnection()?.Dispose();
                Context?.Dispose();
            }

            _logger.Here(l => l.Exiting());
        }
    }
}
