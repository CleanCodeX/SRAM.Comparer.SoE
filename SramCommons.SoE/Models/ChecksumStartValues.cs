namespace SramCommons.SoE.Models
{
    public class ChecksumStartValues
    {
        /// the starting value for the checksum in the US version
        // ReSharper disable once InconsistentNaming
        public const uint US = 1_087;

        /// the starting value for the checksum in the European versions
        public const uint Europe = 5_887;
    }
}