using System;
using System.Threading.Tasks;

namespace Quoridor.DataContracts
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Player
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid Id { get; set; }
    }
}
