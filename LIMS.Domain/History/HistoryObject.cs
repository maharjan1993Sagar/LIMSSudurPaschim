﻿using System;

namespace LIMS.Domain.History
{
    public class HistoryObject: BaseEntity
    {
        public DateTime CreatedOnUtc { get; set; }
        public BaseEntity Object { get; set; }
    }
}
