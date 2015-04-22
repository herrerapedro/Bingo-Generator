using System;
using System.Collections.Generic;
using System.IO;
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

namespace BingoUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICacheStore cache;
        private int size = 120;

        public MainWindow(ICacheStore cache)
        {
            InitializeComponent();

            this.cache = cache;

            ImageSource bgImg = new BitmapImage(new Uri("pack://siteoforigin:,,,/Images/background.png"));
            ImageBrush bgBrush = new ImageBrush(bgImg);
            bgBrush.TileMode = TileMode.FlipXY;
            bgBrush.Stretch = Stretch.Uniform;
            bgBrush.AlignmentY = AlignmentY.Top;
            bgBrush.ViewboxUnits = BrushMappingMode.Absolute;
            this.parentBorder.Background = bgBrush;

            this.parentCanvas.Width = this.size * 5;
            this.parentCanvas.Height = this.size * 5;
        }

        public void DrawContent()
        {
            this.parentCanvas.Children.Clear();

            var random = new Random();
            for (int i = 1; i < 26; i++)
            {
                int imageNumber;

                do
                {
                    imageNumber = random.Next(1, 26);
                } while (this.cache.Exists(imageNumber) || imageNumber == 13);

                if (i == 13) imageNumber = 13;

                this.cache.Save(imageNumber);

                var fileName = imageNumber + ".png";
                var imgPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                // TODO: Extract to separate class SRP violation
                // We may decide to store the images in a relational database in the future.
                if (!File.Exists(imgPath))
                {
                    fileName = fileName.Replace("png", "jpg");
                }

                ImageSource img = new BitmapImage(new Uri(string.Format("pack://siteoforigin:,,,/Images/{0}", fileName)));
                ImageBrush imgBrush = new ImageBrush(img);

                var childBorder = new Border();
                childBorder.Height = this.size;
                childBorder.Width = this.size;
                childBorder.BorderBrush = Brushes.MistyRose;
                childBorder.BorderThickness = new Thickness(1);

                var childCanvas = new Canvas();
                childCanvas.Background = imgBrush;
                childCanvas.Height = 100;
                childCanvas.Width = 100;

                if (i >= 6 && i < 11)
                {
                    Canvas.SetLeft(childBorder, this.size * (i - 6));
                    Canvas.SetTop(childBorder, this.size * 1);
                }
                else if (i >= 11 && i < 16)
                {
                    Canvas.SetLeft(childBorder, this.size * (i - 11));
                    Canvas.SetTop(childBorder, this.size * 2);
                }
                else if (i >= 16 && i < 21)
                {
                    Canvas.SetLeft(childBorder, this.size * (i - 16));
                    Canvas.SetTop(childBorder, this.size * 3);
                }
                else if (i >= 21)
                {
                    Canvas.SetLeft(childBorder, this.size * (i - 21));
                    Canvas.SetTop(childBorder, this.size * 4);
                }
                else
                {
                    Canvas.SetLeft(childBorder, this.size * (i - 1));
                    Canvas.SetTop(childBorder, 0);
                }

                childBorder.Child = childCanvas;
                parentCanvas.Children.Add(childBorder);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.cache.Clear();
            this.DrawContent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Rect rect = new Rect(0, 0, this.size * 5, this.size * 5);
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)rect.Right,
                (int)rect.Bottom,
                96d,
                96d,
                System.Windows.Media.PixelFormats.Default);
            rtb.Render(this.parentCanvas);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            MemoryStream ms = new MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();

            File.WriteAllBytes(string.Format("bingoSheetNumber_{0}.png", this.txtFileName.Text), ms.ToArray());
        }
    }
}
