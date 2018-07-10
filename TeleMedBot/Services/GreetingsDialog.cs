using BestMatchDialog;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;



namespace TeleMedBot.Services
{
    [Serializable]
    public class GreetingsDialog : BestMatchDialog<bool>
    {
        [BestMatch(new string[] { "Hi", "Hi There", "Hello There", "Hey", "Hello", "Hey There",
        "Hey Now", "Greetings", "Good morning", "Good afternoon", "Good evening",  "Good day" }, 
        threshold : 0.5, ignoreCase : true, ignoreNonAlphaNumericCharacters : true)]
        public async Task WelcomeGreeting (IDialogContext context, string messageText)
        {
            await context.PostAsync("Hello!  Shall we get started?  Perhaps I can get your next appointment or get a specific patient record for you?");
            context.Done(true);
        }

        [BestMatch(new string[] { "Confirm", "Save", "Looks Good", "Fine", "Good", "OK" },
        threshold: 0.5, ignoreCase: true, ignoreNonAlphaNumericCharacters: true)]
        public async Task ConfirmSave(IDialogContext context, string messageText)
        {
            await context.PostAsync("Thanks for confirming.  I'll save your changes");
            context.Done(true);
        }

        [BestMatch(new string[] { "What's up?", "Whats up", "What is going on?", "What is new?", "Sup?", "Hey "},
        threshold: 0.5, ignoreCase: true, ignoreNonAlphaNumericCharacters: true)]
        public async Task HowAreYouGreeting(IDialogContext context, string messageText)
        {
            await context.PostAsync("Hello! Not much. Shall we get started?  Perhaps I can get your next appointment or get a specific patient record for you?");
            context.Done(true);
        }

        [BestMatch(new string[] { "bye", "bye bye", "got to go", "see you later", "see ya", "GTG", "im out", "I'm out", "adios" },
        threshold: 0.5, ignoreCase: true, ignoreNonAlphaNumericCharacters: true)]
        public async Task FarewellGreeting(IDialogContext context, string messageText)
        {
            await context.PostAsync("Take care.  I look forward to your next request");
            context.Done(true);
        }

    }
}