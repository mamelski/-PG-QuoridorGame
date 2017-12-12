namespace SendObjectTest
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Sending this event is equal to ending the turn
    /// </summary>
    [DataContract]

    public class BoardEvent
    {
        /// <summary>
        /// Gets or sets the move.
        /// </summary>
        [DataMember]
        public MoveDirection Move { get; set; }

        /// <summary>
        /// Gets or sets the sender id.
        /// </summary>
        [DataMember]
        public int SenderId { get; set; }

        /// <summary>
        /// Gets or sets the board event type.
        /// </summary>
        [DataMember]
        public BoardEventType BoardEventType { get; set; }
    }
}
