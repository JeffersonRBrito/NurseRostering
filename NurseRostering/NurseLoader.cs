namespace NurseRostering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    public class NurseLoader
    {
        const string X_PATH_NURSES = "Nurses/Nurse";
        const int ID = 0;
        const int NAME = 1;

        XmlDocument xmlDoc;

        public NurseLoader(string xmlPath)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
        }

        public List<Nurse> GetNurseList()
        {
            var nodes = xmlDoc.SelectNodes(X_PATH_NURSES);
            List<Nurse> nurses = new List<Nurse>();

            foreach(XmlNode nurseNode in nodes)
            {
                var id = nurseNode.ChildNodes[ID].InnerText;
                var name = nurseNode.ChildNodes[NAME].InnerText;
                nurses.Add(new Nurse(id, name));
            }
            
            return nurses;
        }
    }
}
