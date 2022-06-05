using System;
using System.Collections.Generic;

namespace AlutaApp.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }

        public int? InstitutionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? YearOfAdmission { get; set; }

        public string Name { get; set; }

        public string GroupPhotoLink { get; set; }

        public List<ChatGroupMessage> Messages { get; set; }

        public List<ChatGroupUser> Users { get; set; }

        public ChatGroup()
        {
        }
    }
}
