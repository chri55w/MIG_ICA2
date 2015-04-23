using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace ChessGame
{
    class SpeechRecognition {

        MainWindow m_window;

        private readonly SpeechRecognitionEngine recogniser;

        public SpeechRecognition(MainWindow window) {
            m_window = window;
            this.recogniser = new SpeechRecognitionEngine();
            this.recogniser.SetInputToDefaultAudioDevice();
            this.recogniser.LoadGrammar(makeChessGrammer());
            this.recogniser.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognisedHandler);
            this.recogniser.RecognizeAsync(RecognizeMode.Multiple);            
        }

        private static Grammar makeChessGrammer() {
            Choices gridLocAlpha = new Choices("a", "b", "c", "d", "e", "f", "g", "h");
            Choices gridLocNumer = new Choices("1", "2", "3", "4", "5", "6", "7", "8");

            GrammarBuilder gridReference = new GrammarBuilder();
            gridReference.Append(gridLocAlpha);
            gridReference.Append(gridLocNumer);

            return new Grammar(gridReference);
        }

        private void SpeechRecognisedHandler(object sender, SpeechRecognizedEventArgs args)
        {
            Console.WriteLine("Recognized text: " + args.Result.Text);
        }
    }
}
