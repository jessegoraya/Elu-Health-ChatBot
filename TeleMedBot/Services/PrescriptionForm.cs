using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Threading;
using System.Threading.Tasks;
using Person.Models;
using Client_EMR.Services;
using Microsoft.Bot.Builder.Luis;

namespace TeleMedBot.Services
{
    [Serializable]
    public class PrescriptionForm
    {
        [Prompt(new string[] {"What are you presecribing?"})]
        public string Medicine { get; set; }

        [Prompt("How much would you like to prescribe?")]
        public string Doseage { get; set; }

        public static IForm<PrescriptionForm> BuildForm()
        {
            return new FormBuilder<PrescriptionForm>()
                    .Field(nameof(Medicine))
                    .Field(nameof(Doseage))
                    .Build();
         }
    }
}