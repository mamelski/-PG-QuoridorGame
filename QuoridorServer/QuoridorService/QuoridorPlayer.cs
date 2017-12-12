namespace QuoridorService
{
    using System;

    /// <summary>
    /// The quoridor player.
    /// </summary>
    public class QuoridorPlayer
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is online.
        /// </summary>
        public bool IsOnline { get; set; }
    }
}