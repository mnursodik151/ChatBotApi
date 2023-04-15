using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Vorbis;

public static class AudioConversionUtils
{
    public static async Task<MemoryStream> ConvertToOggAsync(byte[] audioData)
    {
        using var inputStream = new MemoryStream(audioData);
        using var waveStream = new WaveFileReader(inputStream);
        using var vorbisStream = new VorbisWaveReader(waveStream);

        var outputMemoryStream = new MemoryStream();
        int bufferSize = 4096;
        byte[] buffer = new byte[bufferSize];

        int bytesRead;
        while ((bytesRead = await vorbisStream.ReadAsync(buffer, 0, bufferSize)) > 0)
        {
            await outputMemoryStream.WriteAsync(buffer, 0, bytesRead);
        }

        outputMemoryStream.Position = 0; // Reset the stream position to the beginning
        return outputMemoryStream;
    }
}
