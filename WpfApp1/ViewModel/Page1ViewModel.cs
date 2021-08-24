using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Helper;
using WpfApp1.Model;
using WpfApp1.Properties;

namespace WpfApp1.ViewModel
{
    class Page1ViewModel : NotifyPropertyChanged
    {
        public Command OpenFile
        {
            get; set;
        }

        public Command SaveFile 
        {
            get; set;
        }

        private ObservableCollection<ListViewItem> loadedImages;
        public ObservableCollection<ListViewItem> LoadedImages
        {
            get
            {
                return loadedImages;
            }
            set
            {
                loadedImages = value;
                //OnPropertyChanged();
            }
        }

        private Dictionary<WriteableBitmap, BitmapImage> OriginalImages;
        private Dictionary<ListViewItem, WriteableBitmap> Images;
        private Dictionary<WriteableBitmap, IntPixelColor> Settings;

        private ListViewItem currentItem;
        public ListViewItem CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                if (value.Content is Image)
                {
                    if(currentItem != null)
                        Settings[Images[currentItem]] = CurrentSetting; 
                    currentItem = value;
                    CurrentSetting = Settings[Images[value]];
                }
                else
                    throw new NotSupportedException();
            }
        }

        bool updateColor = true;

        private IntPixelColor currentSetting;
        public IntPixelColor CurrentSetting
        {
            get
            {
                return currentSetting;
            }
            set
            {
                currentSetting = value;

                updateColor = false;
                OnPropertyChanged("CurrentR");
                OnPropertyChanged("CurrentG");
                OnPropertyChanged("CurrentB");

                updateColor = true;
                OnPropertyChanged("CurrentA");
            }
        }
        // Slider Binding
        public int CurrentR
        {
            get
            {
                return CurrentSetting.Red;
            }
            set
            {
                currentSetting.Red = value;
                OnPropertyChanged();
                OnColorChanged();
            }
        }
        public int CurrentG
        {
            get
            {
                return CurrentSetting.Green;
            }
            set
            {
                currentSetting.Green = value;
                OnPropertyChanged();
                OnColorChanged();
            }
        }
        public int CurrentB
        {
            get
            {
                return CurrentSetting.Blue;
            }
            set
            {
                currentSetting.Blue = value;
                OnPropertyChanged();
                OnColorChanged();
            }
        }
        public int CurrentA
        {
            get
            {
                return CurrentSetting.Alpha;
            }
            set
            {
                currentSetting.Alpha = value;
                OnPropertyChanged();
                OnColorChanged();
            }
        }
        //

        private uint columnSize;
        public uint ColumnSize
        {
            get => columnSize;
            set
            {
                if ((int)value <= 0) return;
                columnSize = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush backgroundColour;
        public SolidColorBrush BackgroundColour
        {
            get => backgroundColour;
            set
            {
                backgroundColour = value;
                OnPropertyChanged();
            }
        }

        private byte backgroundColourAlpha;
        public byte BackgroundColourAlpha
        {
            get => backgroundColourAlpha;
            set
            {
                backgroundColourAlpha = value;
                OnPropertyChanged();
                BackgroundColour.Color = Color.FromArgb(
                    backgroundColourAlpha,
                    BackgroundColour.Color.R,
                    BackgroundColour.Color.G,
                    BackgroundColour.Color.B
                    );
                OnPropertyChanged("BackgroundColour");
            }
        }

        public Command ChangeBackgroundColour
        {
            get; set;
        }

        private void OnColorChanged()
        {
            if (!updateColor) return;

            var img = Images[CurrentItem];
            var origImg = OriginalImages[img];
            var width = img.PixelWidth;
            var height = img.PixelHeight;
            var stride = width * 4;
            var pixels = new PixelColor[height, width];

            unsafe
            {
                BitmapSourceHelper.CopyPixels(origImg, pixels, stride, 0);
            }

            for (var y = 0; y < height; ++y)
            {
                for(var x = 0; x < width; ++x)
                {
                    pixels[y, x] = new PixelColor()
                    {
                        Blue = (byte)Math.Max(Math.Min(pixels[y, x].Blue + CurrentB, 255), 0),
                        Green = (byte)Math.Max(Math.Min(pixels[y, x].Green + CurrentG, 255), 0),
                        Red = (byte)Math.Max(Math.Min(pixels[y, x].Red + CurrentR, 255), 0),
                        Alpha = (byte)Math.Max(Math.Min(pixels[y, x].Alpha + CurrentA, 255), 0),
                    };
                }
            }

            BitmapSourceHelper.WritePixels(img, pixels, stride, 0);
        }

        public Page1ViewModel()
        {
            OpenFile = new Command(OpenFileCallback);
            SaveFile = new Command(SynthesizeLoadedImages);
            LoadedImages = new ObservableCollection<ListViewItem>();
            OriginalImages = new Dictionary<WriteableBitmap, BitmapImage>();
            Images = new Dictionary<ListViewItem, WriteableBitmap>();
            Settings = new Dictionary<WriteableBitmap, IntPixelColor>();
            ChangeBackgroundColour = new Command(ChangeBackgroundColourCallback);
            BackgroundColour = new SolidColorBrush(Colors.White);
            // 기본 불투명
            BackgroundColourAlpha = 255;

            // 기본 5열
            ColumnSize = 5;
        }

        private void OpenFileCallback()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Filter = "PNG Files|*.png|Jpeg Files|*.jpg";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (var file in dialog.FileNames)
                    {
                        var ret = OpenImageFromPath(file);
                        var origImg = ret.Item1;
                        var img = ret.Item2;

                        var item = new ListViewItem()
                        {
                            Content = new Image()
                            {
                                Source = img,
                                Stretch = Stretch.Uniform,
                                Margin = new Thickness(10),
                            }
                        };

                        OriginalImages.Add(img, origImg);
                        Images.Add(item, img);
                        Settings.Add(img, new IntPixelColor());
                        
                        LoadedImages.Add(item);
                    }
                }
            }
        }

        private (BitmapImage, WriteableBitmap) OpenImageFromPath(string path)
        {
            var origImg = new BitmapImage(new Uri(path));
            var width = origImg.PixelWidth;
            var height = origImg.PixelHeight;
            var stride = width * 4;

            // Alpha값 (투명도)를 가지지 않는 이미지도 투명도를 가질 수 있게 BGRA32포맷으로 강제지정
            var img = new WriteableBitmap(width, height, origImg.DpiX, origImg.DpiY, PixelFormats.Bgra32, null);

            PixelColor[,] pixels = new PixelColor[height, width];

            BitmapSourceHelper.CopyPixels(origImg, pixels, stride, 0);
            BitmapSourceHelper.WritePixels(img, pixels, stride, 0);

            return (origImg, img);
        }

        private void SynthesizeLoadedImages()
        {
            if (loadedImages.Count == 0) return;

            int width = 0;
            int height = 0;

            // Get full image size
            foreach(var item in loadedImages)
            {
                var img = Images[item];
                width = Math.Max(width, img.PixelWidth);
                height = Math.Max(height, img.PixelHeight);
            }

            // 5 

            int col = (int)Math.Min(loadedImages.Count, ColumnSize);
            int row = (int)Math.Ceiling(LoadedImages.Count / (double)ColumnSize);

            var wbmp = new WriteableBitmap(width * col, height * row, 192, 192, PixelFormats.Bgra32, null);

            int x = 0, y = 0;
            foreach(var item in LoadedImages)
            {
                var img = Images[item];

                PixelColor[,] pixels = new PixelColor[img.PixelHeight, img.PixelWidth];
                BitmapSourceHelper.CopyPixels(img, pixels, img.PixelWidth * 4, 0);

                // 색상 혼합 식
                /*
                rOut = (rA * aA / 255) + (rB * aB * (255 - aA) / (255*255))
                gOut = (gA * aA / 255) + (gB * aB * (255 - aA) / (255*255))
                bOut = (bA * aA / 255) + (bB * aB * (255 - aA) / (255*255))
                aOut = aA + (aB * (255 - aA) / 255)
                */

                for (int yy = 0; yy < img.PixelHeight; ++yy)
                {
                    for(int xx = 0; xx < img.PixelWidth; ++xx)
                    {
                        var A = pixels[yy, xx];
                        var B = BackgroundColour.Color;

                        var rOut = (A.Red   * A.Alpha / 255) + (B.R * B.A * (255 - A.Alpha) / 255 * 255);
                        var gOut = (A.Green * A.Alpha / 255) + (B.G * B.A * (255 - A.Alpha) / 255 * 255);
                        var bOut = (A.Blue  * A.Alpha / 255) + (B.B * B.A * (255 - A.Alpha) / 255 * 255);
                        var aOut = (A.Alpha)                 + (      B.A * (255 - A.Alpha) / 255);

                        pixels[yy, xx] = new PixelColor()
                        {
                            Red     = (byte)rOut,
                            Green   = (byte)gOut,
                            Blue    = (byte)bOut,
                            Alpha   = (byte)aOut
                        };
                    }
                }

                int dx = (width - img.PixelWidth) / 2;
                int dy = (height - img.PixelHeight) / 2;

                wbmp.WritePixels(
                    new Int32Rect(width * x + dx, height * y + dy, img.PixelWidth, img.PixelHeight),
                    pixels, 
                    img.PixelWidth * 4, 
                    0);

                if(++x % ColumnSize == 0)
                {
                    ++y;
                    x = 0;
                }
            }

            using(var dialog = new System.Windows.Forms.SaveFileDialog())
            {
                dialog.Filter = "PNG File|*.png";
                dialog.DefaultExt = ".png";
                dialog.FileName = "image";
                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.FileName;

                    var encoder = new PngBitmapEncoder();
                    
                    encoder.Frames.Add(BitmapFrame.Create(wbmp));

                    using (var stream = File.OpenWrite(path))
                        encoder.Save(stream);

                    MessageBox.Show("Done~!");
                    Process.Start($"{Directory.GetParent(path)}");
                }
            }
        }

        private void ChangeBackgroundColourCallback()
        {
            using(var dialog = new System.Windows.Forms.ColorDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BackgroundColour = new SolidColorBrush()
                    {
                        Color = Color.FromArgb(BackgroundColourAlpha, dialog.Color.R, dialog.Color.G, dialog.Color.B)
                    };
                }
            }
        }
    }
}
