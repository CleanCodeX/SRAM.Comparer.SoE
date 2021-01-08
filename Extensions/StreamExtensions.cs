using System.IO;
using System.Text;

namespace SramComparer.SoE.Extensions
{
	public static class StreamExtensions
	{
		public static MemoryStream GetStreamSlice(this Stream source, int length) => source.GetStreamSlice(0, length);
		public static MemoryStream GetStreamSlice(this Stream source, int streamPosition, int length)
		{
			byte[] buffer = new byte[length];
			var br = new BinaryReader(source, Encoding.ASCII, true);

			source.Position = streamPosition;

			br.Read(buffer, 0, length);

			return new MemoryStream(buffer);
		}
    }
}
