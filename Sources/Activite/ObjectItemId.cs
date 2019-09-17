
namespace Orpea_WF.Activite
{
    public class ObjectItemId
    {
        private string Id   { get; }
        private string Name { get; }

        public ObjectItemId(string id, string name)
        {
            Id   = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Value => Id;
    }
}
