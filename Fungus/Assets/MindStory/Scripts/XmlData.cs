using UnityEngine;

namespace MindStory
{
    public class XmlData : ScriptableObject
    {
        public string fileName;
        private byte[] content;

        public byte[] Content
        {
            get { return content; }
            set { content = value; }
        }
    }

}
