using AutoMapper;
using MIMS.Education.Common;
using MIMS.Education.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MIMS.Education.DTOs
{
    public class CourseDTO : DTO
    {
        public override string Key => $"/{CountryCode}/course/{CourseNo}".ToLower();

        #region Index
        public int CourseNo { get; set; }
        public string CourseHash { get; set; }
        #endregion

        //MEDU-450 Course title text to accept upto 140 chars but have warning at 120 chars
        [Required, MaxLength(140)]
        public string Title { get; set; } //aka Title max 120 => 140 chars

        #region Course Status
        /// <summary>
        /// https://mimshub.atlassian.net/wiki/spaces/MIMSCOM/pages/1523744795/050.10.03.03+Course+Status+Matrix
        /// </summary>
        public string Status
        {
            get
            {
                status = CourseStatus.Draft;

                if (DeletedAt is not null)
                {
                    status = CourseStatus.Deleted;
                }
                else if (PublishedAt is not null)
                {
                    if (PausedAt is not null)
                    {
                        status = CourseStatus.Paused;
                    }
                    else if (StoppedAt is not null)
                    {
                        status = CourseStatus.Stopped;
                    }
                    else if (StartAt.Value.Date > DateTime.Now.Date)
                    {
                        status = CourseStatus.Queued;
                    }
                    //MEDU-553 Course Enrollment - Expiry date hotfix
                    else if (EndAt.Value.Date < DateTime.Now.Date)
                    {
                        status = CourseStatus.Expired;
                    }
                    // Limited Seats
                    else if (IsLimitedSeats)
                    {
                        if (PublicEnrolled < SeatsQuota)
                        {
                            status = CourseStatus.Open;
                        }
                        else
                        {
                            status = CourseStatus.Full;
                        }
                    }
                    else
                    {
                        status = CourseStatus.Live;
                    }
                }

                return status;
            }
            init
            {
                status = value;
            }
        }
        string status;

        public bool IsOpen => Status.Equals(CourseStatus.Open);
        public bool IsFull => Status.Equals(CourseStatus.Full);
        public bool IsExpired => Status.Equals(CourseStatus.Expired);
        public bool IsPaused => Status.Equals(CourseStatus.Paused);
        public bool IsStoopped => Status.Equals(CourseStatus.Stopped);
        public bool IsQueued => Status.Equals(CourseStatus.Queued);
        public bool IsDraft => Status.Equals(CourseStatus.Draft);

        #endregion

        public string CountryCode { get; set; }
        public string Profession { get; set; }

        [Required]
        [Display(Name = "Module Code")]
        public string ModuleCode 
        { 
            get => moduleCode; 
            set => moduleCode = value.ToUpperTrimmed();  
        }
        private string moduleCode;

        [Required]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        public string PublicUrl => $"{baseUrl}/{CountryCode}/course/{CourseNo}".ToLower();
        public string PrivateUrl => $"{baseUrl}/{CountryCode}/course/{CourseHash}".ToLower();
        private string baseUrl => (CountryCode == "MY") ?
                    $"{Environment.GetEnvironmentVariable("MIMS_CPD_WEB")}" :
                    $"{Environment.GetEnvironmentVariable("MIMS_MEDU_APP")}";

        //Information
        [Required]
        [Display(Name = "Course Thumbnail")]
        public string Thumbnail { get; set; }

        [Required]
        [Display(Name = "Short Description")]
        public string Introduction { get; set; }
        public string Length { get; set; }
        public string Remarks { get; set; }

        public string LegacyUrl { get; set; }
        public string LegacyId { get; set; }


        //Access Mode
        public bool? PublicAccess { get; set; }
        public bool? PrivateAccess { get; set; }


        //Enrollment
        public int PublicEnrolled { get; set; } // 0 ~ *
        public int PrivateEnrolled { get; set; } // 0 ~ *
        public bool IsLimitedSeats { get; set; } // yes or no
        public int SeatsQuota { get; set; }
        public DateTime? FullAt { get; set; }


        //Validity
        [Required]
        [Display(Name = "Start Date")]
        public DateTime? StartAt { get; set; }
        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime? EndAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? StoppedAt { get; set; }
        public DateTime? PausedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        //Sponsor
        [Required]
        [Display(Name = "Client")]
        public string Sponsor { get; set; }
        public string ProgramCode { get; set; } // Event ID
        public string ProgramProvider { get; set; } // CME Service Provider
        [MaxLength(20)]
        [Display(Name = "CPD Points")]
        public string CPDPoints { get; set; }

        //Certificate
        public bool HavingCertificate { get; set; }
        public Guid? CertificateId { get; set; }
        public CertificateDTO Certificate { get; set; }


        [Required]
        [Display(Name = "Attribution Mode")]
        public string AttributionMode { get; set; } // For reporting


        //Quiz
        public bool HavingQuiz { get; set; }
        public int MaxAttempts { get; set; }
        public double PassingScore { get; set; }
        public QuizDTO Quiz { get; set; }


        public CourseDetailDTO CourseDetail { get; set; }

        public string Specilties { get; set; }
        public List<string> SpecialtyList => Specilties?.Split(",")?.ToList() ?? new List<string>();


        /// <summary>
        /// MIMS Independent Course (MIMS Brand)
        /// <para>https://mimshub.atlassian.net/browse/MEDU-237</para>
        /// </summary>
        public bool IsMIMSBrand { get; set; } = true;

        #region Backing Fields
        //[Required]
        //[Display(Name = "Full Description")]
        //public string description => CourseDetail.Description;

        //Publish, Stop, Pause, Update
        public string Action { get; set; }
        public int SeatsLeft => SeatsQuota - PublicEnrolled;

        QuizDTO quizCopy;
        CourseDTO dtoCopy;
        CourseDetailDTO courseDetailCopy;
        #endregion

        public CourseDTO Backup()
        {
            dtoCopy = this.Copy();
            quizCopy = dtoCopy.Quiz.Copy();
            courseDetailCopy = dtoCopy.CourseDetail.Copy();
            return dtoCopy;
        }
        public CourseDTO Restore()
        {
            var item = dtoCopy?.Copy();
            item.Quiz = quizCopy.Copy();
            item.courseDetailCopy = courseDetailCopy.Copy();
            return item;
        }
        public CourseDTO Copy()
        {
            dtoCopy = (CourseDTO)this.MemberwiseClone();
            dtoCopy.Quiz = Quiz.Copy();
            dtoCopy.CourseDetail = CourseDetail.Copy();
            return dtoCopy;
        }
    }
}

