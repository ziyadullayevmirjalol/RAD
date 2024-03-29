using RAD_BackEnd.DTOs.Users;

namespace RAD_BackEnd.DTOs.Events;
public record EventCreateModel(
    long UserId ,
    UserViewModel User,
    string Title ,
    string Description,
    DateTime Start_Time,
    DateTime End_Time ,
    string Location,
    DateTime Reminder_DateTime);

