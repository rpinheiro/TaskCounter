﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.BD.Services;

namespace TaskManagement.BD.Entity
{
    public class IssueExecutionContext : Context
    {
        public DbSet<IssueExecution> IssueExecutions { get; set; }
    }

    public class IssueExecution
    {
        public int Id { get; set; }
        public virtual Issue Issue { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        [NotMapped]
        public DateTime? StartDateTime
        {
            get
            {
                return BdService.ConvertStringToDateTime(StartDate);
            }
            set
            {
                if (value == null)
                    StartDate = string.Empty;
                else
                    StartDate = BdService.ConvertDatetimeToString(value.Value);
            }
        }
        [NotMapped]
        public DateTime? EndDateTime
        {
            get
            {
                return BdService.ConvertStringToDateTime(EndDate);
            }
            set
            {
                if (value == null)
                    EndDate = string.Empty;
                else
                    EndDate = BdService.ConvertDatetimeToString(value.Value);
            }
        }

        //[NotMapped]
        //public DateTime? StartDateTime
        //{
        //    get
        //    {
        //        DateTime dateTime = DateTime.MinValue;
        //        bool bParse = false;
        //        if (!string.IsNullOrEmpty(StartDate))
        //        {
        //            CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-br");
        //            bParse = DateTime.TryParse(StartDate,culture, DateTimeStyles.None, out dateTime);                    
        //        }


        //        return bParse ? (DateTime?)dateTime : null;
        //    }
        //    set
        //    {
        //        if (value.HasValue)
        //        {
        //            StartDate = value.Value.ToString();
        //        }
        //    }
        //}
        
        //[NotMapped]
        //public DateTime? EndDateTime
        //{
        //    get
        //    {
        //        DateTime dateTime = DateTime.MinValue;
        //        bool bParse = false;
        //        if (!string.IsNullOrEmpty(EndDate))
        //        {
        //            CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-br");
        //            bParse = DateTime.TryParse(EndDate, culture, DateTimeStyles.None, out dateTime);
        //        }

        //        return bParse ? (DateTime?)dateTime : null;
        //    }
        //    set
        //    {
        //        if (value.HasValue)
        //        {
        //            EndDate = value.Value.ToString();
        //        }
        //    }
        //}
    }
}
