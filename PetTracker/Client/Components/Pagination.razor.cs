using Microsoft.AspNetCore.Components;
using PetTracker.Shared.Models;

namespace PetTracker.Client.Components
{
	public partial class Pagination
	{
        private List<PageItem>? pages;

        [Parameter]
        public int CurrentPage { get; set; }
        [Parameter]
        public int TotalPages { get; set; }
        [Parameter]
        public int Spread { get; set; }
        [Parameter]
        public EventCallback<int> OnSelectedPage { get; set; }

        protected override void OnParametersSet()
        {
            CreatePages();
        }

        private void CreatePages()
        {
            pages = new List<PageItem>();

            var hasPreviosPage = CurrentPage > 1;
            pages.Add(new PageItem(CurrentPage - 1, hasPreviosPage, "Prethodno"));

            for (var i = 1; i <= TotalPages; i++)
            {
                if (i >= CurrentPage - Spread && i < CurrentPage + Spread)
                {
                    pages.Add(new PageItem(i, true, i.ToString())
                    {
                        Active = CurrentPage == i
                    });
                }
            }

            var hasNextPage = CurrentPage < TotalPages;
            pages.Add(new PageItem(CurrentPage + 1, hasNextPage, "Sljedeće"));
        }

        private async Task SelectCurrentPage(PageItem pageItem)
        {
            if (CurrentPage == pageItem.Page || !pageItem.Enabled)
                return;

            CurrentPage = pageItem.Page;
            await OnSelectedPage.InvokeAsync(pageItem.Page);
        }
    }
}
