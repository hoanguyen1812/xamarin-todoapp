using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class ItemViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public ItemViewModel()
        {

        }
        public ItemViewModel(Item item)
        {
            Id = item.Id;
            _title = item.Title;
            _notes = item.Notes;
        }

        private string _title;
        public string Title
        {
            get { return _title;}
            set
            {
                SetValue(ref _title, value);
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes;}
            set
            {
                SetValue(ref _notes, value);
                OnPropertyChanged(nameof(Notes));
            }
        }
    }
}
