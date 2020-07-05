// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Blogs
{
    public class BlogService : IDisposable
    {
        public BlogService(BlogContext context, ILogger<BlogService> logger = null)
        {
            if (logger != null) _logger = logger;
            Context = context;
        }

        private ILogger _logger = NullLogger.Instance;

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

            _logger.Here(l => l.LogDebug("disposedValue: {@0}", disposedValue));

            if (!disposedValue)
            {
                disposedValue = true;

                // We should explicitely manage our explicitly provided connection
                Context?.Database.CloseConnection();
                Context?.Dispose();
                Context = null;
            }

            _logger.Here(l => l.Exiting());
        }
    }
}
