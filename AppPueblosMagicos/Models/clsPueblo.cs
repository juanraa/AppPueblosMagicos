using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace AppPueblosMagicos.Models
{
    public class Pueblo
    {
        public int PuebloID { get; set; }
        public string Name { get; set; }
        public string DescripcionEspanish { get; set; }
        public string DescripcionFrench { get; set; }
        public string URLImage { get; set; }
        public string Estado { get; set; }

        public ImageSource SourceImage { get;set;}

    }
}
