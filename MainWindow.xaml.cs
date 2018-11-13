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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
           

            RotateTransform myRotateTransform = new RotateTransform();
            myRotateTransform.Angle = 0;

            TranslateTransform myTranslate = new TranslateTransform();
            myTranslate.X = retro.Child.RenderTransform.Value.OffsetX;
            myTranslate.Y = fronte.Child.RenderTransform.Value.OffsetY;

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
            Rect rect = new Rect(fronte.RenderSize);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Width,(int)rect.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);

            rtb.Render(fronte.Child);
            
            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            System.IO.File.WriteAllBytes("fronte.png", ms.ToArray());
            Console.WriteLine("Done");
        }
    }
}
