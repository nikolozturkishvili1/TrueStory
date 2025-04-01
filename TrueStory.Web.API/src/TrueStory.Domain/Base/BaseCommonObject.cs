using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueStory.Domain.Base
{
    public abstract class BaseCommonObject<TKey> : Entity<TKey>
    {

        public DateTime CreateDate { get; private set; }
        public DateTime? DateModified { get; private set; }

        public bool IsDeleted { get; private set; }

        /// <summary>
        /// set create date when object is created
        /// </summary>
        public void SetCreateDate()
        {
            CreateDate = DateTime.Now;
        }

        public void SetDateModified()
        {
            DateModified = DateTime.Now;
        }

        public void Delete()
        {
            IsDeleted = true;
        }   
    }

    public abstract class BaseCommonObject : BaseCommonObject<Guid>
    {
       
    }
}
