using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextComparison
{
    [Serializable]
    public class DcList
    {
        public List<Discrepancie> dcList;


        public DcList(List<Discrepancie> dclist)
        {
            this.dcList = dclist;
        }
    }
    
}