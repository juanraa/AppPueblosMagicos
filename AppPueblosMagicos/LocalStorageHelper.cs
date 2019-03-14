using AppPueblosMagicos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppPueblosMagicos
{
    class LocalStorageHelper
    {
        public static async Task<bool> CreateLocalFile()
        {
            try
            {
                // Create sample file; replace if exists.
                Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile =
                    await storageFolder.CreateFileAsync("pueblosmagicos.txt",
                        Windows.Storage.CreationCollisionOption.ReplaceExisting);
                return true;
            }
            catch(Exception)
            {

                return false;
            }

        }

        public static async void WriteLocalFile(string jsonString)
        {
            Windows.Storage.StorageFolder storageFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =
                await storageFolder.GetFileAsync("pueblosmagicos.txt");

            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, jsonString);

        }

        public static async Task<string> ReadLocalFile()
        {
            Windows.Storage.StorageFolder storageFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =
                await storageFolder.GetFileAsync("pueblosmagicos.txt");

            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

            return text;

        }

        public static string JsonStringEstado(string Estado)
        {
            string Key = "ListaPueblos" + Estado;
            string listaRecuperada= AppSettings.localSettings.Values[Key].ToString();
            return listaRecuperada;

        }

        public static void SaveJsonStringEstado(string JsonToSave,string Estado)
        {
            string Key = "ListaPueblos" + Estado;
            ApplicationData.Current.LocalSettings.Values[Key] = JsonToSave;


        }
        public static void SaveForState(Pueblo pueblo)
        {
            var jsonString = JsonStringEstado(pueblo.Estado);
            var list1 = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(jsonString, typeof(ObservableCollection<Pueblo>));
            list1.Add(pueblo);
            string output = JsonConvert.SerializeObject(list1);

            SaveJsonStringEstado(output, pueblo.Estado);
            
        }

        public static ObservableCollection<Pueblo> GetAllPueblos()
        {
            ObservableCollection<Pueblo> listReturn = new ObservableCollection<Pueblo>();
            string listaRecuperada1 = AppSettings.localSettings.Values["ListaPueblosJalisco"].ToString();
            string listaRecuperada2 = AppSettings.localSettings.Values["ListaPueblosAguascalientes"].ToString();
            string listaRecuperada3 = AppSettings.localSettings.Values["ListaPueblosGuanajuato"].ToString();
            string listaRecuperada4 = AppSettings.localSettings.Values["ListaPueblosDF"].ToString();
            string listaRecuperada5 = AppSettings.localSettings.Values["ListaPueblosVeracruz"].ToString();




            var list1 = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada1, typeof(ObservableCollection<Pueblo>));
            var list2 = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada2, typeof(ObservableCollection<Pueblo>));
            var list3 = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada3, typeof(ObservableCollection<Pueblo>));
            var list4 = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada4, typeof(ObservableCollection<Pueblo>));
            var list5 = (ObservableCollection<Pueblo>)JsonConvert.DeserializeObject(listaRecuperada5, typeof(ObservableCollection<Pueblo>));

            foreach(var item in list1)
            {
                listReturn.Add(item);
            }
            foreach(var item in list2)
            {
                listReturn.Add(item);
            }
            foreach(var item in list3)
            {
                listReturn.Add(item);
            }
            foreach(var item in list4)
            {
                listReturn.Add(item);
            }
            foreach(var item in list5)
            {
                listReturn.Add(item);
            }
            return listReturn;
        }
    }
    }
