using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;

namespace AppPueblosMagicos
{
    public class FuncionesCortana
    {
        /// <summary>
        /// RegistrarComandos => Funcion para registrar commands
        /// </summary>
        public static async void RegistrarComandos()
        {
            StorageFile storageFile = await Package.Current.InstalledLocation.GetFileAsync(@"CustomVoiceCommandDefinitions.xml");

            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);



        }
    }
}
