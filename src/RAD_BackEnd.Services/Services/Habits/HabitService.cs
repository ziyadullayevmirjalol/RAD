﻿using RAD_BackEnd.DataAccess.UnintOfWorks;
using RAD_BackEnd.Domain.Entities;
using RAD_BackEnd.Services.Exceptions;
using RAD_BackEnd.Services.Services.Users;

namespace RAD_BackEnd.Services.Services.Habits;

public class HabitService(IUserService userService, IUnitOfWork unitOfWork) : IHabitService
{
    public async ValueTask<Habit> CreateAsync(Habit habit)
    {
        var existUser = await userService.GetByIdAsync(habit.UserId);

        var existHabit = await unitOfWork.Habits.SelectAsync(
            expression: h => h.Name == habit.Name && !h.IsDeleted);

        if (existHabit is not null)
            throw new AlreadyExistException($"Habit with name ({habit.Name} is already exists)");

        var created = await unitOfWork.Habits.InsertAsync(habit);
        await unitOfWork.SaveAsync();

        created.User = existUser;

        return created;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existHabit = await unitOfWork.Habits.SelectAsync(
            expression: h => h.Id == id && !h.IsDeleted)
            ?? throw new NotFoundException($"Habit with Id ({id}) is not found");

        await unitOfWork.Habits.DeleteAsync(existHabit);
        await unitOfWork.SaveAsync();

        return true;
    }

    public async ValueTask<IEnumerable<Habit>> GetAllAsync()
    {
        var Habits = await unitOfWork.Habits.SelectAsEnumerableAsync(
            expression: h => !h.IsDeleted,
            includes: ["User"]);

        return Habits;
    }

    public async ValueTask<Habit> GetByIdAsync(long id)
    {
        var existHabit = await unitOfWork.Habits.SelectAsync(
            expression: h => h.Id == id && !h.IsDeleted,
            includes: ["User"])
            ?? throw new NotFoundException($"Habit with Id ({id}) is not found");

        return existHabit;
    }

    public async ValueTask<Habit> UpdateAsync(long id, Habit habit)
    {
        var existHabit = await unitOfWork.Habits.SelectAsync(
            expression: h => h.Id == id && !h.IsDeleted)
            ?? throw new NotFoundException($"Habit with Id ({id}) is not found");

        var existUser = await userService.GetByIdAsync(habit.UserId);

        existHabit.Name = habit.Name;
        existHabit.Description = habit.Description;
        existHabit.StartDate = habit.StartDate;
        existHabit.EndDate = habit.EndDate;
        existHabit.LastCompletedDate = habit.LastCompletedDate;
        existHabit.BestSteak = habit.BestSteak;
        existHabit.Steak = habit.Steak;
        existHabit.Frequenty = habit.Frequenty;
        existHabit.UserId = habit.UserId;
        existHabit.User = existUser;

        var updated = await unitOfWork.Habits.UpdateAsync(existHabit);
        await unitOfWork.SaveAsync();

        return updated;
    }
}
