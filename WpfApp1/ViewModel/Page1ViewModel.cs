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
            LoadedImages = new ObservableCollection<ListViewItem>();
            OriginalImages = new Dictionary<WriteableBitmap, BitmapImage>();
            Images = new Dictionary<ListViewItem, WriteableBitmap>();
            Settings = new Dictionary<WriteableBitmap, IntPixelColor>();
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
                        var origImg = new BitmapImage(new Uri(file));
                        var img = new WriteableBitmap(origImg);

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
    }
}
