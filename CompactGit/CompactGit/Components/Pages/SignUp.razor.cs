using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CompactGit.Components.Pages
{
    public partial class SignUp : ComponentBase, IDisposable
    {
        private string id = "";
        private string passwd = "";
        private string passwdRe = "";
        private string nickname = "";
        private string email = "";

        public string Id { get => id; set { id = Regex.Replace(value, "[^0-9a-zA-Z._-]", ""); } }
        public string Passwd { get => passwd; set { passwd = Regex.Replace(value, "[^\x20-\x7E]", ""); } }
        public string PasswdRe { get => passwdRe; set { passwdRe = Regex.Replace(value, "[^\x20 -\x7E]", ""); } }
        public string Nickname { get => nickname; set { nickname = Regex.Replace(value, "[^0-9a-zA-Z._-]", ""); } }
        public string Email { get => email; set { email = Regex.Replace(value, "[^0-9a-zA-Z._@-]", ""); } }
        public string ErrorMsg { get; set; } = "";
        public GitDb.GitDbContext? Context { get; set; }

        [Inject]
        public IDbContextFactory<GitDb.GitDbContext> DbFactory { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Context = DbFactory.CreateDbContext();
            await base.OnInitializedAsync();
        }

        private string PassHashing(string pass)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(pass)));
        }

        private async void SignUpButtonClick(MouseEventArgs e)
        {
            if (Passwd != PasswdRe || Id == "" || Passwd == "" || Nickname == "" || Email == "")
            {
                ErrorMsg = "There are space that's not filled in";
                return;
            }

            await Context!.Users.LoadAsync();
            Context!.Users.Add(new GitDb.User()
            {
                Id = Id,
                Pw = PassHashing(Passwd),
                Nickname = Nickname,
                Email = Email,
            });
            await Context.SaveChangesAsync();

            NavigationManager.NavigateTo("/");
        }

        void IDisposable.Dispose()
        {
            Context?.Dispose();
        }
    }
}
