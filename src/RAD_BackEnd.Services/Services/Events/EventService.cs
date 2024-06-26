﻿using RAD_BackEnd.DataAccess.UnintOfWorks;
using RAD_BackEnd.Domain.Entities;
using RAD_BackEnd.Services.Exceptions;
using RAD_BackEnd.Services.Services.Users;

namespace RAD_BackEnd.Services.Services.Events;

public class EventService(IUserService userService, IUnitOfWork unitOfWork) : IEventService
{
    public async ValueTask<Event> CreateAsync(Event @event)
    {
        var existEvent = await unitOfWork.Events.SelectAsync(
            expression: e => e.Title == @event.Title && !e.IsDeleted);

        if (existEvent is not null)
            throw new AlreadyExistException($"Event with Title ({@event.Title} is already exists)");

        var existUser = await userService.GetByIdAsync(@event.UserId);

        var created = await unitOfWork.Events.InsertAsync(@event);
        await unitOfWork.SaveAsync();

        created.User = existUser;

        return created;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existEvent = await unitOfWork.Events.SelectAsync(
            expression: e => e.Id == id && !e.IsDeleted)
            ?? throw new NotFoundException($"Event with Id ({id}) is not found");

        await unitOfWork.Events.DeleteAsync(existEvent);
        await unitOfWork.SaveAsync();

        return true;
    }

    public async ValueTask<IEnumerable<Event>> GetAllAsync()
    {
        var Events = await unitOfWork.Events.SelectAsEnumerableAsync(
            expression: e => !e.IsDeleted,
            includes: ["User"]);

        return Events;
    }

    public async ValueTask<Event> GetByIdAsync(long id)
    {
        var existEvent = await unitOfWork.Events.SelectAsync(
            expression: e => e.Id == id && !e.IsDeleted,
            includes: ["User"])
            ?? throw new NotFoundException($"Event with Id({id}) is not found");

        return existEvent;
    }

    public async ValueTask<Event> UpdateAsync(long id, Event @event)
    {
        var existEvent = await unitOfWork.Events.SelectAsync(
            expression: e => e.Id == id && !e.IsDeleted)
            ?? throw new NotFoundException($"Event with Id ({id}) is not found");

        var existUser = await userService.GetByIdAsync(@event.UserId);

        existEvent.UserId = @event.UserId;
        existEvent.User = existUser;
        existEvent.StartTime = @event.StartTime;
        existEvent.EndTime = @event.EndTime;
        existEvent.Title = @event.Title;
        existEvent.Description = @event.Description;
        existEvent.Location = @event.Location;
        existEvent.Reminder = @event.Reminder;

        var updated = await unitOfWork.Events.UpdateAsync(existEvent);
        await unitOfWork.SaveAsync();

        return updated;
    }
}
