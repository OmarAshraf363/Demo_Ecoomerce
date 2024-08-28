using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Demo.Models;

namespace Demo.Check
{
    public static class Methods
    {

        public static string StaticData_CustomerRole = "Customer";
        public static string StaticData_AdminRole = "Admin";

        public static string StaticDataSuccessPayment = "Success-Confirmed-Via-Admin";
        public static string StaticDataRefundedPayment = "Payment-Refunded";
        public static string StaticDataInProcessPayment = "In-Process";





        public static bool SendConfirmationEmail(string userEmail, Order order)
        {

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(userEmail);
                mail.From = new MailAddress("oa38150@gmail.com");
                mail.Subject = "Booking Confirmation";
                mail.Body = $"Dear {order.AppUser.UserName},THank you";
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com ",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("oa38150@gmail.com", "deeo bpmm pmty pzcf"),
                    EnableSsl = true,

                };


                smtp.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public static JsonResult CheckValidation(ModelStateDictionary modelState, HttpRequest request, bool state)
        {

            if (request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (state != true)
                {

                    var nameErrors = modelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToList());

                    return new JsonResult(new { isvalid = false, nameErrors });
                }
                else
                {
                    return new JsonResult(new { isvalid = true });

                }
            }
            return null!;




        }

    }
}