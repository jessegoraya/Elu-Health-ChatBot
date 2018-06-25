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
    public class ComplaintForm
    {
        [Prompt(new string[] { "Briefly describe the Chief Complaint or what the Patent is presenting?" })]
        public string CCDescription { get; set; }

        [Prompt("State the location")]
        public string Location { get; set; }

        [Prompt("Descibe the onset (type NA if not applicable)?")]
        public string Onset { get; set; }

        [Prompt("Descibe the chronology (e.g. episodic, variable, constant, etc.)")]
        public string Chronology { get; set; }

        [Prompt("Describe the quality (e.g. sharp, dull, etc.) ")]
        public string Quality { get; set; }
    
        [Numeric(1,10)]
        [Prompt("Rate the severity (1 being least severe, 10 being most severe) ")]
        public double Severity { get; set; }

        [Prompt("State any modifying factors (i.e. activies, postures, medications that make it worse or better")]
        public string MF { get; set; }

        [Prompt("State any previous treatment taken")]
        public string Treatment { get; set; }

        public static IForm<ComplaintForm> BuildForm()
        {
            return new FormBuilder<ComplaintForm>()
                    .Field(nameof(CCDescription))
                    .Field(nameof(Location))
                    .Field(nameof(Onset))
                    .Field(nameof(Chronology))
                    .Field(nameof(Quality))
                    .Field(nameof(Severity))
                    .Field(nameof(MF))
                    .Field(nameof(Treatment))
                    .Build();
        }

    }
}