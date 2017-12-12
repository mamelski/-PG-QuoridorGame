namespace Quoridor.DataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The move.
    /// </summary>
    [DataContract]
    public class Move
    {
        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        [DataMember]
        public Position Destination { get; set; }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        [DataMember]
        public Player Player { get; set; }
    }
}