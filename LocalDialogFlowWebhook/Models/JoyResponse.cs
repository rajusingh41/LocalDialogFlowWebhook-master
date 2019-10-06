using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalDialogFlowWebhook
{
    public class JoyResponse
    {
         /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IgnoreTextResponse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Payload> Payload { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ReplyMetadata ReplyMetadata { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReplyMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public string ActionRequest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedValue { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public static class WelcomeMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userResponse"></param>
        /// <returns></returns>

        internal static string Message(string userResponse)
        {
            var list = new List<Payload>();
            list.Add(new Payload
            {
                Title = "Raise Helpdesk Ticket",
                Message = "Helpdesk Ticket",
                ReplyMetadata = new ReplyMetadata
                {
                    ActionRequest = "input.helpDesk"
                }
            });

            list.Add(new Payload { Title = "View Payslip", Message = "Sure. For which month do you want?" });
            list.Add(new Payload { Title = "Show absent days", Message = "Sure. " });
            var wehbookResponse = JoyResponseMessage.JoyJsonMessage(userResponse, false, 6, list);
            return wehbookResponse;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public static class HelpdeskMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
       
        internal static string CategoryMessage(string answer, ICollection<CategoryViewModel> categories)
        {
            var list = new List<Payload>();
            foreach (var category in categories)
            {
                list.Add(new Payload
                {
                    Title = category.CategoryName,
                    Message = category.CategoryName,
                    ReplyMetadata = new ReplyMetadata
                    {
                        ActionRequest = "HelpDeskTicket.HelpDeskSubCategory",
                        SelectedValue = category.CategoryId.ToString()
                    }

                });
            }
            var wehbookResponse = JoyResponseMessage.JoyJsonMessage(answer, false, 6, list);
            return wehbookResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="subCategories"></param>
        /// <returns></returns>
        internal static string SubCategoryMessage(string answer, ICollection<SubcategoryEntity> subCategories)
        {
            var list = new List<Payload>();
            foreach (var subCategory in subCategories)
            {
                list.Add(new Payload
                {
                    Title = subCategory.SubcategoryName,
                    Message = $"Priority of {subCategory.SubcategoryName}",
                    ReplyMetadata = new ReplyMetadata
                    {
                        ActionRequest = "HelpDeskTicketPriority",
                        SelectedValue = subCategory.SubcategoryId.ToString(),
                    }

                });
            }
            var wehbookResponse = JoyResponseMessage.JoyJsonMessage(answer, false, 6, list);
            return wehbookResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="priorities"></param>
        /// <returns></returns>
        internal static string Priority(string answer, ICollection<KeyValueEntity> priorities)
        {
            var list = new List<Payload>();
            foreach (var priority in priorities)
            {
                list.Add(new Payload
                {
                    Title = priority.Text,
                    Message = priority.Text,
                    ReplyMetadata = new ReplyMetadata
                    {
                        ActionRequest = "HelpDeskTicketBooked",
                        SelectedValue = priority.Id.ToString(),
                    }
                });
            }
            var wehbookResponse = JoyResponseMessage.JoyJsonMessage(answer, false, 6, list);
            return wehbookResponse;
        }
    }


    public static class JoyResponseMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isIgnoreTextResponse"></param>
        /// <param name="templateId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string JoyJsonMessage(string message, bool isIgnoreTextResponse, int templateId, ICollection<Payload> payload)
        {
            var model = new JoyResponse
            {
                Message = message,
                IgnoreTextResponse = isIgnoreTextResponse,
                Platform = "kommunicate",
                Metadata = new Metadata
                {
                    ContentType = "300",
                    TemplateId = templateId.ToString(),
                    Payload = payload
                }
            };
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            return json;
        }
    }


}