using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.ViewModels;
using AppRpgEtec.Views.Armas;
using AppRpgEtec.Views.Personagens;
using System.Text;

namespace AppRpgEtec;

public partial class AppShell : Shell
{
    AppShellViewModel viewModel;
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("cadPersonagemView", typeof(CadastroPersonagemView));
        Routing.RegisterRoute("cadArmaView", typeof(CadastroArmaView));

        viewModel = new AppShellViewModel();        
        BindingContext = viewModel;

        string login = Preferences.Get("UsuarioUsername", string.Empty);
		lblLogin.Text = $"Login: {login}";
    }

    
}


