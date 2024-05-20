using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CompactGit.Components.Pages
{
	public partial class Settings
	{
		public bool IsNotMyProfile = false;

		[Parameter]
		public string UserUrl { get; set; } = default!;

		[Inject]
		public NavigationManager NavigationManager { get; set; } = default!;

		[Inject]
		public ICookie Cookie { get; set; } = default!;

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			string loginCookie = await Cookie.GetValue("login");

			if (loginCookie != UserUrl)
			{
				IsNotMyProfile = true;
				StateHasChanged();
			}

			await base.OnAfterRenderAsync(firstRender);
		}

		private string FindInput { get; set; } = "";

		private void ImageSaveButtonClick(MouseEventArgs e)
		{
			/*
			if (FindInput.Contains("/") == true)
				NavigationManager.NavigateTo("/repos/" + FindInput);

			else
				NavigationManager.NavigateTo("/user/" + FindInput);
			*/
		}

		private void SaveButtonClick(MouseEventArgs e)
		{
			NavigationManager.NavigateTo("/user/" + UserUrl);
		}

		private async Task CancelButtonCLick(MouseEventArgs e)
		{
			/*
			string msg = await Cookie.GetValue("login");

			if (msg != UserUrl)
			{
				return;
			}
			*/
			NavigationManager.NavigateTo("/user/" + UserUrl);
		}
	}
}