using Microsoft.AspNetCore.Html;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.ViewModels
{
    public class DocumentViewModel
    {
        [Display(Name = "Document Id")]
        public Guid? DocumentId { get; set; }

        [Display(Name = "Canonical Name")]
        public string CanonicalName { get; set; }

        [Display(Name = "Sequence Number")]
        public long SequenceNumber { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastReviewed { get; set; }

        public HtmlString Markup { get; set; }
    }
}