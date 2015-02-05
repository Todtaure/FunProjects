using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FimFictionHistoryBrowser
{
    public class Locator
    {
        private static DataReader _dataReader;
        //private static UserViewModel _user;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public Locator()
        {
        }

        /// <summary>
        /// Return Exercise ViewModel object
        /// </summary>
        public DataReader DataReader
        {
            get { return _dataReader ?? (_dataReader = new DataReader()); }
        }
    }
}
