using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TrueStory.Domain.Base;
using TrueStory.Domain.Base.Primitives;

namespace TrueStory.Domain.Aggregate.Product
{
    public class T_Product : BaseCommonObject<Guid>, IAggregateRoot
    {

        public string Name { get; private set; }

        public T_Product()
        {
        }

        public static T_Product Create(Guid ID,string name)
        {
            return new T_Product(ID, name);
        }

        public T_Product(Guid ID,string name)
        {
            Name = name;
            this.ID = ID;
        }
        public void Update(string name)
        {
            Name = name;
        }
    }
}
