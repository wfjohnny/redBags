using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Domain.Entities
{
    public class EntityStr : EntityBase<string>
    {

        public EntityStr()
        {
            base.Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
