using System;
using System.Collections.Generic;
using TextComparison;

namespace Games.TextComparison.Artificial_dictionary_scripts
{
    [Serializable]
    // This is a artificial dictionary so that we can see it in the inspector as well
    public class DcList
    {
        public List<Discrepancy> dcList;


        public DcList(List<Discrepancy> dclist)
        {
            this.dcList = dclist;
        }
    }
    
}