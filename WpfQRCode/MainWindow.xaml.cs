using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace WpfQRCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //二维码生成操作
        private BitmapImage DoGenerateQrImage(string url)
        {
            try
            {
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                qrEncoder.TryEncode(url, out var qrCode);
                GraphicsRenderer renderer = new GraphicsRenderer(
                    new FixedCodeSize(150, QuietZoneModules.Zero),
                    System.Drawing.Brushes.Black,
                    System.Drawing.Brushes.White);
                MemoryStream ms = new MemoryStream();

                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                BitmapImage tempImage = new BitmapImage();
                tempImage.BeginInit();
                tempImage.DecodePixelWidth = 150;
                tempImage.StreamSource = ms;
                tempImage.EndInit();
                tempImage.Freeze();
                return tempImage;
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e);
                //throw;
            }
            return null;
        }

        // <summary>
        // 生成二维码
        // </summary>
        // <param name="msg">信息</param>
        // <param name="pixel">像素点大小</param>
        // <param name="iconPath">图标路径</param>
        // <param name="iconSize">图标尺寸</param>
        // <param name="iconBorder">图标边框厚度</param>
        // <param name="whiteEdge">二维码白边</param>
        // <returns>BitmapImage</returns>
        public BitmapImage GetBitmapCode(string msg, int pixel, string iconPath, int iconSize, int iconBorder, bool whiteEdge)
        {
            try
            {
                QRCoder.QRCodeGenerator codeGenerator = new QRCoder.QRCodeGenerator();
                QRCoder.QRCodeData codeData = codeGenerator.CreateQrCode(msg, QRCoder.QRCodeGenerator.ECCLevel.Q);
                QRCoder.QRCode code = new QRCoder.QRCode(codeData);
                Bitmap icon = null;
                if (!string.IsNullOrEmpty(iconPath))
                    icon = new Bitmap(iconPath);
                Bitmap bmp = code.GetGraphic(10, Color.Black, Color.White, icon, iconSize, iconBorder, whiteEdge);
                return BitmapToBitmapImage(bmp, pixel);
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e);
                //throw;
            }
            return null;
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap, int pixel)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    stream.Position = 0;
                    BitmapImage result = new BitmapImage();
                    result.BeginInit();
                    result.DecodePixelWidth = pixel;
                    result.DecodePixelHeight = pixel;
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.StreamSource = stream;
                    result.EndInit();
                    result.Freeze();
                    return result;
                }
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e);
                //throw;
            }

            return null;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var text = UrlTextBox.Text;

                if(string.IsNullOrEmpty(text))
                    return;

                QrCodeImage.Source = DoGenerateQrImage(text);
            }
            catch /*(Exception exception)*/
            {
                //Console.WriteLine(exception);
                //throw;
            }
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var text = UrlTextBox.Text;

                if (string.IsNullOrEmpty(text))
                    return;

                QrCodeImage.Source = GetBitmapCode(text, 140, "", 0, 0, true);
            }
            catch /*(Exception exception)*/
            {
                //Console.WriteLine(exception);
                //throw;
            }
        }
    }
}
