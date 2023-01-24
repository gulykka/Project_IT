using DataBase.Models;
using Domain;

namespace DataBase.Converters;

public static class ReceptionConverter
{
    public static ReceptionModel ToModel(this Reception domainReception)
    {
        return new ReceptionModel
        {
            Id = domainReception.Id,
            EndTime = domainReception.EndTime,
            StartTime = domainReception.StartTime,
            UserId = domainReception.UserId,
            DoctorId = domainReception.DoctorId
        };
    }

    public static Reception ToDomain(this ReceptionModel receptionModel)
    {
        return new Reception(
            receptionModel.Id,
            receptionModel.EndTime,
            receptionModel.StartTime,
            receptionModel.UserId,
            receptionModel.DoctorId
        );
    }
}