namespace QuoridorService.DataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The position.
    /// </summary>
    [DataContract]
    public class Position
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        [DataMember]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        [DataMember]
        public int Y { get; set; }
    }
}