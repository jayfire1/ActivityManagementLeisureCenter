using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ActivityManagementLeisureCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageAdminAdherents : Page
    {
        public PageAdminAdherents()
        {
            this.InitializeComponent();
            chargerAdherents();
        }

        private void chargerAdherents()
        {
            lvAdherents.ItemsSource = SingletonBD.getInstance().getListeAdherents();
        }

        private void SupprimerAdherent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifierAdherent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AjouterAdherent_Click(Object sender, RoutedEventArgs e)
        {

        }


    }
}
