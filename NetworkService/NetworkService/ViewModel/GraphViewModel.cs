using NetworkService.Helpers;
using NetworkService.Model;
using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkService.ViewModel
{
    public class GraphViewModel : BindableBase
    {
        private int height1;
        private int height2;
        private int height3;
        private int height4;
        private int height5;
        private SolidColorBrush b1;
        private SolidColorBrush b2;
        private SolidColorBrush b3;
        private SolidColorBrush b4;
        private SolidColorBrush b5;
        private Entity selected;

        public SolidColorBrush B1
        {
            get { return b1; }
            set
            {
                b1 = value;
                OnPropertyChanged("B1");
            }
        }
        public SolidColorBrush B2
        {
            get { return b2; }
            set
            {
                b2 = value;
                OnPropertyChanged("B2");
            }

        }
        public SolidColorBrush B3
        {
            get { return b3; }
            set
            {
                b3 = value;
                OnPropertyChanged("B3");
            }
        }
        public SolidColorBrush B4
        {
            get { return b4; }
            set
            {
                b4 = value;
                OnPropertyChanged("B4");
            }
        }
        public SolidColorBrush B5
        {
            get { return b5; }
            set
            {
                b5 = value;
                OnPropertyChanged("B5");
            }
        }
        public MyICommand GenerateCommand { get; set; }
        public Entity Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }

        private ObservableCollection<Entity> allEnt;
        public ObservableCollection<Entity> AllEnt
        {
            get { return allEnt; }
            set
            {
                AllEnt = value;
                OnPropertyChanged("AllEnt");
            }
        }
        public int Height1
        {
            get { return height1; }
            set
            {
                height1 = value;
                OnPropertyChanged("Height1");
            }
        }
        public int Height2
        {
            get { return height2; }
            set
            {
                height2 = value;
                OnPropertyChanged("Height2");
            }
        }
        public int Height3
        {
            get { return height3; }
            set
            {
                height3 = value;
                OnPropertyChanged("Height3");
            }
        }

        private void generateGraph()
        {
            
             var bckgThread = new Thread(() =>
            {
                while (true)
                {
                    OnGen();
                    Thread.Sleep(500);
                }
            }
            );

            bckgThread.IsBackground = true;
            bckgThread.Start();
        }
        public int Height4
        {
            get { return height4; }
            set
            {
                height4 = value;
                OnPropertyChanged("Height4");
            }
        }
        public int Height5
        {
            get { return height5; }
            set
            {
                height5 = value;
                OnPropertyChanged("Height5");
            }
        }
        public GraphViewModel()
        {
            Height1 = 100;
            Height2 = 50;
            Height3 = 20;
            Height4 = 30;
            Height5 = 40;
            B1 = Brushes.LightGreen;
            B2 = Brushes.LightGreen;
            B3 = Brushes.LightGreen;
            B4 = Brushes.LightGreen;
            B5 = Brushes.LightGreen;
            allEnt = MainWindowViewModel.Entities;
            Selected = new Entity();
            GenerateCommand = new MyICommand(OnGen);
            generateGraph();
        }
       
        public void OnGen()
        {
            CalculateHeights(Selected);
        }

        public void CalculateHeights(Entity en)
        {
            Height1 = CalculateValue(en.Temperature[0]);
            B1 = CalculateBrush(en.Temperature[0]);
            Height2 = CalculateValue(en.Temperature[1]);
            B2 = CalculateBrush(en.Temperature[1]);
            Height3 = CalculateValue(en.Temperature[2]);
            B3 = CalculateBrush(en.Temperature[2]);
            Height4 = CalculateValue(en.Temperature[3]);
            B4 = CalculateBrush(en.Temperature[3]);
            Height5 = CalculateValue(en.Temperature[4]);
            B5 = CalculateBrush(en.Temperature[4]);
        }
        private int CalculateValue(double b)
        {
            if(b > 350)
            {
                return 280;
            }else if(b < 250)
            {
                return 15;
            }
            else 
            {
                return ((int)(b - 250) * 2) + 30;
            }
        }
        private SolidColorBrush CalculateBrush(double b)
        {
            if (b > 350)
            {
                return Brushes.Red;
            }
            else if (b < 250)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.LightGreen;
            }
        }
    }
}
