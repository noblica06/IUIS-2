using NetworkService.Helpers;
using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetworkService.Model.Types;

namespace NetworkService.Model
{

    public class Entity : ValidationBase
    {
        string id;
        string label;
        Etype entityType;
        List<double> temperature;

        public Entity()
        {
            temperature = new List<double>(5) { 0, 0, 0, 0, 0 };
            id = "";

        }
        public Entity(string id, string label, Etype type)
        {
            Id = id;
            Label = label;
            EntityType = type;
            temperature = new List<double>(5) { 0, 0, 0, 0, 0 };
        }

        public void addTemp(double temp)
        {
            Temperature.Insert(0, temp);
            OnPropertyChanged("Temperature");

        }
        public override string ToString()
        {
            return $"{Id} {Label}";
        }
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                if (label != value)
                {
                    label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        public Etype EntityType
        {
            get { return entityType; }
            set
            {
                if (entityType != value)
                {
                    entityType = value;
                    OnPropertyChanged("EntityType");
                }
            }
        }

        public List<double> Temperature
        {
            get { return temperature; }
            set
            {
                if (temperature != value)
                {
                    temperature = value;
                    OnPropertyChanged("Temperature");
                }
            }
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.label))
            {
                this.ValidationErrors["Label"] = "Label name is required";
            }
            int parseId = 0;
            Int32.TryParse(this.id, out parseId);

            if (parseId < 0)
            {
                this.ValidationErrors["Id"] = "ID cannot be a negative number";
            }
            if (string.IsNullOrWhiteSpace(this.id))
            {
                this.ValidationErrors["Id"] = "ID is required";
            }
            foreach (Entity e in MainWindowViewModel.Entities)
            {
                if (e.id == this.id)
                {
                    this.ValidationErrors["Id"] = "ID Already exists";
                }
            }
        }

    }
    public class EntityByType : BindableBase
    {
        public Etype Etype { get; set; }
        private BindingList<Entity> entities;

        public BindingList<Entity> Entities
        {
            get { return entities; }
            set 
            { 
                entities = value;
                OnPropertyChanged("Entities");
            }
        }

        public EntityByType()
        {
            Entities = new BindingList<Entity>();
        }
    }
}
