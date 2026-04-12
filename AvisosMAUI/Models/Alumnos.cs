using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AvisosMAUI.Models
{
    public class Alumno : INotifyPropertyChanged
    {
        public string Nombre { get; set; }
        public string Matricula { get; set; }

        private bool isMenuVisible;


        public bool IsMenuVisible
        {
            get => isMenuVisible;
            set
            {
                if (isMenuVisible != value)
                {
                    isMenuVisible = value;
                    OnPropertyChanged(nameof(IsMenuVisible));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
