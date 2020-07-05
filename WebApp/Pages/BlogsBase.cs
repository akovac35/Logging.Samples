// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using global::Shared.Blogs;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace WebApp.Pages
{
    public class BlogsBase : ComponentBase, IDisposable
    {
        public BlogService blogService { get; set; }

        [Inject] public BlogServiceFactory blogServiceFactory { get; set; }

        [Inject] public ILoggerFactory loggerFactory { get; set; }

        [Inject] public ILogger<BlogsBase> logger { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Action<DbContextOptionsBuilder<BlogContext>> builderConfig = (DbContextOptionsBuilder<BlogContext> builder) => builder.UseSqlite("DataSource =:memory: ");
            blogService = blogServiceFactory.CreateInstance(builderConfig, loggerFactory);
        }

        public void AddNewRandomBlog()
        {
            logger.Here(l => l.Entering());

            blogService.Add(Guid.NewGuid().ToString());
            // Will automatically use transaction
            blogService.Context.SaveChanges();

            this.StateHasChanged();

            logger.Here(l => l.Exiting());
        }

        private bool disposedValue = false;// To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    blogService?.Dispose();
                    blogService = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
