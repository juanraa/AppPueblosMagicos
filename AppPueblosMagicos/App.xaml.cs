using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AppPueblosMagicos
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            DataAccess.InitializeDatabase();

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(DashboardPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();

                FuncionesCortana.RegistrarComandos();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            //Para abrir decir, Escucha, mostrar pueblos magicos de jalisco
            //base.OnActivated(args);
            if(args.Kind == ActivationKind.VoiceCommand)
            {
                VoiceCommandActivatedEventArgs voiceArgs = (VoiceCommandActivatedEventArgs)args;
                SpeechRecognitionResult res = voiceArgs.Result;
                string cmdName = res.RulePath[0];


                string textSpoken = res.Text;
                string commandMode = this.SemanticInterpretation("commandMode", res);


                Frame rootFrame = Window.Current.Content as Frame;
                if(rootFrame == null)
                {
                    rootFrame = new Frame();
                    Window.Current.Content = rootFrame;

                }

                if(cmdName.Equals("Magicos"))
                {
                    if(textSpoken.Equals("Mostrar los pueblos de Jalisco"))
                    {
                        cmdName = "AbrirPueblosMagicosJalisco";
                    }
                    if(textSpoken.Equals("Mostrar los pueblos de Veracruz"))
                    {
                        cmdName = "AbrirPueblosMagicosVeracruz";
                    }
                    if(textSpoken.Equals("Mostrar los pueblos de Guanajuato"))
                    {
                        cmdName = "AbrirPueblosMagicosGuanajuato";
                    }

                }


                switch(cmdName)
                {

                    case "AbrirPueblosMagicosJalisco":
                        rootFrame.Navigate(typeof(DashboardPage), "Jalisco");

                        break;
                    case "AbrirPueblosMagicosGuanajuato":
                        rootFrame.Navigate(typeof(DashboardPage), "Guanajuato");

                        break;
                    case "AbrirPueblosMagicosMichoacan":
                        rootFrame.Navigate(typeof(DashboardPage), "Michoacan");

                        break;
                    case "AbrirPueblosMagicosTamaulipas":
                        rootFrame.Navigate(typeof(DashboardPage), "Tamaulipas");

                        break;
                    case "AbrirPueblosMagicosVeracruz":
                        rootFrame.Navigate(typeof(DashboardPage), "Veracruz");

                        break;
                    case "AbrirPueblosMagicosAguascalientes":
                        rootFrame.Navigate(typeof(DashboardPage), "Aguascalientes");

                        break;
                    case "AgregarNuevoPuebloMagico":
                        rootFrame.Navigate(typeof(DashboardPage), "Agregar");

                        break;

                    case "EscucharPuebloNativo":
                        rootFrame.Navigate(typeof(DashboardPage), "Nativo");

                        break;
                    case "EscucharPuebloFrances":
                        rootFrame.Navigate(typeof(DashboardPage), "Frances");

                        break;
                    case "AbrirAppPueblosMagicos":
                        rootFrame.Navigate(typeof(DashboardPage), "");

                        break;
                    case "EliminarUltimoPueblo":
                        rootFrame.Navigate(typeof(DashboardPage), "EliminarUltimo");

                        break;

                    case "PueblosAgregadosActualmente":
                        rootFrame.Navigate(typeof(DashboardPage), "TotalActual");

                        break;


                }
                Window.Current.Activate();
            }
        }
        /// <summary>
        /// Returns the semantic interpretation of a speech result. Returns null if there is no interpretation for
        /// that key.
        /// </summary>
        /// <param name="interpretationKey">The interpretation key.</param>
        /// <param name="speechRecognitionResult">The result to get an interpretation from.</param>
        /// <returns></returns>
        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

    }
}
