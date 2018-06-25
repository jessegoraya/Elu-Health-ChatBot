using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using TeleMedBot.Services;

namespace TeleMedBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                //updated this to include activity text looking for Update Patient so that it can be converted to 
                if (activity.Type == ActivityTypes.Message)
                {
                    await Conversation.SendAsync(activity, () => new TeleMedBot.Services.PatientServiceLUIS());
                    // LUIS NEW URL (2017-03-28): https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/5e651f26-a8a2-46ad-8dbb-ef6131479b8e?subscription-key=2e99cdefeea342a0bcd7f00f527c2037&timezoneOffset=0.0&verbose=true&q=
                    // LUIS OLD URL: https://api.projectoxford.ai/luis/v1/application?id=5e651f26-a8a2-46ad-8dbb-ef6131479b8e&subscription-key=22141386d4d9486aa5e1bc9d6b032c2c

                }
                else
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    var reply = HandleSystemMessage(activity);
                    if (reply != null)
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    //HandleSystemMessage(activity);
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
            else if (message.Type == ActivityTypes.Event)
            {
                //from web chat where 
                //Activity reply;
                //reply = CreateEvent("UpdatePatient", message);
            }

            return null;
        }

        //private Activity CreateEvent(String eventName, Activity message)
        //{
        //    Activity eventMsg = new Activity();
        //    eventMsg.Type = "event";
        //    eventMsg.Name = eventName;
        //    eventMsg.Value = message.Text;
        //    return eventMsg;

        //}
    }
}