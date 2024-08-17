using GalaSoft.MvvmLight.Messaging;
using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class EntitiesViewModel : BindableBase
    {
        private static ObservableCollection<Entity> toBeShown;
        public List<String> FilterTypes{ get; set;}
        public MyICommand AddCommand { get; set; }
        public MyICommand GotFCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand GotIDCommand { get; set; }
        public MyICommand GotLabelCommand { get; set; }
        public MyICommand GotDeleteCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }
        private bool greaterChecked;
        private bool lesserChecked;
        private bool equalChecked;
        private string infoMessage;
        private Entity currentEntity = new Entity();
        private string selectedType;
        private string filterIdTxt;
        private string deleteID;

        public string DeleteID
        {
            get { return deleteID; }
            set
            {
                deleteID = value;
                OnPropertyChanged("DeleteID");
            }
        }
        public string FilterIDTxt
        {
            get { return filterIdTxt; }
            set
            {
                filterIdTxt = value;
                OnPropertyChanged("FilterIDTxt");
            }
        }
        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                OnPropertyChanged("SelectedType");
            }
        }
        public bool GreaterChecked
        {
            get { return greaterChecked; }
            set
            {
                greaterChecked = value;
                OnPropertyChanged("GreaterChecked");
            }
        }
        public bool LesserChecked
        {
            get { return lesserChecked; }
            set
            {
                lesserChecked = value;
                OnPropertyChanged("LesserChecked");
            }
        }
        public bool EqualChecked
        {
            get { return equalChecked; }
            set
            {
                equalChecked = value;
                OnPropertyChanged("EqualChecked");
            }
        }
        public Entity CurrentEntity
        {
            get { return currentEntity; }
            set
            {
                currentEntity = value;
                OnPropertyChanged("CurrentEntity");
            }
        }
        public  ObservableCollection<Entity> ToBeShown
        {
            get { return toBeShown; }
            set
            {
                toBeShown = value;
                OnPropertyChanged("ToBeShown");
            }
        }
       /* private void updateColl()
        {
            var updateThread = new Thread(() =>
            {
                while (true)
                {
                    Filter();
                    Thread.Sleep(1000);
                }
            });
            updateThread.IsBackground = true;
            updateThread.Start();
        }*/
        public string InfoMessage
        {
            get { return infoMessage; }
            set
            {
                infoMessage = value;
                OnPropertyChanged("InfoMessage");
            }
        }

        public EntitiesViewModel()
        {
            ToBeShown = MainWindowViewModel.Entities;
            AddCommand = new MyICommand(OnAdd);
            FilterCommand = new MyICommand(Filter);
            GotFCommand = new MyICommand(OnFocus);
            GotIDCommand = new MyICommand(OnIDFocus);
            GotLabelCommand = new MyICommand(OnLabelFocus);
            GotDeleteCommand = new MyICommand(OnDeleteFocus);
            DeleteCommand = new MyICommand(OnDelete);
            GreaterChecked = false;
            LesserChecked = false;
            EqualChecked = false;
            FilterTypes = new List<string>();
            FilterTypes.Add("Both");
            FilterTypes.Add("RTD");
            FilterTypes.Add("ThermoCoupling");
            //updateColl();
        }

        private void OnDelete()
        {
            if (!string.IsNullOrWhiteSpace(DeleteID))
            {
                Entity does = null;
                foreach(Entity e in MainWindowViewModel.Entities)
                {
                    if(e.Id == DeleteID)
                    {
                        does = e;
                    }
                }
                if (does != null)
                {
                    MainWindowViewModel.Entities.Remove(does);
                }
            }
        }

        private void OnDeleteFocus()
        {
            KeyboardWindow kw = new KeyboardWindow(DeleteID);
            if (kw.ShowDialog() == true)
            {
                Console.WriteLine("Uso sam");
                DeleteID = kw.Ret.Trim();
            }
        }

        private void OnLabelFocus()
        {
            KeyboardWindow kw = new KeyboardWindow(CurrentEntity.Label);
            if (kw.ShowDialog() == true)
            {
                Console.WriteLine("Uso sam");
                CurrentEntity.Label = kw.Ret.Trim();
            }
        }

        private void OnIDFocus()
        {
            KeyboardWindow kw = new KeyboardWindow(CurrentEntity.Id);
            if (kw.ShowDialog() == true)
            {
                Console.WriteLine("Uso sam");
                CurrentEntity.Id = kw.Ret.Trim();
            }
        }

        private void OnFocus()
        {
            KeyboardWindow kw = new KeyboardWindow(FilterIDTxt);
            if(kw.ShowDialog() == true)
            {
                Console.WriteLine("Uso sam");
                FilterIDTxt = kw.Ret.Trim();
            }
        }

        private void OnAdd()
        {
            CurrentEntity.Validate();
            if (CurrentEntity.IsValid)
            {
                Entity e = new Entity(CurrentEntity.Id, CurrentEntity.Label, CurrentEntity.EntityType);
                Messenger.Default.Send<Entity>(e);
                ToBeShown = MainWindowViewModel.Entities;

                //Filter();
                CurrentEntity = new Entity();
                InfoMessage = $"Succesfully Added Entity: {e.Label}";
            }
        }
        private void Filter()
        {
            List<Entity> tba = new List<Entity>();
            if (selectedType != "Both" && selectedType != null)
            {
                ToBeShown = new ObservableCollection<Entity>();
                if (selectedType == "RTD")
                {
                    foreach (Entity e in MainWindowViewModel.Entities)
                    {
                        if (e.EntityType == Model.Types.Etype.RTD)
                        {
                            ToBeShown.Add(e);
                        }
                    }
                }
                else
                {
                    foreach (Entity e in MainWindowViewModel.Entities)
                    {
                        if (e.EntityType == Model.Types.Etype.ThermoCoupling)
                        {
                            ToBeShown.Add(e);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Nema Filtera");
                ToBeShown = MainWindowViewModel.Entities;
            }
            if (LesserChecked)
            {
                
                if (!string.IsNullOrEmpty(filterIdTxt))
                {
                    int intID;
                    bool good = Int32.TryParse(filterIdTxt, out intID);
                    if (good)
                    {
                        foreach (Entity e in ToBeShown)
                        {
                            if(intID > Int32.Parse(e.Id))
                            {
                                tba.Add(e);
                            }
                        }
                    }
                }
            }
            if (GreaterChecked)
            {

                if (!string.IsNullOrEmpty(filterIdTxt))
                {
                    int intID;
                    bool good = Int32.TryParse(filterIdTxt, out intID);
                    if (good)
                    {
                        foreach (Entity e in ToBeShown)
                        {
                            if (intID < Int32.Parse(e.Id))
                            {
                                tba.Add(e);
                            }
                        }
                    }
                }
            }
            if (EqualChecked)
            {

                if (!string.IsNullOrEmpty(filterIdTxt))
                {
                    int intID;
                    bool good = Int32.TryParse(filterIdTxt, out intID);
                    if (good)
                    {
                        foreach (Entity e in ToBeShown)
                        {
                            if (intID == Int32.Parse(e.Id))
                            {
                                tba.Add(e);
                            }
                        }
                    }
                }
            }
            int idC;
            if((LesserChecked || GreaterChecked || EqualChecked) && !string.IsNullOrWhiteSpace(filterIdTxt) && Int32.TryParse(filterIdTxt, out idC))
            {
                ToBeShown = new ObservableCollection<Entity>();
                foreach(Entity e in tba)
                {
                    ToBeShown.Add(e);
                }
            }
        }
    }
}
