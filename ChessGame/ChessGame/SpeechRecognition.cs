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
            this.recogniser.LoadGrammar(makeGridGrammer());
            this.recogniser.LoadGrammar(makeMenuGrammer());
            this.recogniser.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognisedHandler);
            this.recogniser.RecognizeAsync(RecognizeMode.Multiple);            
        }

        private static Grammar makeGridGrammer() {
            Choices gridLocAlpha = new Choices("alpha", "bravo", "charlie", "delta", "echo", "foxtrot", "golf", "hotel");
            Choices gridLocNumer = new Choices("one", "two", "three", "four", "five", "six", "seven", "eight");

            GrammarBuilder gridReference = new GrammarBuilder();
            gridReference.Append(gridLocAlpha);
            gridReference.Append(gridLocNumer);

            Choices direction = new Choices("to", "from");

            GrammarBuilder movement = new GrammarBuilder();
            movement.Append(direction);

            GrammarBuilder finalCommand = new GrammarBuilder();
            finalCommand.Append(gridReference);
            finalCommand.Append(direction);
            finalCommand.Append(gridReference);

            return new Grammar(finalCommand);
        }
        private static Grammar makeMenuGrammer() {
            Choices menuChoices = new Choices("begin", "instructions", "exit");

            GrammarBuilder options = new GrammarBuilder();
            options.Append(menuChoices);

            return new Grammar(options);
        }

        private void SpeechRecognisedHandler(object sender, SpeechRecognizedEventArgs args) {
            if (args.Result.Words.Count == 5) {    
                string gridPointAAlpha = ProcessToGridAlpha(args.Result.Words[0].Text);
                string gridPointANumer = ProcessToGridNumer(args.Result.Words[1].Text);
                string direction = args.Result.Words[2].Text;
                string gridPointBAlpha = ProcessToGridAlpha(args.Result.Words[3].Text);
                string gridPointBNumer = ProcessToGridNumer(args.Result.Words[4].Text);
                Console.WriteLine(gridPointAAlpha + " " + gridPointANumer + " " + direction + " " + gridPointBAlpha + " " + gridPointBNumer);
            } else {

            }


            Console.WriteLine("Recognized text: " + args.Result.Text);
        }

        string ProcessToGridAlpha(string speechText) {
            if (speechText == "alpha") {
                return "A";
            } else if (speechText == "bravo") {
                return "B";
            } else if (speechText == "charlie") {
                return "C";
            } else if (speechText == "delta") {
                return "D";
            } else if (speechText == "echo") {
                return "E";
            } else if (speechText == "foxtrot") {
                return "F";
            } else if (speechText == "golf") {
                return "G";
            } else if (speechText == "hotel") {
                return "H";
            }
            return "NULL";
        }

        string ProcessToGridNumer(string speechText) { 
            
            if (speechText == "one") {
                return "1";
            } else if (speechText == "two") {
                return "2";
            } else if (speechText == "three") {
                return "3";
            } else if (speechText == "four") {
                return "4";
            } else if (speechText == "five") {
                return "5";
            } else if (speechText == "six") {
                return "6";
            } else if (speechText == "seven") {
                return "7";
            } else if (speechText == "eight") {
                return "8";
            }
            return "NULL";


        }
    }
}
