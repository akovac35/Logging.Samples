// Author: Aleksander Kovač

using com.github.akovac35.Logging;
using global::Shared.Blogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using static com.github.akovac35.Logging.LoggerHelper<WebApp.Pages.BlogsBase>;

namespace WebApp.Pages
{
    public class BlogsBase : ComponentBase
    {
        [Inject] public IHttpContextAccessor httpContextAccessor { get; set; }

        // Object is disposed of when we navigate away from the view
        [Inject] public BlogService blogService { get; set; }

        public void AddNewRandomBlog()
        {
            Here(l => l.Entering());

            blogService.Add(Guid.NewGuid().ToString());
            // Will automatically use transaction
            blogService.Context.SaveChanges();

            this.StateHasChanged();

            Here(l => l.Exiting());
        }
    }
}
