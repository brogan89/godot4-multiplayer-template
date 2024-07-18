using Godot;
using MemoryPack;

namespace Networking.Utils;

public static class SceneMultiplayerEx
{
    public static void SendBytesCompressed(
        this SceneMultiplayer _multiplayer,
        byte[] rawData,
        int id,
        MultiplayerPeer.TransferModeEnum mode = MultiplayerPeer.TransferModeEnum.Reliable,
        int channel = 0)
    {
        var compress = LZ4.Compress(rawData);
        _multiplayer.SendBytes(compress, id, mode, channel);
    }

    public static NetMessage.ICommand DecompressMessage(this byte[] rawData)
    {
        var data = LZ4.Decompress(rawData);
        return MemoryPackSerializer.Deserialize<NetMessage.ICommand>(data);
    }
}