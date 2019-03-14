using AppPueblosMagicos.Models;
using AppPueblosMagicos.ViewModels;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
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
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppPueblosMagicos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPuebloPage : Page
    {
        public EditPuebloPage()
        {
            this.InitializeComponent();
        }
        public Pueblo PuebloSeleccionado;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = (Pueblo)e.Parameter;
            PuebloSeleccionado = parameters;
            txtNombrePueblo.Text = parameters.Name;
            txtDescripcionFrances.Text = parameters.DescripcionFrench;
            txtpalespanol.Text = parameters.DescripcionEspanish;
            txtEstado.Text = parameters.Estado;

            StorageFolder picturesFolder = KnownFolders.MusicLibrary;
            StorageFile file=null;
            if(!string.IsNullOrEmpty(parameters.URLImage))
            {
                file = await GetFileAsync(picturesFolder, @"PueblosMagicos\" + parameters.URLImage);
                //file = await GetFileAsync(picturesFolder, @"PueblosMagicos\mexico.png");


            }
          

            var getStream = await file.OpenReadAsync();
            var result = new BitmapImage();
            result.SetSource(getStream);
            imgPueblo.Source = result;
        }

        public async Task<StorageFile> GetFileAsync(StorageFolder folder, string filename)
        {
            StorageFile file = null;
            if(folder != null)
            {
                file = await folder.GetFileAsync(filename);
            }
            return file;
        }


        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Frame.GoBack();
            }
            catch(Exception)
            {

                throw;
            }
        }

        private async void btnMicFrench_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Windows.Media.SpeechRecognition.SpeechRecognizer speechRecognizer =
               new Windows.Media.SpeechRecognition.SpeechRecognizer(new Windows.Globalization.Language("fr"));     // se le puede pasar parámetro de idiioma, ahoria agarra el del sisytem
                await speechRecognizer.CompileConstraintsAsync();
                Windows.Media.SpeechRecognition.SpeechRecognitionResult resultado =
                     await speechRecognizer.RecognizeWithUIAsync();
                txtDescripcionFrances.Text = resultado.Text;
            }
            catch(Exception ex)
            {

            }
        }

        private void btnDescEspaniol_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNombrePueblo.Text)
                || string.IsNullOrEmpty(txtpalespanol.Text)
                || string.IsNullOrEmpty(txtDescripcionFrances.Text)
                || string.IsNullOrEmpty(txtEstado.Text))
            {
                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Informacion",
                    Content = "Necesitas ingresar todos los datos del nuevo Pueblo",
                    CloseButtonText = "OK"
                };
                await contentDialog.ShowAsync();
                return;
            }
            

            ObservableCollection<Pueblo> ListaAGuardar = new ObservableCollection<Pueblo>();
            

            ListaAGuardar = new DashboardViewModel().ListaPueblosMagicosSinMostrar;

            ListaAGuardar.Where(x => x.PuebloID == PuebloSeleccionado.PuebloID).FirstOrDefault().DescripcionEspanish = txtpalespanol.Text;
            ListaAGuardar.Where(x => x.PuebloID == PuebloSeleccionado.PuebloID).FirstOrDefault().DescripcionFrench = txtDescripcionFrances.Text;
            ListaAGuardar.Where(x => x.PuebloID == PuebloSeleccionado.PuebloID).FirstOrDefault().Estado = txtEstado.Text;
            ListaAGuardar.Where(x => x.PuebloID == PuebloSeleccionado.PuebloID).FirstOrDefault().Name = txtNombrePueblo.Text;
            ListaAGuardar.Where(x => x.PuebloID == PuebloSeleccionado.PuebloID).FirstOrDefault().URLImage = URLImage;


            //ListaAGuardar.Add(palabra);

            try
            {

                string output = JsonConvert.SerializeObject(ListaAGuardar);
                DataAccess.AddData(output);

                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Pueblo Actualizado Correctamente",
                    Content = "Se actualizo el pueblo correctamente",
                    CloseButtonText = "OK"
                };
                await contentDialog.ShowAsync();
            }
            catch(Exception ex)
            {

            }


        }

        private void btnFrenToEsp_Click(object sender, RoutedEventArgs e)
        {
            string certificado = ObtenerCertificado();

            GoogleCredential googleCredential = GoogleCredential.FromJson(certificado);
            TranslationClient client = TranslationClient.Create(googleCredential, TranslationModel.ServiceDefault);
            var response = client.TranslateText(txtpalespanol.Text, "es");
            txtpalespanol.Text = response.TranslatedText;
        }

        private void btnEspToFrench_Click(object sender, RoutedEventArgs e)
        {
            string certificado = ObtenerCertificado();

            GoogleCredential googleCredential = GoogleCredential.FromJson(certificado);
            TranslationClient client = TranslationClient.Create(googleCredential, TranslationModel.ServiceDefault);
            var response = client.TranslateText(txtpalespanol.Text, "fr");
            txtDescripcionFrances.Text = response.TranslatedText;
        }

        private async void btnMicEspaniol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Windows.Media.SpeechRecognition.SpeechRecognizer speechRecognizer =
                new Windows.Media.SpeechRecognition.SpeechRecognizer(new Windows.Globalization.Language("es-MX"));     // se le puede pasar parámetro de idiioma, ahoria agarra el del sisytem
                await speechRecognizer.CompileConstraintsAsync();
                Windows.Media.SpeechRecognition.SpeechRecognitionResult resultado =
                     await speechRecognizer.RecognizeWithUIAsync();
                txtpalespanol.Text = resultado.Text;
            }
            catch(Exception)
            {

                throw;
            }

        }

        private string ObtenerCertificado()
        {
            string contenido = string.Empty;

            try
            {
                contenido = "{'type': 'service_account','project_id': 'mtwdm2018-1549677159474','private_key_id': '36aca600486d6dd7bf8aa3700dd79538907c3ee8','private_key': '-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCyFNl52aPr1Avp\nXGnfCLVZy5HFycMX0Eb6xUq+bxiKXbivlgRwbma1cjSaUHXjc6oyBBzn/9q8Iap1\nt60yYLKyC0TWPoYc+DNvo8MEkKppAc7MoWAKlqcLDdEd+iV5gEM+2YHNZCRzfRgu\nrHUzK3fkVBFcxjfW/0SeY7p5xRxtPZu8W86EQuk1rR1ZAzlZIwQodHdG1TX/m9H2\nuZMvtoi3D6kwMFksOwdTvWlBn16ng9t3XU5547D6u4ovhaI5cvwsHEQv0DPh6lmo\n2dteRBDe2hY20RIMoUjaIdxumcjUzTR5NeYVaAf2jkoQAMD+ATBlbyFUVGo/0DO8\nNv8f3QhNAgMBAAECggEAFxLbgM3Bzv8ZEgH+176tnLZjK2DyjcXXRIvGnPkREXCG\nSv3hkl3DohPrm+j79V4ZucRNqIO+qCymhP0pDEN6M9aA80+DmgJQy9DIpnFGGzf4\nPwxTwNt3RlfidgNg0qbbT6voBSBKFsqpFPcUcm5Z6PQ5ka8/MfS+Q9WBJmDzR3YC\nEknad3NqSqBO0SqRP/3NxkoPjwe2ZwwWHuu2ZTyvetp15OpyywP3yCW3fCrlQptb\nZoa/52/LnKaNsMJ+02uKaku/3kRNh2aoZE/IEuFUZ7Mx50jryoGq9J8xMoMo8u7x\nXsGJO37e/a1ONHD7urEKrTlLjk0eNle/ozwY9EotUwKBgQDngReh6eWL5l4sL9e2\n6jKFtSyEZK/GvGShV4U7NSIaskLDb5ojvvrecezdpUxZwVIXNiRAj5cqu+HxAJBn\nWBcH4mQnXyYkiLbfmB7QMpVgaYu3BnYAwyVJZkdCc1NFvqmocddbbbkv/gh+K6IN\nJP37vuvY6xI9XJhmewA+wKnh1wKBgQDE7Kh/lzfDTG0fJu1sMpT46GkCqbtFbLbg\nN66VXHt51eRnwNkGZbSv4WSBucRoPHGW2xACnogQmGoC143KU3j91azkg3sUra9F\nGLrKdBms2suxbsRXYgLGVC3gCfJXMEAHCnZX+cHaK43N47N1ObfuO3V3e+osa+Tp\nN6KGLMnqewKBgQCdH0Cq49Sn3vKLiu0deFZR6WUNdkjW2YZy+rOyO52qANLPUi+L\nk1MxJqFczZPEVzEgD98K8mnm1x3CNF/NxDvdXgobrrh0k1WK6/P92lcH2Jq63ee5\nHLlx17kFoMAj1gPQD3Pa2d2WdRPOjk6uHS1Eb5Ai6Wi3vOCyrUi9ToX+gQKBgE8+\nwSvfKYSBC+SeYKrKzCJCPIfiz8bHUex2292lQtcrmOebtnoZkZW5iR2fKQedU0SW\n0SGMtEqhWv/byGZkIutbAmFO+8e9gSu4IOr5v59MyO2VGpPjkCRJmdBvkEM/2nQ6\n5JbQng7yufThrcT9viOzb7jud7T6kjq7tb5y5apjAoGACZ3gwoN7oaYzr1lzGOfX\n8qC+ZElboyDKDxt6CzpLJxA/EeIl2guzWIGbPOVYMXR8tquZo4ysfVEd59wyO6jI\nLdGF9Stk6UeZczZUGlNbOFiFXY+utp2YtTjCYmh4qS/fURoVuoU+Ll4LUfD3St6I\njrAwC7EeK0Qxva6TKbPAzt8=\n-----END PRIVATE KEY-----\n','client_email': 'starting-account-qg9w2gxjm10l@mtwdm2018-1549677159474.iam.gserviceaccount.com','client_id': '103055109065943811322','auth_uri': 'https://accounts.google.com/o/oauth2/auth','token_uri': 'https://oauth2.googleapis.com/token','auth_provider_x509_cert_url': 'https://www.googleapis.com/oauth2/v1/certs','client_x509_cert_url': 'https://www.googleapis.com/robot/v1/metadata/x509/starting-account-qg9w2gxjm10l%40mtwdm2018-1549677159474.iam.gserviceaccount.com'}";

            }
            catch(Exception ex)
            {
                contenido = string.Empty;
            }

            return contenido;
        }

        public string URLImage;
        private async void btnSeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if(file != null)
            {
                // Application now has read/write access to the picked file
                URLImage = file.Name;
                using(var randomAccessStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var result = new BitmapImage();
                    await result.SetSourceAsync(randomAccessStream);
                    imgPueblo.Source = result;

                }


            }
            else
            {
            }
        }
    }
}
