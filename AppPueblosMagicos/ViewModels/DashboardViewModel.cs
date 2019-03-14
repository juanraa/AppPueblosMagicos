using AppPueblosMagicos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace AppPueblosMagicos.ViewModels
{
    public class DashboardViewModel
    {
        public ObservableCollection<Pueblo> ListaPueblosMagicos { get; set; } = new ObservableCollection<Pueblo>();
        public ObservableCollection<Pueblo> ListaPueblosMagicosSinMostrar { get; set; } = new ObservableCollection<Pueblo>();


        public Pueblo WordSelected { get; set; }

        public bool IsFirstTime = false;
        public DashboardViewModel()
        {
            if(!IsFirstTime)
            {

                CargarPueblos();
                IsFirstTime = true;

            }
        }

        private async void CargarPueblos()
        {
            //if(await LocalStorageHelper.CreateLocalFile())
            //{

            //ListaPueblosMagicos = new ObservableCollection<Pueblo>()
            //{
            //    new Pueblo() {
            //        PuebloID =1,
            //        Name ="Mazamitla",
            //        DescripcionEspanish ="Lagos de Moreno es un municipio del estado de Jalisco. Se trata de un vasto territorio de gran belleza natural y con muchas comunidades valiosas por su historia, tradición y su cultura.",
            //        DescripcionFrench="",
            //        URLImage ="Assets/mazamitla.png",
            //    Estado="Jalisco"},
            //    new Pueblo() { PuebloID=2, Name="San Sebastian del Oeste",
            //        DescripcionEspanish ="San Sebastián del Oeste es una población que debe su belleza a su esplendoroso pasado minero, que en la época de la Colonia lo pobló con más de 20,000 habitantes que buscaban explotar la riqueza de los yacimientos. ", URLImage="Assets/sansebastian.png"
            //    ,
            //        DescripcionFrench="",Estado ="Jalisco"},
            //    new Pueblo() { PuebloID=3, Name="Tapalpa ", DescripcionEspanish="En medio de un maravilloso paisaje de territorios boscosos, el pueblo jalisciense de Tapalpa aparece con la belleza de lo sencillo y natural.", URLImage="Assets/tapalpa.png"
            //    ,DescripcionFrench="",
            //        Estado ="Jalisco"},
            //    new Pueblo() { PuebloID=4, Name="Tequila", DescripcionEspanish="Una primera impresión que nos puede venir a la mente cuando hablamos de la localidad de Tequila en el estado de Jalisco, es que es un paraíso donde abundan las barricas de tequila.", URLImage="Assets/tequila.png"
            //    ,DescripcionFrench="",
            //        Estado ="Jalisco"},
            //    new Pueblo() { PuebloID=5, Name="Lagos de Moreno", DescripcionEspanish="Mazamitla se encuentra en la región sureste del estado de Jalisco, es un típico poblado de montaña inmerso en la sierra que enmarca el lago de Chapala.", URLImage="Assets/lagosdemoreno.png"
            //    ,DescripcionFrench="",Estado="Jalisco"},
            //    new Pueblo() { PuebloID=6, Name="Talpa", DescripcionEspanish="Talpa de Allende, combina naturaleza y cultura en un espléndido destino turístico, con variadas opciones culinarias, notables creaciones arquitectónicas, cómodos hoteles y muchos atractivos más.", URLImage="Assets/talpa.png",
            //    DescripcionFrench="",Estado="Jalisco"},
            //    new Pueblo() { PuebloID=7, Name="Mascota", DescripcionEspanish="Mascota, Pueblo Mágico del estado de Jalisco, es un sitio de hermosos paisajes con ríos, valles y montañas que sobresalen por su vegetación compuesta por robles, abetos y pinos.", URLImage="Assets/mascota.png",
            //    DescripcionFrench="",Estado="Jalisco"},

            //};

            //}
            //else
            //{
            //    string text = await LocalStorageHelper.ReadLocalFile();
            //    if(!string.IsNullOrEmpty(text))
            //    {

            //        ListaPueblosMagicos = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(text, typeof(ObservableCollection<Pueblo>));

            //    }
            //}


            //if(AppSettings.localSettings.Values.ContainsKey("ListaPueblos"))
            //{
            //    string listaRecuperada = AppSettings.localSettings.Values["ListaPueblos"].ToString();

            //    var list = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada, typeof(ObservableCollection<Pueblo>));
            //    foreach(var item in list)
            //    {
            //        ListaPueblosMagicos.Add(item);
            //    }
            //    ListaPueblosMagicos = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada, typeof(ObservableCollection<Pueblo>));
            //}

            var returnFromdb = DataAccess.GetData();
            if(returnFromdb != null&&returnFromdb.Count>0)
            {
                string listaRecuperada = DataAccess.GetData().LastOrDefault().ContentDB.ToString();

                ListaPueblosMagicosSinMostrar = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada, typeof(ObservableCollection<Pueblo>));

               
            }


            //ListaPueblosMagicos = LocalStorageHelper.GetAllPueblos();
            //if(ApplicationData.Current.LocalSettings.Values.ContainsKey("ListaPueblos"))
            //{
            //    string listaRecuperada = ApplicationData.Current.LocalSettings.Values["ListaPueblos"].ToString();
            //    ListaPueblosMagicos = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada, typeof(ObservableCollection<Pueblo>));
            //}
        }

        



    }
}
