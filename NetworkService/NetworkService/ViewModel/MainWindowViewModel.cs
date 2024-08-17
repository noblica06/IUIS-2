using GalaSoft.MvvmLight.Messaging;
using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata
                                //           zavisno od broja entiteta u listi
        private string windowTitle;
        public MyICommand<Window> CloseWindowCommand { get; private set; }
        public MyICommand<string> NavCommand { get; private set; }
        public static ObservableCollection<Entity> entities = new ObservableCollection<Entity>();
        public static BindableBase currentViewModel;
        public HomeViewModel homeViewModel;
        public EntitiesViewModel entitiesViewModel;
        public DisplayViewModel displayViewModel;
        public GraphViewModel graphViewModel;
        private BindableBase pastViewModel;
        private bool entityIE;
        private bool displayIE;
        private bool graphIE;
       
        public MainWindowViewModel()
        {
            createListener(); //Povezivanje sa serverskom aplikacijom
            //Entities = new ObservableCollection<Entity>();
            //Entities.Add(new Entity(1, "IDK", Model.Type.RTD));
            WindowTitle = "Network Service";
            CloseWindowCommand = new MyICommand<Window>(OnClose);
            NavCommand = new MyICommand<string>(OnNav);
            homeViewModel = new HomeViewModel();
            entitiesViewModel = new EntitiesViewModel();
            displayViewModel = new DisplayViewModel();
            graphViewModel = new GraphViewModel();
            CurrentViewModel = homeViewModel;
            pastViewModel = homeViewModel;
            EntityIE = true;
            DisplayIE = true;
            GraphIE = true;
            Messenger.Default.Register<Entity>(this, AddEntity);
        }
        public static ObservableCollection<Entity> Entities
        {
            get { return entities; }
            set
            {
                entities = value;
               
            }
        }
        public bool EntityIE
        {
            get { return entityIE; }
            set
            {
                entityIE = value;
                OnPropertyChanged("EntityIE");
            }
        }
        public bool GraphIE
        {
            get { return graphIE; }
            set
            {
                graphIE = value;
                OnPropertyChanged("GraphIE");
            }
        }
        public bool DisplayIE
        {
            get { return displayIE; }
            set
            {
                displayIE = value;
                OnPropertyChanged("DisplayIE");
            }
        }
        private void AddEntity(Entity tba)
        {
            Entities.Add(tba);
            
            
        }
        private void OnNav(string target)
        {
            switch (target)
            {
                case "home":
                    if (CurrentViewModel == homeViewModel)
                    {
                        pastViewModel = homeViewModel;

                    }
                    if (CurrentViewModel == entitiesViewModel)
                    {
                        pastViewModel = entitiesViewModel;

                    }
                    if (CurrentViewModel == displayViewModel)
                    {
                        pastViewModel = displayViewModel;

                    }
                    if (CurrentViewModel == graphViewModel)
                    {
                        pastViewModel = graphViewModel;

                    }
                    CurrentViewModel = homeViewModel;
                    DisplayIE = true;
                    EntityIE = true;
                    GraphIE = true;
                    break;
                case "entity":
                    if (CurrentViewModel == homeViewModel)
                    {
                        pastViewModel = homeViewModel;

                    }
                    if (CurrentViewModel == entitiesViewModel)
                    {
                        pastViewModel = entitiesViewModel;

                    }
                    if (CurrentViewModel == displayViewModel)
                    {
                        pastViewModel = displayViewModel;

                    }
                    if (CurrentViewModel == graphViewModel)
                    {
                        pastViewModel = graphViewModel;

                    }
                    CurrentViewModel = entitiesViewModel;
                    DisplayIE = true;
                    EntityIE = false;
                    GraphIE = true;
                    break;
                case "display":
                    if (CurrentViewModel == homeViewModel)
                    {
                        pastViewModel = homeViewModel;

                    }
                    if (CurrentViewModel == entitiesViewModel)
                    {
                        pastViewModel = entitiesViewModel;

                    }
                    if (CurrentViewModel == displayViewModel)
                    {
                        pastViewModel = displayViewModel;

                    }
                    if (CurrentViewModel == graphViewModel)
                    {
                        pastViewModel = graphViewModel;

                    }
                    CurrentViewModel = displayViewModel;
                    DisplayIE = false;
                    EntityIE = true;
                    GraphIE = true;
                    break;
                case "graph":
                    if (CurrentViewModel == homeViewModel)
                    {
                        pastViewModel = homeViewModel;
                        
                    }
                    if (CurrentViewModel == entitiesViewModel)
                    {
                        pastViewModel = entitiesViewModel;
                        
                    }
                    if (CurrentViewModel == displayViewModel)
                    {
                        pastViewModel = displayViewModel;
                        
                    }
                    if (CurrentViewModel == graphViewModel)
                    {
                        pastViewModel = graphViewModel;
                        
                    }
                    CurrentViewModel = graphViewModel;
                    DisplayIE = true;
                    EntityIE = true; 
                    GraphIE = false;
                    break;
                case "undo":
                    BindableBase temp = new BindableBase();
                    
                    if (CurrentViewModel == homeViewModel)
                    {
                        temp = homeViewModel;

                    }
                    if (CurrentViewModel == entitiesViewModel)
                    {
                        temp = entitiesViewModel;

                    }
                    if (CurrentViewModel == displayViewModel)
                    {
                        temp = displayViewModel;

                    }
                    if (CurrentViewModel == graphViewModel)
                    {
                        temp = graphViewModel;

                    }
                    if (pastViewModel == homeViewModel)
                    {
                        DisplayIE = true;
                        EntityIE = true;
                        GraphIE = true;

                    }
                    if (pastViewModel == entitiesViewModel)
                    {
                        DisplayIE = true;
                        EntityIE = false;
                        GraphIE = true;

                    }
                    if (pastViewModel == displayViewModel)
                    {
                        DisplayIE = false;
                        EntityIE = true;
                        GraphIE = true;

                    }
                    if (pastViewModel == graphViewModel)
                    {
                        DisplayIE = true;
                        EntityIE = true;
                        GraphIE = false;

                    }
                    CurrentViewModel = pastViewModel;
                    pastViewModel = temp;
                    break;
            }
        }

        private void OnClose(Window MainWindow)
        {
            MainWindow.Close();

        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
                
            }
        }
            
        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                windowTitle = value;
                OnPropertyChanged("WindowTitle");
            }
        }
       
        private static bool added = false;
        private  void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(MainWindowViewModel.Entities.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            string[] split1 = incomming.Split('_'); //Na primer: "Entitet_1:272"
                                                                    // WindowTitle = incomming;
                            string[] split2 = split1[1].Split(':');
                            if (Int32.Parse(split2[0]) < Entities.Count)
                            {
                                Entity e = Entities[Int32.Parse(split2[0])];
                                e.addTemp(double.Parse(split2[1]));
                            }
                            Messenger.Default.Send<bool>(true);

                            
                            

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
    }
}
