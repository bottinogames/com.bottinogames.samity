using System.Threading.Tasks;

namespace SamSharp
{
    public class Sam
    {
        public Options Options { get; set; }

        public bool loggingEnabled = false;
        
        public Sam(Options options, bool enableLogging = true)
        {
            Options = options;
            loggingEnabled = enableLogging;
        }

        public Sam() : this(new Options())
        {
        }

        public byte[] Speak(string input)
        {
            Reciter.Reciter reciter = new Reciter.Reciter();

            if (loggingEnabled)
                LoggingContext.OpenContext("Reciter");
            string phonemes = reciter.TextToPhonemes(input);
            LoggingContext.CloseAndLogContext();
            
            return SpeakPhonetic(phonemes);
        }

        public byte[] SpeakPhonetic(string phoneticInput)
        {
            Parser.Parser parser = new Parser.Parser();
            Renderer.Renderer renderer = new Renderer.Renderer();
            
            if (loggingEnabled)
                LoggingContext.OpenContext("Parser");
            var data = parser.Parse(phoneticInput);
            LoggingContext.CloseAndLogContext();

            if (loggingEnabled)
                LoggingContext.OpenContext("Renderer");
            byte[] output = renderer.Render(data, Options);
            LoggingContext.CloseAndLogContext();

            return output;
        }

        public Task<byte[]> SpeakAsync(string input)
        {
            return Task<byte[]>.Factory.StartNew(() => Speak(input));
        }

        public Task<byte[]> SpeakPhoneticAsync(string phoneticInput)
        {
            return Task<byte[]>.Factory.StartNew(() => SpeakPhonetic(phoneticInput));
        }
    }
}