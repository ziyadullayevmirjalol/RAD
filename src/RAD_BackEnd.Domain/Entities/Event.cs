﻿using RAD_BackEnd.Domain.Commons;
using RAD_BackEnd.DTOs.Users;

namespace RAD_BackEnd.Domain.Entities;
public class Event : Auditable
{
    public long UserId { get; set; }
    public UserViewModel User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public DateTime Reminder { get; set; }
}
