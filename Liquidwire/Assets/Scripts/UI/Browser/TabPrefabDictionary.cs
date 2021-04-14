using System;
using UnityEngine;

namespace UI.Browser
{
    // class to store id of prefabs and tabs.
    [Serializable]
    public class TabPrefabDictionary
    {
        private float _id;
        public GameObject prefab;


        public TabPrefabDictionary( GameObject prefab)
        {
            this.prefab = prefab;
        }

        public float GetId()
        {
            return _id;
        }

        public void SetId()
        {
            string temp = prefab.name.Split(' ')[1];
            string[] tempArr = temp.Split('.');
            string key = tempArr[0] + "," + tempArr[1];
            this._id = (float.Parse(key));
        }
    }
}