using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompactGit.Components.Pages
{
    public partial class Login : ComponentBase, IDisposable
    {
        public string Id { get; set; } = "";
        public string Passwd { get; set; } = "";
        public string UserUrl { get; set; } = "";
        public string ErrorMsg { get; set; } = "";
        public GitDb.GitDbContext? Context { get; set; }

        [Inject]
        public IDbContextFactory<GitDb.GitDbContext> DbFactory { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Context = DbFactory.CreateDbContext();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                string loginCookie = await Cookie.GetValue("login");

                if (!string.IsNullOrEmpty(loginCookie))
                {
                    NavigationManager.NavigateTo("/user/" + loginCookie);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private string PassHashing(string pass)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(pass)));
        }

        private async Task LoginButtonClick()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Passwd))
            {
                ErrorMsg = "ID or Password is empty";
                return;
            }

            try
            {
                await Context!.Users.LoadAsync();

                var user = Context.Users.FirstOrDefault(x => x.Id == Id && x.Pw == PassHashing(Passwd));

                if (user != null)
                {
                    UserUrl = user.Id;
                    await Cookie.SetValue("login", UserUrl);
                    NavigationManager.NavigateTo("/user/" + UserUrl);
                }
                else
                {
                    ErrorMsg = "Invalid ID or Password";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                ErrorMsg = "An error occurred during login.";
            }
        }

        private void SignUpButtonClick()
        {
            NavigationManager.NavigateTo("/sign-up");
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
