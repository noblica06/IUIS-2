using GalaSoft.MvvmLight.Messaging;
using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static NetworkService.Model.Types;

namespace NetworkService.ViewModel
{
    public class DisplayViewModel : BindableBase
    {
        private List<SolidColorBrush> borderBrushes;
        private BindingList<EntityByType> allEnts;
        private Entity draggedItem;
        private Entity selectedItem;
        private bool dragging = false;
        private Dictionary<int, string> dropped;
        private static List<Canvas> dropani;
        public Dictionary<int,string> Dropped
        {
            get { return dropped; }
            set
            {
                dropped = value;
                OnPropertyChanged("Dropped");
            }
        }

        public List<SolidColorBrush> BorderBrushes
        {
            get { return borderBrushes; }
            set
            {
                borderBrushes = value;
                OnPropertyChanged("BorderBrushes");
            }
        }
        public MyICommand Mlbu { get; set; }
        public MyICommand<object> SelChangedCommand { get; set; }

        public MyICommand<object> DropCommand { get; set; }

        public MyICommand<object> ResetCommand { get; set; }
        public MyICommand<object> MouseDownCommand { get; set; }
        public Entity DraggedItem
        {
            get { return draggedItem; }
            set
            {
                draggedItem = value;
                OnPropertyChanged("DraggedItem");
            }
        }
        public Entity SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public BindingList<EntityByType> AllEnts
        {
            get { return allEnts; }
            set
            {
                allEnts = value;
                OnPropertyChanged("AllEnts");
            }
        }

        public DisplayViewModel()
        {
            AllEnts = new BindingList<EntityByType>();
            EntityByType RTD = new EntityByType() { Etype = Etype.RTD };
            foreach(Entity e in MainWindowViewModel.Entities)
            {
                if(e.EntityType == Etype.RTD)
                {
                    RTD.Entities.Add(e);
                }
            }
            EntityByType TC = new EntityByType() { Etype = Etype.ThermoCoupling };
            foreach (Entity e in MainWindowViewModel.Entities)
            {
                if (e.EntityType == Etype.ThermoCoupling)
                {
                    TC.Entities.Add(e);
                }
            }
            AllEnts.Add(RTD);
            AllEnts.Add(TC);
            SelChangedCommand = new MyICommand<object>(OnSelCh);
            Mlbu = new MyICommand(OnMLBU);
            DropCommand = new MyICommand<object>(OnDrop);
            ResetCommand = new MyICommand<object>(OnReset);
            MouseDownCommand = new MyICommand<object>(OnMouseDown);
            DraggedItem = new Entity();
            SelectedItem = new Entity();
            BorderBrushes = new List<SolidColorBrush>();
            for(int i = 0; i < 12; ++i)
            {

                SolidColorBrush sb = new SolidColorBrush();
                sb = Brushes.White;
                BorderBrushes.Add(sb);
            }
            Dropped = new Dictionary<int, string>();
            dropani = new List<Canvas>();
            //ThreadUpdate();
        }

        private void OnMouseDown(object sender)
        {
            Canvas can = sender as Canvas;
            TextBlock tb1 = ((TextBlock)((Canvas)sender).Children[0]);
            TextBlock tb2 = ((TextBlock)((Canvas)sender).Children[1]);
            string id = tb1.Text.Trim();
            if (can.Resources["taken"] != null)
            {
                if (!dragging)
                {

                    dragging = true;
                    foreach(Entity e in MainWindowViewModel.Entities)
                    {
                        if(e.Id == id)
                        {
                            DraggedItem = e;
                            break;
                        }
                    }
                    can.Background = Brushes.Transparent;
                    tb1.Text = String.Empty;
                    tb2.Text = String.Empty;
                    can.Resources.Remove("taken");
                    can.AllowDrop = true;
                    Border b = (Border)VisualTreeHelper.GetParent(can);

                    b.BorderBrush = Brushes.White;
                    
                    dropani.Remove(can);
                    UpdateCan();
                    var a = DragDrop.DoDragDrop(can, DraggedItem, DragDropEffects.Move | DragDropEffects.None);
                    
                    if (a == DragDropEffects.None)
                    {
                        tb1.Text = DraggedItem.Id.ToString();
                        tb2.Text = DraggedItem.Label.ToString();
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        if(DraggedItem.EntityType == Etype.RTD)
                            img.UriSource = new Uri("C:/Users/nikol/Desktop/IUIS/NetworkService/NetworkService/Resources/RTD.jpg", UriKind.RelativeOrAbsolute);
                        else
                            img.UriSource = new Uri("C:/Users/nikol/Desktop/IUIS/NetworkService/NetworkService/Resources/ThermoCouple.jpg", UriKind.RelativeOrAbsolute);
                        img.EndInit();
                        can.Background = new ImageBrush(img);
                        can.Resources.Add("taken", true);
                        can.AllowDrop = false;
                        //ukloni element
                        RemoveElement();
                        dropani.Add(can);
                        DraggedItem = null;
                        dragging = false;
                        UpdateCan();
                        Messenger.Default.Register<bool>(this, NewAdded);
                        //ThreadUpdate();
                    }
                }
            }

        }

        private void NewAdded(bool obj)
        {
            if (obj)
            {
                UpdateCan();
            }
        }

        public  void  UpdateCan()
        {


            foreach (Canvas c in dropani)
            {
                TextBlock tb1 = ((TextBlock)c.Children[0]);
                TextBlock tb2 = ((TextBlock)c.Children[1]);

                foreach (Entity e in MainWindowViewModel.Entities)
                {
                    if (e.Id == tb1.Text.Trim())
                    {
                        Console.WriteLine("NASO SAM");
                        Border b = (Border)VisualTreeHelper.GetParent(c);
                        if (e.Temperature[0] < 250 || e.Temperature[0] > 350)
                        {
                            b.BorderBrush = Brushes.Red;
                            //OnMLBU();
                        }
                        else
                        {
                            b.BorderBrush = Brushes.White;
                           // OnMLBU();
                        }

                    }

                }
            }
        }
        private void OnReset(object sender)
        {
            Canvas can = sender as Canvas;

            if(can.Resources["taken"] != null)
            {
                //vratiElement
                ReturnElement(can);
                can.Background = Brushes.Transparent;
                TextBlock tb1 = ((TextBlock)((Canvas)sender).Children[0]);
                TextBlock tb2 = ((TextBlock)((Canvas)sender).Children[1]);
                tb1.Text = String.Empty;
                tb2.Text = String.Empty;
                can.Resources.Remove("taken");
                can.AllowDrop = true;
                Border b = (Border)VisualTreeHelper.GetParent(can);
                
                b.BorderBrush = Brushes.White;
                dropani.Remove(can);
                
            }
        }

        private void OnDrop(object sender)
        {

            Canvas can = (Canvas)sender;
            TextBlock tb1 = ((TextBlock)((Canvas)sender).Children[0]);
            TextBlock tb2 = ((TextBlock)((Canvas)sender).Children[1]);

            if (DraggedItem != null && can.Resources["taken"] == null)
            {
                tb1.Text = DraggedItem.Id.ToString();
                tb2.Text = DraggedItem.Label.ToString();
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                if (DraggedItem.EntityType == Etype.RTD)
                    img.UriSource = new Uri("C:/Users/nikol/Desktop/IUIS/NetworkService/NetworkService/Resources/RTD.jpg", UriKind.RelativeOrAbsolute);
                else
                    img.UriSource = new Uri("C:/Users/nikol/Desktop/IUIS/NetworkService/NetworkService/Resources/ThermoCouple.jpg", UriKind.RelativeOrAbsolute);
                img.EndInit();
                can.Background = new ImageBrush(img);
                can.Resources.Add("taken", true);
                can.AllowDrop = false;
                dropani.Add(can);
                //ukloni element
                RemoveElement();
                
            }
            DraggedItem = null;
            dragging = false;
            UpdateCan();
        }

       
        private void RemoveElement()
        {
            if(DraggedItem.EntityType == Etype.RTD)
            {
                AllEnts[0].Entities.Remove(DraggedItem);
            }
            else
            {
                AllEnts[1].Entities.Remove(DraggedItem);
            }
        }
        private void ReturnElement(Canvas can)
        {
            string id = ((TextBlock)can.Children[0]).Text.Trim();
            foreach(Entity e in MainWindowViewModel.Entities)
            {
                if(e.Id == id)
                {
                    if(e.EntityType == Etype.RTD)
                    {
                        AllEnts[0].Entities.Add(e);
                        Console.WriteLine("AAAAA");
                        break;
                        
                    }
                    else
                    {
                        AllEnts[1].Entities.Add(e);
                        Console.WriteLine("AAAAA");
                        break;
                        
                    }
                }
            }
            UpdateCan();
        }
        private void OnMLBU()
        {
            DraggedItem = null;
            dragging = false;
            UpdateCan();
        }

        private void OnSelCh(object sender)
        {
            TreeView tw = sender as TreeView;
            if (!dragging
                && tw.SelectedItem.GetType() != typeof(EntityByType) )
            {
                
                dragging = true;
                DraggedItem = (Entity)tw.SelectedItem;
                DragDrop.DoDragDrop(tw, DraggedItem, DragDropEffects.Move | DragDropEffects.None);
                UpdateCan();
            }
        }
    }
}
