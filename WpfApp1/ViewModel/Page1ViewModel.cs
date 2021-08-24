using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Command openFile;
        public Command OpenFile
        {
            get
            {
                return openFile;
            }
            set
            {
                openFile = value;
            }
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
                OnPropertyChanged("CurrentR");
                OnPropertyChanged("CurrentG");
                OnPropertyChanged("CurrentB");
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

        private void OnColorChanged()
        {
            var img = Images[CurrentItem];
            var origImg = OriginalImages[img];
            int width = img.PixelWidth;
            int height = img.PixelHeight;
            int stride = width * 4;
            var pixels = new PixelColor[height, width];

            try
            {
                unsafe
                {
                    BitmapSourceHelper.CopyPixels(origImg, pixels, stride, 0);
                }
            }
            finally
            {

            }

            for(int y = 0; y < height; ++y)
            {
                for(int x = 0; x < width; ++x)
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

        private double listViewHeight;
        public double ListViewHeight
        {
            get
            {
                return listViewHeight;
            }
            set
            {
                listViewHeight = value;
            }
        }

        public Page1ViewModel()
        {
            //OpenFile = new Command(OpenFileCallback);
            OpenFile = new Command(Test);
            LoadedImages = new ObservableCollection<ListViewItem>();
            OriginalImages = new Dictionary<WriteableBitmap, BitmapImage>();
            Images = new Dictionary<ListViewItem, WriteableBitmap>();
            Settings = new Dictionary<WriteableBitmap, IntPixelColor>();
        }

        private void Test()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Filter = "PNG Files|*.png|Jpeg Files|*.jpg";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (var file in dialog.FileNames)
                    {
                        var origImg = new BitmapImage(new Uri(file));
                        var img = new WriteableBitmap(origImg);

                        //ImageColorChangeTest(ref img);
                        var item = new ListViewItem()
                        {
                            Content = new Image()
                            {
                                Source = img,
                                Stretch = Stretch.UniformToFill,
                                Margin = new Thickness(10),
                            }
                        };

                        OriginalImages.Add(img, origImg);
                        Images.Add(item, img);
                        Settings.Add(img, new IntPixelColor()
                        {
                            Red = 0,
                            Green = 0,
                            Blue = 0,
                            Alpha = 0,
                        });
                        
                        LoadedImages.Add(item);
                    }
                }
            }
        }

        private void ImageColorChangeTest(ref WriteableBitmap wbmp)
        {
            int width = wbmp.PixelWidth;
            int height = wbmp.PixelHeight;
            int stride = width * 4;
            PixelColor[,] pcs = new PixelColor[height, width];

            BitmapSourceHelper.CopyPixels(wbmp, pcs, stride, 0);

            for(int y = 0; y < height; ++y)
            {
                for(int x = 0; x < width; ++x)
                {
                    if (pcs[y, x].Red >= 150 &&
                        pcs[y, x].Green >= 150 &&
                        pcs[y, x].Blue >= 150)
                        pcs[y, x] = new PixelColor
                        {
                            Blue = 0x00,
                            Green = 0x00,
                            Red = 0x00,
                            Alpha = 0x00
                        };
                    
                }
            }
            BitmapSourceHelper.WritePixels(wbmp, pcs, stride, 0);
        }

        public void OpenFileCallback()
        {
            using(var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Filter = "PNG Files|*.png|Jpeg Files|*.jpg";
                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach(var file in dialog.FileNames)
                    {
                        var img = new WriteableBitmap(new BitmapImage(new Uri(file)));

                        var item = new ListViewItem()
                        {
                            Content = new Image()
                            {
                                Source = img,
                                Stretch = Stretch.UniformToFill,
                            }
                        };

                        Images.Add(item, img);
                        Settings.Add(img, new IntPixelColor()
                        {
                            Red = 0,
                            Green = 0,
                            Blue = 0,
                            Alpha = 0,
                        });
                        LoadedImages.Add(item);
                        
                    }
                }
            }
        }
    }
}
