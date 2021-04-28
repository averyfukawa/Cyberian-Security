using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextComparison
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