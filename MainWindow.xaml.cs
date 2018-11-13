using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Drawing;
using System.IO;

namespace CoverAdd
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFronte_Click(object sender, RoutedEventArgs e)
        {
            // Stampa le coordinate del fronte sulla console
            Console.WriteLine("Fronte");
            Console.WriteLine("RenderTransform.Value: " + fronte.Child.RenderTransform.Value.Determinant.ToString());
                        
        }

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is ScaleTransform);
        }

        private void btnAllinea_Click(object sender, RoutedEventArgs e)
        {
            var st = GetScaleTransform(fronte.Child);
            
            retro.Child.RenderTransformOrigin = new Point(0,0);
            
            ScaleTransform myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleY = st.ScaleX;
            myScaleTransform.ScaleX = st.ScaleY;

            Console.WriteLine("st.ScaleX: " + st.ScaleX);
            Console.WriteLine("st.ScaleY: " + st.ScaleY);

            RotateTransform myRotateTransform = new RotateTransform();
            myRotateTransform.Angle = 0;

            TranslateTransform myTranslate = new TranslateTransform();
            myTranslate.X = retro.Child.RenderTransform.Value.OffsetX;
            myTranslate.Y = fronte.Child.RenderTransform.Value.OffsetY;

            Console.WriteLine("OffsetX: " + fronte.Child.RenderTransform.Value.OffsetX);
            Console.WriteLine("OffsetY: " + fronte.Child.RenderTransform.Value.OffsetY);

            SkewTransform mySkew = new SkewTransform();
            mySkew.AngleX = 0;
            mySkew.AngleY = 0;

            // Create a TransformGroup to contain the transforms 
            // and add the transforms to it. 
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myScaleTransform);
            myTransformGroup.Children.Add(myRotateTransform);
            myTransformGroup.Children.Add(myTranslate);
            myTransformGroup.Children.Add(mySkew);

            // Associate the transforms to the object 
            retro.Child.RenderTransform = myTransformGroup;
        }

        private void btnSalva_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("OffsetX: " + fronte.Child.RenderTransform.Value.OffsetX);
            Console.WriteLine("OffsetY: " + fronte.Child.RenderTransform.Value.OffsetY);

            var st = GetScaleTransform(fronte.Child);
            ScaleTransform myScaleTransform = new ScaleTransform();
            
            Console.WriteLine("st.ScaleX: " + st.ScaleX);
            Console.WriteLine("st.ScaleY: " + st.ScaleY);

            Console.WriteLine("rect_fronte width: " + (int)rect_fronte.ActualWidth);
            Console.WriteLine("rect_fronte height: " + (int)rect_fronte.ActualHeight);

            //Dimensioni Elemento visualizzato per avere le proprorzioni dell'elemento rect ritagliato
            Rect rfronte = new Rect(fronte.RenderSize);
            Console.WriteLine("renderSize width: " + (int)rfronte.Width);
            Console.WriteLine("renderSize height: " + (int)rfronte.Height);

            string uriImg = "cover.jpg";

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(uriImg, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            int w = (int)src.PixelWidth;
            int h = (int)src.PixelHeight;

            Console.WriteLine("Cover width: " + w);
            Console.WriteLine("Cover height: " + h);


            CropImage(uriImg,0,0,w,h);


        }

        public void CropImage(string uriImg, int x, int y, int w, int h)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(uriImg, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            var immagine =  new CroppedBitmap(src, new Int32Rect( x, y, w, h));

            FileStream stream = new FileStream("new.jpg", FileMode.Create);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(immagine));
            encoder.Save(stream);
        
        }


    }


}
