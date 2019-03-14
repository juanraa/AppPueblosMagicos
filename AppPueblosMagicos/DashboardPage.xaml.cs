using AppPueblosMagicos.Models;
using AppPueblosMagicos.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppPueblosMagicos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {
        private string lenguaje = "fr";
        public DashboardViewModel DashboardViewModel;

        public DashboardPage()
        {
            this.InitializeComponent();
            
            lenguaje = "fr";
            btnNativo.IsChecked = false;
            btnFrench.IsChecked = true;

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter.ToString();
            if(string.IsNullOrEmpty(parameters))
            {
                DashboardViewModel = new DashboardViewModel();
                this.DataContext = DashboardViewModel;
                foreach(var item in DashboardViewModel.ListaPueblosMagicosSinMostrar)
                {
                    DashboardViewModel.ListaPueblosMagicos.Add(item);
                }
            }
            else
            {
                if(parameters.Equals("Frances"))
                {
                    DashboardViewModel = new DashboardViewModel();
                    this.DataContext = DashboardViewModel;
                    foreach(var item in DashboardViewModel.ListaPueblosMagicosSinMostrar)
                    {
                        DashboardViewModel.ListaPueblosMagicos.Add(item);
                    }
                    lenguaje = "fr";
                    btnNativo.IsChecked = false;
                    btnFrench.IsChecked = true;
                }else if(parameters.Equals("Nativo"))
                {
                    DashboardViewModel = new DashboardViewModel();
                    this.DataContext = DashboardViewModel;
                    foreach(var item in DashboardViewModel.ListaPueblosMagicosSinMostrar)
                    {
                        DashboardViewModel.ListaPueblosMagicos.Add(item);
                    }
                    lenguaje = "MX";
                    btnNativo.IsChecked = true;
                    btnFrench.IsChecked = false;
                }
                else if(parameters.Equals("Agregar"))
                {
                    DashboardViewModel = new DashboardViewModel();
                    this.DataContext = DashboardViewModel;
                    foreach(var item in DashboardViewModel.ListaPueblosMagicosSinMostrar)
                    {
                        DashboardViewModel.ListaPueblosMagicos.Add(item);
                    }
                    Frame.Navigate(typeof(NewPuebloPage), "NewPueblo");

                }else if(parameters.Equals("EliminarUltimo"))
                {
                    DashboardViewModel = new DashboardViewModel();
                    this.DataContext = DashboardViewModel;
                    foreach(var item in DashboardViewModel.ListaPueblosMagicosSinMostrar)
                    {
                        DashboardViewModel.ListaPueblosMagicos.Add(item);
                    }
                    await EliminarPuebloMagico(DashboardViewModel.ListaPueblosMagicos.LastOrDefault());

                }else if(parameters.Equals("TotalActual"))
                {
                    DashboardViewModel = new DashboardViewModel();
                    this.DataContext = DashboardViewModel;
                    foreach(var item in DashboardViewModel.ListaPueblosMagicosSinMostrar)
                    {
                        DashboardViewModel.ListaPueblosMagicos.Add(item);
                    }
                    try
                    {
                        int TextToSpeechTotal = DashboardViewModel.ListaPueblosMagicos.Count;
                        var saySomething = (Pueblo)list.SelectedItem;
                        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

                        VoiceInformation voiceInformation = (
                            from voice in SpeechSynthesizer.AllVoices
                            where voice.Gender == VoiceGender.Female && voice.Language.Contains(lenguaje)
                            select voice
                            ).FirstOrDefault();

                        speechSynthesizer.Voice = voiceInformation;

                        string palabra = string.Empty;
                        if(lenguaje == "fr")
                        {
                            palabra = "Le chiffre total de Pueblos Magicos agregados es: " + TextToSpeechTotal;
                        }
                        else
                        {
                            palabra = "El numero total de Pueblos Magicos agregados es: "+TextToSpeechTotal;
                        }

                        if(!string.IsNullOrEmpty(palabra))
                        {
                            var speechStream = await speechSynthesizer.SynthesizeTextToStreamAsync(palabra);
                            mediaElement.AutoPlay = true;
                            mediaElement.SetSource(speechStream, speechStream.ContentType);
                            mediaElement.Play();
                        }

                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                }
                else
                {
                    DashboardViewModel = new DashboardViewModel();
                    this.DataContext = DashboardViewModel;

                    var listoDisplay = DashboardViewModel.ListaPueblosMagicosSinMostrar.Where(x => x.Estado == parameters).ToList();
                    foreach(var item in listoDisplay)
                    {
                        DashboardViewModel.ListaPueblosMagicos.Add(item);
                    }
                }
                

            }

        }


        private void btnNativo_Click(object sender, RoutedEventArgs e)
        {
            if(btnNativo.IsChecked == true)
            {
                lenguaje = "MX";
                btnFrench.IsChecked = false;
            }

            if(btnFrench.IsChecked == false)
            {
                btnNativo.IsChecked = true;
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Frame.Navigate(typeof(NewPuebloPage), "NewPueblo");
            }
            catch(Exception)
            {

                throw;
            }
        }

        private void btnFrench_Click(object sender, RoutedEventArgs e)
        {
            var returnFromdb = DataAccess.GetData();

            //DataAccess.DeleteData(returnFromdb.Count);

            if(btnFrench.IsChecked == true)
            {
                lenguaje = "FR";
                btnNativo.IsChecked = false;
            }

            if(btnNativo.IsChecked==false)
            {
                btnFrench.IsChecked = true;
            }
        }
        

        private async void list_ItemClick(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var saySomething = (Pueblo)list.SelectedItem;
                SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
               
                VoiceInformation voiceInformation = (
                    from voice in SpeechSynthesizer.AllVoices
                    where voice.Gender == VoiceGender.Female && voice.Language.Contains(lenguaje)
                    select voice
                    ).FirstOrDefault();

                speechSynthesizer.Voice = voiceInformation;

                string palabra = string.Empty;
                if(lenguaje=="fr")
                {
                    palabra = saySomething.DescripcionFrench;
                }else
                {
                    palabra = saySomething.DescripcionEspanish;
                }

                if(!string.IsNullOrEmpty(palabra))
                {
                    var speechStream = await speechSynthesizer.SynthesizeTextToStreamAsync(palabra);
                    mediaElement.AutoPlay = true;
                    mediaElement.SetSource(speechStream, speechStream.ContentType);
                    mediaElement.Play();
                }

            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(list.SelectedItem!=null)
                {
                    var saySomething = (Pueblo)list.SelectedItem;
                

                    Frame.Navigate(typeof(EditPuebloPage), saySomething);

                }
                
            }
            catch(Exception ex)
            {

                throw;
            }
        }

        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(list.SelectedItem != null)
            {
                var saySomething = (Pueblo)list.SelectedItem;
                await EliminarPuebloMagico(saySomething);


            }
            else
            {
                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Alerta",
                    Content = "Necesitas seleccionar un pueblo de la lista",
                    CloseButtonText = "OK",
                };
                await contentDialog.ShowAsync();
            }


           
        }

        private async Task EliminarPuebloMagico(Pueblo pueblo)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Desea eliminar el Pueblo Magico: " + pueblo.Name + " ?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Si",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(
                "No",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();

        }
        private async void CommandInvokedHandler(IUICommand command)
        {
            if(command.Label == "Si")
            {
                ObservableCollection<Pueblo> ListaAGuardar = new ObservableCollection<Pueblo>();
                ListaAGuardar = new DashboardViewModel().ListaPueblosMagicosSinMostrar;

                var saySomething = (Pueblo)list.SelectedItem;
                var newListPueblos = ListaAGuardar.Where(x => x.PuebloID != saySomething.PuebloID);
                try
                {
                    string output = JsonConvert.SerializeObject(newListPueblos);
                    //ApplicationData.Current.LocalSettings.Values["ListaPueblos"] = output;
                    DataAccess.AddData(output);


                    ContentDialog contentDialog = new ContentDialog()
                    {
                        Title = "Pueblo Eliminado Correctamente",
                        Content = "Se elimino el pueblo de la lista",
                        CloseButtonText = "OK",
                    };
                    await contentDialog.ShowAsync();
                    DashboardViewModel.ListaPueblosMagicos.Remove(saySomething);
                    DashboardViewModel.ListaPueblosMagicosSinMostrar.Remove(saySomething);


                }
                catch(Exception ex)
                {

                }
            }
            else
            {

            }
            // Display message showing the label of the command that was invoked
            //rootPage.NotifyUser("The '" + command.Label + "' command has been selected.",
              //  NotifyType.StatusMessage);
        }
    }
}
