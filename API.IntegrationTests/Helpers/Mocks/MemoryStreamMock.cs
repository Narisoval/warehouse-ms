namespace API.IntegrationTests.Helpers.Mocks;

public class MemoryStreamMock : Stream
{
    private long _position;
    private readonly long _length;

    public MemoryStreamMock(long length)
    {
        _length = length;
    }

    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;
    public override long Length => _length;
    public override long Position { get => _position; set => _position = value; }
    public override void Flush() { }
    public override int Read(byte[] buffer, int offset, int count)
    {
        int bytesRead = (int)Math.Min(count, _length - _position);
        _position += bytesRead;
        return bytesRead;
    }
    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                _position = offset;
                break;
            case SeekOrigin.Current:
                _position += offset;
                break;
            case SeekOrigin.End:
                _position = _length + offset;
                break;
        }
        return _position;
    }
    public override void SetLength(long value) { }
    public override void Write(byte[] buffer, int offset, int count) { }
}
