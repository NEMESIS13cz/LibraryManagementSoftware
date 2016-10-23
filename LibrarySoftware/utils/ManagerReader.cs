using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LibrarySoftware.utils
{
    // Třída pro práci s čtenáři
    // !! Důležité dodělat funkce tak, aby pracovala se serverem a opačně!!
    class ManagerReader
    {
        public ObservableCollection<Reader> Readers { get; private set; }

        public ManagerReader()
        {
            Readers = new ObservableCollection<Reader>();
        }
        public ManagerReader(ObservableCollection<Reader> Readers)
        {
            this.Readers = Readers;
        }

        public void AddNewReader(Reader reader)
        {
            Readers.Add(reader);
        }

        public void DeleteReader(Reader reader)
        {
            Readers.Remove(reader);
        }
    }
}
