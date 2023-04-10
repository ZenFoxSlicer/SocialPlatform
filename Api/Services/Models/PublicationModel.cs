using App.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace App.Service.Models
{
    public class PublicationModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Labels { get; set; }
        public DateTime DateTime { get; set; }
        public UserModel Author{ get; set; }
    }
}
