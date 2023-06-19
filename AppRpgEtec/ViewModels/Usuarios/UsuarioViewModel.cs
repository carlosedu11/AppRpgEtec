using AppRpgEtec.Helpers.Message;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Views.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {

        //ctor + TAB + TAB: Atalho para criar o construtor
        public UsuarioViewModel()
        {
            uService = new UsuarioService();
            InicializarCommands();
        }

        private UsuarioService uService;
        public ICommand RegistrarCommand { get; set; }
        public ICommand AutenticarCommand { get; set; }
        public ICommand DirecionarCadastroCommand { get; set; }

        public void InicializarCommands()
        {
            RegistrarCommand = new Command(async ()=> await RegistrarUsuario());
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }
                

        #region AtributosPropriedades
        //As propriedades serão chamadas na View futuramente

        private string login = string.Empty;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string senha = string.Empty;
        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region Métodos
        public async Task RegistrarUsuario()//Método para registrar um usuário     
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Senha;

                Usuario uRegistrado = await uService.PostRegistrarUsuarioAsync(u);

                if (uRegistrado.Id != 0)
                {
                    string mensagem = $"Usuário Id {uRegistrado.Id} registrado com sucesso.";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");
                    
                    await Application.Current.MainPage
                        .Navigation.PopAsync();//Remove a página da pilha de visualização
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }


        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task AutenticarUsuario()//Método para autenticar um usuário     
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Senha;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-vindo(a) {uAutenticado.Username}.";

                    //Guardando dados do usuário para uso futuro
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);
                    
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < uAutenticado.Foto.Length; i++)
                    {
                        builder.Append(uAutenticado.Foto[i].ToString("x2"));
                    }

                    Preferences.Set("UsuarioFoto", builder.ToString());

                    Models.Email email = new Models.Email();
                    email.Remetente = "luizfernando987@gmail.com";
                    email.RemententePassword = "xtpechxinxlmxnmo";
                    email.Destinatario = "luizfernando987@gmail.com";
                    email.DominioPrimario = "smtp.gmail.com";
                    email.PortaPrimaria = 587;
                    email.Assunto = "Notificação de acesso";
                    email.Mensagem = $"Usuário {u.Username} acessou o aplicativo" +
                        $" em {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                    EmailHelper emailHelper = new EmailHelper(); //using AppRpgEtec.Helpers.Message
                    //await emailHelper.EnviarEmail(email);

                    //Inicio parte 4
                    _isCheckingLocation= true;
                    _cancelTokenSource= new CancellationTokenSource();
                    GeolocationRequest request =
                        new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                    Location location = await Geolocation
                        .Default.GetLocationAsync(request, _cancelTokenSource.Token);

                    Usuario uLoc = new Usuario();
                    uLoc.Id = uAutenticado.Id;
                    uLoc.Latitude = location.Latitude;
                    uLoc.Longitude = location.Longitude;

                    UsuarioService uServiceLoc = new UsuarioService(uAutenticado.Token);
                    await uServiceLoc.PutAtualizarLocalizacaoAsync(uLoc);
                    //Fim da coleta de geolocalização atual para Atualizção na API

                    await Application.Current.MainPage
                            .DisplayAlert("Informação", mensagem, "Ok");

                    Application.Current.MainPage = new AppShell();                    
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados incorretos :(", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }




        public async Task DirecionarParaCadastro()//Método para exibição da view de Cadastro      
        {
            try
            {
                await Application.Current.MainPage.
                    Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }




        #endregion




    }






}
