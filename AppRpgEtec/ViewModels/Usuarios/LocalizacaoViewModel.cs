using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Models;
using System.Collections.ObjectModel;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class LocalizacaoViewModel : BaseViewModel
    {
        private UsuarioService uService;
        public LocalizacaoViewModel()
        {
            MeuMapa = new Map();

            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService= new UsuarioService(token);
        }
        private Map meuMapa;
        public Map MeuMapa
        {
            get => meuMapa;
            set
            {
                meuMapa = value;
                OnPropertyChanged();
            }
        }


        public async void LocalizarEscola()
        {
            try
            {
                Location location = new Location(-23.5200241d, -86.596498d);
                Pin pinEtec = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Etec Horácio",
                    Address = "Rua alcântara, 113,Vila Guilherme",
                    Location = location
                };
                MeuMapa.Pins.Add(pinEtec);
                OnPropertyChanged(nameof(MeuMapa));

                MeuMapa.MoveToRegion(MapSpan
                    .FromCenterAndRadius(location, Distance.FromMeters(500)));
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage
                    .DisplayAlert("Erro", ex.Message, "Ok");
            }
        }


        public async void ExibirUsuariosNoMapa()
        {
            try
            {
                ObservableCollection<Usuario> ocUsuarios = await uService.GetUsuariosAsync();
                List<Usuario> listaUsuarios = new List<Usuario>(ocUsuarios);
                Map map = new Map();

                foreach (Usuario u in listaUsuarios)
                {
                    if (u.Latitude != null && u.Longitude != null)
                    {
                        double latitude = (double)u.Latitude;
                        double longitude = (double)u.Longitude;
                        Location location = new Location(latitude, longitude);

                        Pin pinAtual = new Pin()
                        {
                            Type = PinType.Place,
                            Label = u.Username,
                            Address = $"E-mail: {u.Email}",
                            Location = location
                        };
                        map.Pins.Add(pinAtual);
                    }
                }
                MeuMapa = map;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

    }
}