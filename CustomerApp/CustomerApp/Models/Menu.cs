using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp.Models
{
    public class Menu : ObservableObject
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime? AvailableDate { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }


        int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                this.RaisePropertyChanged(p => p.Quantity);
            }
        }

        bool _overlayVisible;
        public bool OverlayVisible
        {
            get { return _overlayVisible; }
            set
            {
                _overlayVisible = value;                
                this.RaisePropertyChanged(p => p.OverlayVisible);
            }
        }


        #region Commands       

        #region InfoClickedCommand
        private Command _infoClickedCommand;
        public Command InfoClickedCommand
        {
            get { return _infoClickedCommand ?? (_infoClickedCommand = new Command(OnInfoClickedCommand)); }
        }

        private void OnInfoClickedCommand()
        {
            this.OverlayVisible = !this.OverlayVisible;
        }
        #endregion

        #region AddCommand
        private Command _addCommand;
        public Command AddCommand
        {
            get { return _addCommand ?? (_addCommand = new Command(OnAddCommand)); }
        }

        private void OnAddCommand()
        {
            this.Quantity++;

            if (App.Locator.MainViewModel.CurrentOrder.Meals.All(o => o != this) && this.Quantity > 0)
                App.Locator.MainViewModel.CurrentOrder.Meals.Add(this);
        }
        #endregion

        #region RemoveCommand
        private Command _removeCommand;
        public Command RemoveCommand
        {
            get { return _removeCommand ?? (_removeCommand = new Command(OnRemoveCommand)); }
        }

        private void OnRemoveCommand()
        {
            if (this.Quantity > 0)
                this.Quantity--;

            if (this.Quantity == 0)
                App.Locator.MainViewModel.CurrentOrder.Meals.Remove(this);            
        }
        #endregion
        #endregion
    }
}
