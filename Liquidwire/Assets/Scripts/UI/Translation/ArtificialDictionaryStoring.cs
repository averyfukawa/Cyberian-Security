using System.Collections.Generic;
using UnityEngine;

namespace UI.Translation
{
    public class ArtificialDictionaryStoring
    {
        private GameObject prefab;
        private List<string> names;

        public ArtificialDictionaryStoring(GameObject prefab, List<string> names)
        {
            this.names = names;
            this.prefab = prefab;
        }

        public GameObject prefab1 => prefab;

        public List<string> names1 => names;
    }
}