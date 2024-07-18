using System;
using K4os.Compression.LZ4;

namespace Networking.Utils;

public static class LZ4
{
    public static ulong TotalUncompressed { get; private set; }
    public static ulong TotalCompressed { get; private set; }

    /// <summary>
    /// When false compression is disabled.
    /// </summary>
    private static bool UseCompression { get; set; } = true;

    /// <summary>
    /// When true the compression will decompress the data and compare it with the original data.
    /// Set to <i>FALSE</i> when you are sure the compression is working correctly and want to save some CPU and extra allocations
    /// </summary>
    public static bool SafetyCheck { get; set; } = false;

    public static byte[] Compress(byte[] data)
    {
        if (!UseCompression)
            return data;

        TotalUncompressed += (ulong)data.Length;

        var compressedSize = LZ4Codec.MaximumOutputSize(data.Length);
        var compressed = new byte[compressedSize];
        var compressedLength = LZ4Codec.Encode(
            data,
            0,
            data.Length,
            compressed,
            0,
            compressed.Length
        );

        TotalCompressed += (ulong)compressedLength;

        // Console.WriteLine(
        //     $"Raw: {data.Length}, Compressed: {compressedLength} ({compressedLength / (double)data.Length:P})"
        // );

        var result = new byte[compressedLength];
        Array.Copy(compressed, result, compressedLength);

        if (SafetyCheck)
        {
            var decompress = Decompress(result);
            if (data.Length != decompress.Length)
                throw new Exception();
        }

        return result;
    }

    public static byte[] Decompress(byte[] data)
    {
        if (!UseCompression)
            return data;

        var decompressed = new byte[1500 * 2];
        var decompressedLength = LZ4Codec.Decode(
            data,
            0,
            data.Length,
            decompressed,
            0,
            decompressed.Length
        );

        // Console.WriteLine(
        //     $"Raw: {data.Length}, Decompressed: {decompressedLength} ({decompressedLength / (double)data.Length:P})"
        // );

        var result = new byte[decompressedLength];
        Array.Copy(decompressed, result, decompressedLength);
        return result;
    }
}
