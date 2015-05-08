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
        #region AddToCardCommand
        private Command _addToCardCommand;
        public Command AddToCardCommand
        {
            get { return _addToCardCommand ?? (_addToCardCommand = new Command(OnAddToCardCommand)); }
        }

        private void OnAddToCardCommand()
        {            
            if(this.Quantity>0)
            {
                this.OverlayVisible = !this.OverlayVisible;
            }
        }
        #endregion


        #region MenuClickedCommand
        private Command _menuClickedCommand;
        public Command MenuClickedCommand
        {
            get { return _menuClickedCommand ?? (_menuClickedCommand = new Command(OnMenuClickedCommand)); }
        }

        private void OnMenuClickedCommand()
        {
            this.OverlayVisible = true;
        }
        #endregion

        #region CloseOverlayCommand
        private Command _closeOverlayCommand;
        public Command CloseOverlayCommand
        {
            get { return _closeOverlayCommand ?? (_closeOverlayCommand = new Command(OnCloseOverlayCommand)); }
        }

        private void OnCloseOverlayCommand()
        {
            this.OverlayVisible = false;
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
            this.Quantity--;
        }
        #endregion
        #endregion
    }
}
