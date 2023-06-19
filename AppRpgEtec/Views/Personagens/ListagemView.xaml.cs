
using AppRpgEtec.Models;
using AppRpgEtec.ViewModels.Personagens;

namespace AppRpgEtec.Views.Personagens;

public partial class ListagemView : ContentPage
{
	ListagemPersonagemViewModel viewModel;
	public ListagemView()
	{
		InitializeComponent();

		viewModel = new ListagemPersonagemViewModel();
		BindingContext = viewModel;
		Title = "Personagens - App Rpg Etec";
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		_ = viewModel.ObterPersonagens();
    }

	private Personagem personagemSelecionado;

	public Personagem PersonagemSelecionado
	{
		get { return PersonagemSelecionado; }
		set
		{
			if (value != null)
			{
				personagemSelecionado = value;

				Shell.Current
					.GoToAsync($"cadPersonagemView?pId={personagemSelecionado.Id}");
			}
		}
	}

	
}