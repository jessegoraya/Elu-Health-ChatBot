using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using Person.Models;
using Client_EMR.Services;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace TeleMedBot.Services
{

    [LuisModel("5e651f26-a8a2-46ad-8dbb-ef6131479b8e", "2e99cdefeea342a0bcd7f00f527c2037", LuisApiVersion.V2)]
    [Serializable]
    public class PatientServiceLUIS : LuisDialog<object>
    {
        [LuisIntent("See patient record")]
        public async Task SeePatientRecord(IDialogContext context, LuisResult result)
        {
            string patientName = string.Empty;
            string replyText = string.Empty;
            List<Person.Models.Person> People = new List<Person.Models.Person>();

            try
            {
                if (result.Entities.Count > 0)
                {
                    patientName = result.Entities.FirstOrDefault(e => e.Type == "Patient").Entity;

                    if (!string.IsNullOrWhiteSpace(patientName))
                    {
                        //call API to get Patient here
                        //right now just getting all patients of a service call i know works. 
                        //after this go back to a API
                        People = await ServiceWrapper.GetPersonByName(patientName);
                    }

                    try
                    {
                        //need to add patients to a carousel of cards
                        if (People.Count >= 1)
                        {
                            replyText = $"I found the following patients that are a fit... \n\n";
                            foreach (var Person in People)
                            {
                                replyText += $"{Person.FName} {Person.LastName}\n\n";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        replyText = "Sorry I didn't find a file for  " + patientName;
                    }
                    await context.PostAsync(replyText);
                }
            }
            catch (Exception)
            {
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
            }

        }

        []

        [LuisIntent("Update Patient")]
        public async Task UpdatePatient(IDialogContext context, LuisResult result)
        {
            try
            {
                string patientName = string.Empty;
                string replyText = string.Empty;

                //adding this to test pulling a name.
                List<Person.Models.Person> PeopleResults = new List<Person.Models.Person>();
                PeopleResults = await ServiceWrapper.GetPersonByName("Chris Baker");

                Person.Models.Person UpdPerson = new Person.Models.Person();
                UpdPerson = PeopleResults.First();

                try
                {
                    //extract height and weight from language
                    if (result.Entities.Count > 0)
                    {
                        UpdPerson.ChatHeight = "";
                        UpdPerson.ChatWeight = 0;

                        EntityRecommendation Heightfeet;
                        if (result.TryFindEntity("heightfeet", out Heightfeet))
                        {
                            UpdPerson.ChatHeight = Heightfeet.Entity;
                        }

                        EntityRecommendation Heightinches;
                        if (result.TryFindEntity("heightinches", out Heightinches))
                        {
                            UpdPerson.ChatHeight += "'" +  Heightinches.Entity;
                        }

                        EntityRecommendation Weight;
                        if (result.TryFindEntity("weight", out Weight))
                        {
                            UpdPerson.ChatWeight = Convert.ToInt32(Weight.Entity);
                        }

                    }

                    //call Person API to update with Chat content
                    string workflowid = await ServiceWrapper.UpdatePerson(UpdPerson);

                    //IMessageActivity msg = Activity.CreateMessageActivity();
                    IMessageActivity msg = context.MakeMessage();
                    msg.Type = "event";
                    msg.Value = "Update Patient";
                    msg.Text = "Patient updated on the right. Confirm and/or make additonal changes in the form and then type confirm";


                    await context.PostAsync(msg.Text);
                    context.Done(workflowid);
                }
                catch (Exception)
                {
                    await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                    context.Wait(MessageReceived);
                }

            }
            catch (Exception e)
            { 
                await context.PostAsync(e.InnerException.Message);
                context.Wait(MessageReceived);
            }
        }


        [LuisIntent("Prescription Wizard")]
        public async Task PrescriptionWizard(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("OK Let's fill the Prescription");
                var prescriptionForm = new FormDialog<PrescriptionForm>(new PrescriptionForm(), PrescriptionForm.BuildForm, FormOptions.PromptInStart);
                context.Call(prescriptionForm, PrescriptionFormComplete);
            }
            catch (Exception)
            {
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Complaint Wizard")]
        public async Task ComplaintWizard(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("OK Let's add the Chief Complaint");
                var complaintForm = new FormDialog<ComplaintForm>(new ComplaintForm(), ComplaintForm.BuildForm, FormOptions.PromptInStart);
                context.Call(complaintForm, ComplaintFormComplete);
            }
            catch (Exception)
            {
                await context.PostAsync("Something really bad happened. You can try again later meanwhile I'll check what went wrong.");
                context.Wait(MessageReceived);
            }
        }



        private async Task PrescriptionFormComplete(IDialogContext context, IAwaitable<PrescriptionForm> result)
        {
            try
            {
                var feedback = await result;
                // message = GenerateEmailMessage(feedback);
                var success = true;
                //fire a workflow to add the prescription for the patient and if it successfully fired send the result back
                // var success = await EmailSender.SendEmail(recipientEmail, senderEmail, $"Email from {feedback.Name}", message);
                if (!success)
                    await context.PostAsync("Sorry something got jacked up.");
                else
                {
                    await context.PostAsync("The prescription has been added.");
                    await context.PostAsync("I can add another prescription simply just type 'Add Presciprtion', or we can add other items to the Assessment by typing 'Add Lab Order' 'Add Referral'");
                }
            }

            catch (FormCanceledException)
            {
                await context.PostAsync("Totally fine.  We can add the prescription later.");
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry something got jacked up.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }

        private async Task ComplaintFormComplete(IDialogContext context, IAwaitable<ComplaintForm> result)
        {
            try
            {
                var feedback = await result;
                var success = true;

                if (!success)
                    await context.PostAsync("Sorry something got jacked up.");
                else
                {
                    await context.PostAsync("The Chief Complaint has been added");
                    await context.PostAsync("To add another complaint, simply enter 'Add CC' or we can move on to other areas such as 'Enter Vitals' or if they have alredy been entered 'Show me vitals'");
                }
            }

            catch (FormCanceledException)
            {
                await context.PostAsync("Totally fine.  We can add the complaint later.");
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry something got jacked up.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Get Next Appointment")]
        public async Task GetNextAppt(IDialogContext context, LuisResult result)
        {
            try
            {
                //you have determined that the user is intending on finding their next patient \
                //so call the workflow function within the ServiceWrapper cs

                Person.Models.Person NextApptPatient = new Person.Models.Person();
                NextApptPatient = await ServiceWrapper.GetNextApptforDoc("GeannieandNicky", "Discharge", "1");
                //string replyText = "Your next appointment is with " + NextApptPatient.FName + " " + NextApptPatient.LastName + " at " + NextApptPatient.NextAppt.ToString();
                //create hero image with reply...


                Activity nextApptCard = (Activity)context.MakeMessage();

                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: "http://img.s-msn.com/tenant/amp/entityid/AA2snpK?m=2&w=480&h=480&f=PNG&h=480&w=480&m=6&q=60&o=t&l=f"));

                nextApptCard.Recipient = nextApptCard.Recipient;
                nextApptCard.Type = "message";
                nextApptCard.Attachments = new List<Attachment>();

                ThumbnailCard patientCard = new ThumbnailCard()
                {
                    Title = "Your next appt. is with  " + NextApptPatient.FName + " " + NextApptPatient.LastName + ".",
                    //Subtitle = "Appointment Date/Time: " + NextApptPatient.NextAppt.ToString(),
                    Images = cardImages
                };

                Attachment patientAttachment = patientCard.ToAttachment();
                nextApptCard.Attachments.Add(patientAttachment);

                //change replyText to activity variable in order to send a card back
                await context.PostAsync(nextApptCard);
                Thread.Sleep(3000);
                //20170411 - Here I need to add code to start a child dialog asking user to set the context of the current user or not
                await context.PostAsync("If you are ready to start logging the visit for this patient, just tell me to add CC");
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, we couldn't find your next appointment or you don't have one");
            }
        }

        //Capture Symptom - Identify request to capture a symptom, get the symptom area, indicication and/or environment
        //then call a Capture Symptom workflow
        //[LuisIntent("Create Symptom")]
        //public async Task AddNewSymptom(IDialogContext context, LuisResult result)
        //{
        //    try
        //    {

        //    }
        //    catch
        //    {

        //    }
        //}

        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            try
            {
                var cts = new CancellationTokenSource();
                await context.Forward(new GreetingsDialog(), GreetingDialogDone, await message, cts.Token);
            }
            catch
            {
                await context.PostAsync("I'm sorry but your request was not understood.  Please try again.");
            }
        }

        private async Task GreetingDialogDone(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;
            if (!success)
                await context.PostAsync("I'm sorry. I didn't understand you.");

            context.Wait(MessageReceived);
        }
    }
}