using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Windows;

namespace ChessGame {
    class SpeechRecognition {
        //declare speech engine
        private readonly SpeechRecognitionEngine recogniser;

        public SpeechRecognition() {
            //setup speech engine and set to use default audio device
            this.recogniser = new SpeechRecognitionEngine();
            this.recogniser.SetInputToDefaultAudioDevice();

            //load grammar objects into speech engine
            this.recogniser.LoadGrammar(makeGridGrammer());
            this.recogniser.LoadGrammar(makeMenuGrammer());

            //add event handler to call when speech is recognised
            this.recogniser.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognisedHandler);

            //set the speech engine to asynchronos
            this.recogniser.RecognizeAsync(RecognizeMode.Multiple);
        }

        private static Grammar makeGridGrammer() {
            //define grid position grammar builder choices and add to the grid reference grammar builder
            Choices gridLocAlpha = new Choices("alpha", "bravo", "charlie", "delta", "echo", "foxtrot", "golf", "hotel");
            Choices gridLocNumer = new Choices("one", "two", "three", "four", "five", "six", "seven", "eight");

            GrammarBuilder gridReference = new GrammarBuilder();
            gridReference.Append(gridLocAlpha);
            gridReference.Append(gridLocNumer);

            //define direction choices and add to the grammar builder movement
            Choices direction = new Choices("to", "from");

            GrammarBuilder movement = new GrammarBuilder();
            movement.Append(direction);

            //define the grammar as a command using a grid reference, direction then another grid reference.
            GrammarBuilder finalCommand = new GrammarBuilder();
            finalCommand.Append(gridReference);
            finalCommand.Append(direction);
            finalCommand.Append(gridReference);

            return new Grammar(finalCommand);
        }
        private static Grammar makeMenuGrammer() {
            //define menu grammar choices
            Choices menuChoices = new Choices("start", "instructions", "exit");

            GrammarBuilder options = new GrammarBuilder();
            options.Append(menuChoices);

            return new Grammar(options);
        }

        private void SpeechRecognisedHandler(object sender, SpeechRecognizedEventArgs args) {
            //define access back to the main window.
            var main = Application.Current.MainWindow as MainWindow;

            // if speech is 5 words then it is a movement command
            if (args.Result.Words.Count == 5) {
                //calculate the position is a more computable grid reference (EG: A 1 to B 5).
                string gridPointAAlpha = ProcessToGridAlpha(args.Result.Words[0].Text);
                string gridPointANumer = ProcessToGridNumer(args.Result.Words[1].Text);
                string direction = args.Result.Words[2].Text;
                string gridPointBAlpha = ProcessToGridAlpha(args.Result.Words[3].Text);
                string gridPointBNumer = ProcessToGridNumer(args.Result.Words[4].Text);
                Console.WriteLine(gridPointAAlpha + " " + gridPointANumer + " " + direction + " " + gridPointBAlpha + " " + gridPointBNumer);

                //if the direction is moving peice A to Peice B then pass as such otherwise if the direction is moving peice A FROM peice B then reverse the direction;
                if (direction == "to") {
                    main.tryMovePeice(gridPointAAlpha + gridPointANumer, gridPointBAlpha + gridPointBNumer);
                } else if (direction == "from") {
                    main.tryMovePeice(gridPointBAlpha + gridPointBNumer, gridPointAAlpha + gridPointANumer);

                }
            } else {
                if (args.Result.Text == "exit") {
                    main.Close();
                } else if (args.Result.Text == "start") {
                    main.pressStart();
                }
            }
            //print the recognised text
            Console.WriteLine("Recognized text: " + args.Result.Text);
        }

        string ProcessToGridAlpha(string speechText) {
            //Convert alpha string into correct letter reference for easier processing
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
            //convert from word version numbers to actual numbers but still stored as strings for easy position passing.
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
