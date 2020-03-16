using JRPC_HMS.Data;
using JRPC_HMS.Models;
using JRPC_HMS.Services.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace JRPC_HMS.Pages
{
    public class FBFormModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEmailSender _emailSender;

        public FBFormModel(ApplicationDbContext context, IHostingEnvironment environment, IEmailSender emailSender)
        {
            _context = context;
            _hostingEnvironment = environment;
            _emailSender = emailSender;
        }

        #region Properties
        public IList<Customer> Customer { get; set; }
        [BindProperty]
        public int? FormID { get; set; }
        public IList<Question> Questions { get; set; }
        public string FormName { get; set; }
        public int Form_ID { get; set; }
        public IList<Feedback> Feedbacks { get; set; }
        public IList<Form> Forms { get; set; }
        public Form TheForm { get; set; }
        [BindProperty]
        public IList<Answer> Answers { get; set; }
        [BindProperty]
        [Required]
        public string Name { get; set; }
        [BindProperty]
        [Required]
        public string Email { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Waitron { get; set; }
        [BindProperty]
        public string FormComments { get; set; }
        [BindProperty]
        public string q1 { get; set; }
        [BindProperty]
        public string q2 { get; set; }
        [BindProperty]
        public string q3 { get; set; }
        [BindProperty]
        public string q4 { get; set; }
        [BindProperty]
        public string q5 { get; set; }
        [BindProperty]
        public string q6 { get; set; }
        [BindProperty]
        public string q7 { get; set; }
        [BindProperty]
        public string q8 { get; set; }
        [BindProperty]
        public string q9 { get; set; }
        [BindProperty]
        public string q10 { get; set; }
        [BindProperty]
        public IFormFile UploadImage { set; get; }
        [BindProperty]
        public string FormSelected { get; set; }
        [BindProperty]
        public string ContactedString { get; set; }
        #endregion

        #region Generate IDs
        protected string GenerateID()
        {
            string id = string.Empty;
            int count = 0;
            if (_context.Feedbacks.ToList().Count > 0)
            {
                id = _context.Feedbacks.LastOrDefault().RefNo;
                id = id.Remove(0, 3);
                count = Convert.ToInt32(id);
            }
            else
            {
                count = 0;
            }

            count++;
            id = string.Format("REF{0}", count.ToString("0000000"));
            return id;
        }
        #endregion

        #region OnGet
        public IActionResult OnGet()
        {
            FormSelected = "";
            Forms = _context.Forms.ToList();
            Questions = _context.Questions.ToList();
            return Page();
        }

        public IActionResult OnPostSelectedForm(string formname)
        {
            TheForm = _context.Forms.FirstOrDefault(f => f.FormName == formname);
            FormSelected = TheForm.FormName;
            Questions = _context.Questions.Where(q => q.Form_ID == TheForm.Form_ID).ToList();
            Feedbacks = _context.Feedbacks.ToList();
            return Page();
        }
        #endregion

        #region OnPost
        public IActionResult OnPost(int id)
        {
            Forms = _context.Forms.ToList();
            Form_ID = Forms.FirstOrDefault(f => f.Form_ID == id).Form_ID;
            Questions = _context.Questions.Where(q => q.Form_ID == id).ToList();
            string refNo = GenerateID();
            string status = string.Empty;
            if (q1 == "20" || q1 == "40" || q1 == "60" ||
                q2 == "20" || q2 == "40" || q2 == "60" ||
                q3 == "20" || q3 == "40" || q3 == "60" ||
                q4 == "20" || q4 == "40" || q4 == "60" ||
                q5 == "20" || q5 == "40" || q5 == "60" ||
                q6 == "20" || q6 == "40" || q6 == "60" ||
                q7 == "20" || q7 == "40" || q7 == "60" ||
                q8 == "20" || q8 == "40" || q8 == "60" ||
                q9 == "20" || q9 == "40" || q9 == "60" ||
                q10 == "20" || q10 == "40" || q10 == "60")
            {
                status = "NEGATIVE";
            }
            else
            {
                status = "POSITIVE";
            }
            string imagePath = string.Empty;
            if (UploadImage != null)
            {
                var fileName = Path.Combine(_hostingEnvironment.WebRootPath, "uploadedImages", UploadImage.FileName);
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    UploadImage.CopyTo(fileStream);
                }
                imagePath = "\\uploadedImages\\" + UploadImage.FileName; // Set the file name
            }

            Customer = _context.Customers.ToList();
            Customer cust = new Customer();
            cust = _context.Customers.FirstOrDefault(c => c.Email == Email.ToLower());
            if (cust == null)
            {
                Customer customer = new Customer
                {
                    Name = Name,
                    Email = Email.ToLower(),
                    Phone = Phone

                };
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }
            List<string> answers = new List<string>();
            answers.Add(q1);
            answers.Add(q2);
            answers.Add(q3);
            answers.Add(q4);
            answers.Add(q5);
            answers.Add(q6);
            answers.Add(q7);
            answers.Add(q8);
            answers.Add(q9);
            answers.Add(q10);
            double answerPerc = 0;
            for (int a = 0; a < Questions.Count; a++)
            {
                if (answers[a] != "")
                {
                    answerPerc += Convert.ToDouble(answers[a]);
                }
                else
                {
                    answerPerc += 0;
                }
                Answer answer = new Answer
                {
                    Question_ID = Questions[a].Question_ID,
                    QAnswer = answers[a],
                    Form_ID = Form_ID,
                    Feedback_ID = refNo
                };
                _context.Answers.Add(answer);
                _context.SaveChanges();
            }
            answerPerc = answerPerc / Questions.Count;
            answerPerc = Math.Round(answerPerc);
            cust = new Customer();
            cust = _context.Customers.FirstOrDefault(c => c.Email == Email.ToLower());
            Feedback feedback = new Feedback
            {
                RefNo = refNo,
                Customer_ID = cust.Id,
                Comments = FormComments,
                Servant = Waitron,
                Form_ID = Form_ID,
                SubmittedDate = DateTime.Now,
                Notes = "",
                Status = status,
                RefScore = answerPerc.ToString(),
                Photo = imagePath,
                ContactCustomer = ContactedString
            };

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            SendEmail(status, feedback, cust);
            SendMailTo(status, feedback, cust);

            return RedirectToPage("PostForm", new { refno = feedback.RefNo });
        }
        #endregion

        #region SendEmail()
        protected void SendEmail(string status, Feedback feedback, Customer customer)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var bodyBuilder = new BodyBuilder();
                var pathImage = Path.Combine(_hostingEnvironment.WebRootPath, "images/Texican_Logo_Blue.jpg");
                var image = bodyBuilder.LinkedResources.Add(pathImage);
                image.ContentId = MimeUtils.GenerateMessageId();
                bodyBuilder.HtmlBody = string.Format(@"<html><body><div style='text-align:center;'><img src='cid:{0}' style='width:60%;height:auto;' /></div><br /><br /><b>Dear {1}</b><br /><br />Thank you for providing us with your valuable feedback.<br /><br />Have a great day!<br /><br />Kind Regards<br />Texican</body></html>", image.ContentId, customer.Name);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Texican", "info@jrpc-solutions.com"));
                message.To.Add(new MailboxAddress(customer.Name, customer.Email));
                message.Subject = "Texican Feedback Response";
                message.Body = bodyBuilder.ToMessageBody();

                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect("41.185.13.224", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("info@jrpc-solutions.com", "JrpcSolutions17");
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void SendMailTo(string status, Feedback feedback, Customer customer)
        {
            try
            {
                var bodyBuilder = new BodyBuilder();
                var pathImage = Path.Combine(_hostingEnvironment.WebRootPath, "images/Texican_Logo_Blue.jpg");
                var image = bodyBuilder.LinkedResources.Add(pathImage);
                image.ContentId = MimeUtils.GenerateMessageId();
                bodyBuilder.HtmlBody = string.Format(@"<html><body><div style='text-align:center;'><img src='cid:{0}' style='width:60%;height:auto;' /></div><br /><br /><b>Dear {1}</b><br /><br />You have received a " + status + " review.<br /><br />Kind Regards<br />Texican</body></html>", image.ContentId, _context.Forms.FirstOrDefault(f => f.Form_ID == feedback.Form_ID).FormName);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Texican", "info@jrpc-solutions.com"));
                message.To.Add(new MailboxAddress("Rudi", "roedolf.bothma.123@gmail.com"));
                message.Subject = "Texican Feedback: " + feedback.RefNo;
                message.Body = bodyBuilder.ToMessageBody();

                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect("41.185.13.224", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("info@jrpc-solutions.com", "JrpcSolutions17");
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion

        #region Calculations
        protected double StorePercentage(string q1, string q2, string q3, string q4, string q5)
        {
            double percT = 0, avg = 0, perc = 0;
            int rate1, rate2, rate3, rate4, rate5, total;
            int count100 = 0, count80 = 0, count60 = 0, count40 = 0, count20 = 0;
            List<string> answers = new List<string>();
            answers.Add(q1);
            answers.Add(q2);
            answers.Add(q3);
            answers.Add(q4);
            answers.Add(q5);
            foreach (string answer in answers)
            {
                if (answer == "100")
                {
                    count100++;
                }
                else if (answer == "80")
                {
                    count80++;
                }
                else if (answer == "60")
                {
                    count60++;
                }
                else if (answer == "40")
                {
                    count40++;
                }
                else if (answer == "20")
                {
                    count20++;
                }
            }
            rate5 = count100;
            rate4 = count80;
            rate3 = count60;
            rate2 = count40;
            rate1 = count20;
            percT = (rate5 * 100) + (rate4 * 60) + (rate3 * 40) + (rate2 * 20) + (rate1 * 0);
            total = rate5 + rate4 + rate3 + rate2 + rate1;

            if (percT > 0)
            {
                avg = percT / total;
            }
            perc = Math.Round(perc, 2);
            return perc;
        }
        #endregion
    }
}