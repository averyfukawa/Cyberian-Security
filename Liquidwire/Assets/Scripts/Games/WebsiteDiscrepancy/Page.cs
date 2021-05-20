using UnityEngine;

namespace Games.WebsiteDiscrepancy
{
    public class Page : MonoBehaviour
    {
        private int _id;
        public string _tabText;

        public void ChangeActive()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
