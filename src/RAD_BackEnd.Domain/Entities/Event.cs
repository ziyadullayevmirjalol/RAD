﻿using RAD_BackEnd.Domain.Commons;

namespace RAD_BackEnd.Domain.Entities;
public class Event : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public DateTime Reminder { get; set; }
}
