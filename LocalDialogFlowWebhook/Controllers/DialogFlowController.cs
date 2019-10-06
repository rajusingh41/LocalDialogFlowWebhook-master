using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LocalDialogFlowWebhook
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/DialogFlow")]
    [AllowAnonymous]
    public class DialogFlowController : ApiController
    {
        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]

        public async Task<HttpResponseMessage> WebhookResponse()
        {
            try
            {
                WebhookRequest request;

                using (var stream = await Request.Content.ReadAsStreamAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        request = jsonParser.Parse<WebhookRequest>(reader);
                    }
                }
                try
                {
                    Logger.Info(request);
                    var actionType = request.QueryResult.Action;
                    var parameters = request.QueryResult.Parameters;

                    var fulfilmentMessage = request.QueryResult.FulfillmentMessages;

                    var details = JObject.Parse(request.OriginalDetectIntentRequest.ToString());
                    // var jwtToken = (string)details["payload"]["Authorization"];

                    var domainCode = "dgn5";

                    var userName = "Raju"; // LogOnId


                    HttpContext.Current.Request.Headers.Add("domainCode", domainCode);

                    var userId = 10;
                    var webhookResponse = new WebhookResponse();
                    var response = Request.CreateResponse();
                    switch (actionType)
                    {
                        case "input.welcome":
                            var answer = $"Hi {userName}, I am joy, your buddy.\nHow can i assist you wtih today?";
                            var welcomeMessage = WelcomeMessage.Message(answer);
                            var payload = Struct.Parser.ParseJson(welcomeMessage);
                            webhookResponse.FulfillmentText = answer;
                            webhookResponse.FulfillmentMessages.Add(new Intent.Types.Message
                            {
                                Payload = payload,
                            });
                            return HttpResponceMessage(webhookResponse, response);
                        case "input.helpDesk":

                            var categories = FeedbackData.GetCategroyData();
                            var helpdeskAnswer = $"Sure. What is the category of helpdesk ticket?";
                            var helpdeskMessage = HelpdeskMessage.CategoryMessage(helpdeskAnswer, categories);
                            var helpdeskPayload = Struct.Parser.ParseJson(helpdeskMessage);
                            webhookResponse.FulfillmentText = helpdeskAnswer;
                            webhookResponse.FulfillmentMessages.Add(new Intent.Types.Message
                            {
                                Payload = helpdeskPayload,
                            });
                            return HttpResponceMessage(webhookResponse, response);
#pragma warning disable S1871 // Two branches in a conditional structure should not have exactly the same implementation
                        case "HelpDeskTicket.HelpDeskSubCategory":

                            var categoryName = parameters.Fields["categoryId"].StringValue;
                            var subCategories = FeedbackData.GetSubcategories()
                                                            .Where(x => x.CategoryName.ToLower() == categoryName.ToLower()).ToList();
                            var subHelpdeskAnswer = $"What is the sub category of ticket?";
                            var subHelpdeskMessage = HelpdeskMessage.SubCategoryMessage(subHelpdeskAnswer, subCategories);
                            var subHelpdeskPayload = Struct.Parser.ParseJson(subHelpdeskMessage);
                            webhookResponse.FulfillmentText = subHelpdeskAnswer;
                            webhookResponse.FulfillmentMessages.Add(new Intent.Types.Message
                            {
                                Payload = subHelpdeskPayload,
                            });
                            return HttpResponceMessage(webhookResponse, response);
                        case "HelpDeskTicketPriority":

                            var priorities = FeedbackData.GetTicktPriortiy();
                            var priorityHelpdeskAnswer = $"Can you please specify the priority of the issue?";
                            var priorityHelpdeskMessage = HelpdeskMessage.Priority(priorityHelpdeskAnswer, priorities);
                            var priorityHelpdeskPayload = Struct.Parser.ParseJson(priorityHelpdeskMessage);
                            webhookResponse.FulfillmentText = priorityHelpdeskAnswer;
                            webhookResponse.FulfillmentMessages.Add(new Intent.Types.Message
                            {
                                Payload = priorityHelpdeskPayload,
                            });
                            return HttpResponceMessage(webhookResponse, response);

                        case "HelpDeskTicketBooked":

                            webhookResponse.FulfillmentText = "Ticket has been raised. Helpdesk ticket id is 62359!";
                            return HttpResponceMessage(webhookResponse, response);

                        default:
                            webhookResponse.FulfillmentText = "Greetings from our Webhook API!";
                            return HttpResponceMessage(webhookResponse, response);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    Logger.Info(request);
                    return null;
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// HttpResponceMessage
        /// </summary>
        /// <param name="webhookResponse"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private static HttpResponseMessage HttpResponceMessage(WebhookResponse webhookResponse, HttpResponseMessage response)
        {
            response.Content = new StringContent(webhookResponse.ToString());
            response.StatusCode = HttpStatusCode.OK;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }
    }
}
