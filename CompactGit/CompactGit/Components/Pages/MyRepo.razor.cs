/**
using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CompactGit.Components.Pages
{
    public partial class MyRepo : ComponentBase
    {
        public string FindInput { get; set; } = "";
        public RepoDb.RepoDbContext? Context { get; set; }
        public List<RepoDb.Repo> RepoList { get; set; } = new List<RepoDb.Repo>();

        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IDbContextFactory<RepoDb.RepoDbContext> DbContextFactory { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Context = DbContextFactory.CreateDbContext();
            await Context!.Repos.LoadAsync();

            RepoList = Context!.Repos.Where(x=>x.UserId == UserUrl).ToList();

            await base.OnInitializedAsync();
        }

        private void NewButtonClickAsync()
        {
            NavigationManager.NavigateTo("/create-repo");
        }

        private void TypeButtonClickAsync()
        {

        }

        private void SettingsButtonClickAsync(MouseEventArgs e)
        {
            NavigationManager.NavigateTo("/settings/" + UserUrl);
        }

        private void ColumnButtonClickAsync(MouseEventArgs e)
        {
        }

        private void RepositoryClickAsync(string name)
        {
            NavigationManager.NavigateTo(UserUrl + "/" + name);
        }
    }
}**/

using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CompactGit.Components.Pages
{
    public partial class MyRepo : ComponentBase
    {
        private bool showTypeDropdown = false;
        private string FindInput { get; set; } = "";
        private List<RepoDb.Repo> RepoList { get; set; } = new List<RepoDb.Repo>();
        private List<RepoDb.Repo> FilteredRepoList { get; set; } = new List<RepoDb.Repo>();

        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IDbContextFactory<RepoDb.RepoDbContext> DbContextFactory { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            using var context = DbContextFactory.CreateDbContext();
            await context.Repos.LoadAsync();

            RepoList = context.Repos.Where(x => x.UserId == UserUrl).ToList();
            FilteredRepoList = new List<RepoDb.Repo>(RepoList);

            await base.OnInitializedAsync();
        }

        private void NewButtonClickAsync()
        {
            NavigationManager.NavigateTo("/create-repo");
        }

        private void ToggleTypeDropdown()
        {
            showTypeDropdown = !showTypeDropdown;
        }

        private void FilterRepos(bool isPublic)
        {
            FilteredRepoList = RepoList.Where(repo => repo.IsPublic == isPublic).ToList();
            showTypeDropdown = false;
        }

        private void SettingsButtonClickAsync()
        {
            NavigationManager.NavigateTo("/settings/" + UserUrl);
        }

        private void RepositoryClickAsync(string name)
        {
            NavigationManager.NavigateTo(UserUrl + "/" + name);
        }
    }
}
