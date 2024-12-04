using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    public class BoutonPersonnalise : Button
    {
        public BoutonPersonnalise()
        {
            this.PointerEntered += BoutonPersonnalise_PointerEntered;
            this.PointerExited += BoutonPersonnalise_PointerExited;
        }

        private void BoutonPersonnalise_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void BoutonPersonnalise_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }
    }
}
