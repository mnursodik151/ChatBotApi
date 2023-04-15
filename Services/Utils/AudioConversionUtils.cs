using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Vorbis;

public static class AudioConversionUtils
{
    public static int GetWavDurationInSeconds(MemoryStream memoryStream)
    {
        memoryStream.Position = 0; // Reset the memory stream position
        using var waveStream = new WaveFileReader(memoryStream);
        return (int)waveStream.TotalTime.TotalSeconds;
    }

    public static int GetOggDurationInSeconds(MemoryStream oggStream)
    {
        oggStream.Position = 0; // Reset the memory stream position
        using var vorbisReader = new VorbisWaveReader(oggStream);
        return (int)vorbisReader.TotalTime.TotalSeconds;
    }
    
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
