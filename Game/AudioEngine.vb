Imports SharpDX.XAudio2
Imports SharpDX.Multimedia
Imports System.IO

Public Module AudioEngine

    Private xaudio As XAudio2
    Private masterVoice As MasteringVoice

    Private soundBuffers As New Dictionary(Of String, AudioBuffer)
    Private waveFormats As New Dictionary(Of String, WaveFormat)
    Private activeVoices As New Dictionary(Of Integer, SourceVoice)
    Private nextHandle As Integer = 1

    Public Sub Initialize()
        xaudio = New XAudio2()
        masterVoice = New MasteringVoice(xaudio)
    End Sub

    Public Sub LoadSound(id As String, stream As SoundStream)
        Dim buffer = New AudioBuffer With {
            .AudioBytes = CInt(stream.Length),
            .Stream = stream.ToDataStream(),
            .Flags = BufferFlags.EndOfStream
        }
        soundBuffers(id) = buffer
        waveFormats(id) = stream.Format
    End Sub

    Public Sub PlayOneShot(id As String, volume As Single)
        Dim voice = New SourceVoice(xaudio, waveFormats(id))
        voice.SetVolume(volume)
        voice.SubmitSourceBuffer(soundBuffers(id), Nothing)
        voice.Start()
    End Sub

    Public Function PlayLoop(id As String, volume As Single) As Integer
        Dim voice = New SourceVoice(xaudio, waveFormats(id))
        voice.SetVolume(volume)

        Dim buffer = soundBuffers(id)
        buffer.LoopCount = AudioBuffer.LoopInfinite

        voice.SubmitSourceBuffer(buffer, Nothing)
        voice.Start()

        Dim handle = nextHandle
        nextHandle += 1
        activeVoices(handle) = voice

        Return handle
    End Function

    Public Sub StopLoop(handle As Integer)
        If activeVoices.ContainsKey(handle) Then
            activeVoices(handle).Stop()
            activeVoices(handle).DestroyVoice()
            activeVoices(handle).Dispose()
            activeVoices.Remove(handle)
        End If
    End Sub


End Module

