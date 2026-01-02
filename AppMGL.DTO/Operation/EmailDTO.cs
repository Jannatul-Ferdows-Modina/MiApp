using System;


namespace AppMGL.DTO.Operation
{
    public class EmailDTO
    {
        public string Email { get; set; }
    }


    public class EmailDetail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Uuid { get; set; }

        public string createdby { get; set; }
    }
}
