namespace SamSharp
{
    public readonly struct Options
    {
        public readonly byte Pitch;
        public readonly byte Mouth;
        public readonly byte Throat;
        public readonly byte Speed;
        public readonly bool SingMode;

        public Options(byte pitch = 64, byte mouth = 128, byte throat = 128, byte speed = 72, bool singMode = false)
        {
            Pitch = pitch;
            Mouth = mouth;
            Throat = throat;
            Speed = speed;
            SingMode = singMode;
        }
    }
}